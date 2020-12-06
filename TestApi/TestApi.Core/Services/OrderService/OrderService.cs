using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Contracts.Models;
using TestApi.Domain.Entities;
using TestApi.Infrastructure;

namespace TestApi.Core.Services.OrderService
{
    /// <summary>
    /// Сервис по работе с заказами.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            AppDbContext context,
            IMapper mapper,
            ILogger<OrderService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<PagedResult<OrderShortInfoDto>> GetCustomerOrdersShortInfo(CustomerOrdersRequest request)
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);

            if (customer == null)
            {
                var error = $"Пользователь с id = {request.CustomerId} не найден";
                _logger.LogError(error);
                return new PagedResult<OrderShortInfoDto>(isSuccess: false, errors: new string[] { error });
            }

            if (customer.Orders.Count == 0)
            {
                return new PagedResult<OrderShortInfoDto>(totalCount: customer.Orders.Count, currentPage: request.PageNumber, pageSize: request.PageSize);
            }

            var orders = _context.Orders
                .Where(o => o.Customer.Id == customer.Id)
                .Skip(request.PageNumber * request.PageSize)
                .Take(request.PageSize);

            return new PagedResult<OrderShortInfoDto>(
                totalCount: customer.Orders.Count,
                currentPage: request.PageNumber,
                pageSize: request.PageSize,
                items: orders.Select(o => _mapper.Map<OrderShortInfoDto>(o)).ToArray(),
                isSuccess: true);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<OrderDto>> GetOrderDetails(OrderDetailsRequest request) 
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);

            if (customer == null)
            {
                var error = $"Пользователь с id = {request.CustomerId} не найден";
                _logger.LogError(error);
                return new ServiceResult<OrderDto>(isSuccess: false, errors: new string[] { error });
            }

            var order = customer.Orders.Where(o => o.Id == request.OrderNumber).FirstOrDefault();

            if (order == null)
            {
                var error = $"Заказ с id = {request.OrderNumber} не найден";
                _logger.LogError(error);
                return new ServiceResult<OrderDto>(isSuccess: false, errors: new string[] { error });
            }

            return new ServiceResult<OrderDto>(
                item: _mapper.Map<OrderDto>(order),
                isSuccess: true);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<OrderDto>> MakeOrder(MakeOrderRequest request)
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);

            if (customer == null)
            {
                var error = $"Пользователь с id = {request.CustomerId} не найден";
                _logger.LogError(error);
                return new ServiceResult<OrderDto>(isSuccess: false, errors: new string[] { error });
            }

            var productsIds = request.Items.Select(x => x.ProductNumber).ToArray();
            var productsCount = _context.Products.Where(p => productsIds.Contains(p.Id)).Count();
            if (productsCount == 0)
            {
                return new ServiceResult<OrderDto>(isSuccess: false, errors: new string[] { "Не найдены товары с такими Id" });
            }

            var products = _context.Products.Where(p => productsIds.Contains(p.Id)).ToArray();
            if (productsCount != productsIds.Length)
            {
                var missedIds = productsIds.Except(products.Select(x => x.Id)).ToArray();
                return new ServiceResult<OrderDto>(isSuccess: false, errors: new string[] { $"Не найдены товары с такими Id: {string.Join(",", missedIds)}" });
            }
            
            var newOrder = new Order { Description = request.Description, Customer = customer };

            foreach(var item in request.Items)
            {
                var product = products.Where(x => x.Id == item.ProductNumber).FirstOrDefault();

                AddItemToOrder(newOrder, product, item);
            }

            UpdateOrderTotalPrice(newOrder);
            CalculateDiscount(newOrder);

            try
            {
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.LogError("Произошла ошибка при обновлении заказа.", e);
                return new ServiceResult<OrderDto>(isSuccess: false, errors: new string[] { $"Произошла непредвиденная ошибка." });
            }            

            return new ServiceResult<OrderDto>(item: _mapper.Map<OrderDto>(newOrder), isSuccess: true);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<OrderDto>> EditOrder(EditOrderRequest request)
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);

            if (customer == null)
            {
                var error = $"Пользователь с id = {request.CustomerId} не найден";
                _logger.LogError(error);
                return new ServiceResult<OrderDto>(isSuccess: false, errors: new string[] { error });
            }

            var order = customer.Orders.Where(o => o.Id == request.OrderNumber).FirstOrDefault();

            if (order == null)
            {
                var error = $"Заказ с номером {request.OrderNumber} не найден";
                _logger.LogError(error);
                return new ServiceResult<OrderDto>(isSuccess: false, errors: new string[] { error });
            }

            var errors = await EditOrder(order, request.Items);             

            return errors.Count != 0
                ? new ServiceResult<OrderDto>(item: _mapper.Map<OrderDto>(order), isSuccess: true, errors: errors.ToArray())
                : new ServiceResult<OrderDto>(item: _mapper.Map<OrderDto>(order), isSuccess: true);
        }

        /// <summary>
        /// Редактирует заказ.
        /// </summary>
        /// <param name="order">Заказ клиента.</param>
        /// <param name="itemsDto">Транспортная модель позиций заказа.</param>
        public async Task<List<string>> EditOrder(Order order, OrderItemDto[] itemsDto)
        {
            var errors = new List<string>();

            foreach (var item in itemsDto)
            {
                try
                {
                    await UpdateOrderItem(order, item);
                }
                catch (Exception e)
                {
                    errors.Add(e.Message);
                }
            }

            UpdateOrderTotalPrice(order);
            CalculateDiscount(order);

            await _context.SaveChangesAsync();

            return errors;
        }

        /// <summary>
        /// Обновляет состояние позиции заказа при редактировании заказа.
        /// </summary>
        /// <param name="order">Заказ.</param>
        /// <param name="itemDto">Транспортная модель позиции заказа.</param>
        private async Task UpdateOrderItem(Order order, OrderItemDto itemDto)
        {
            if (order == null || itemDto == null)
                return;

            switch (itemDto?.Action)
            {
                case Contracts.Enums.ChangeStateEnum.Add:
                    await AddItemToOrder(order, itemDto);
                    break;

                case Contracts.Enums.ChangeStateEnum.ChangeQuantity:                    
                    ChangeItemQuantityInOrder(order, itemDto);
                    break;

                case Contracts.Enums.ChangeStateEnum.Delete:                    
                    RemoveItemFromOrder(order, itemDto);
                    break;

                default:
                    throw new Exception("Не задано или неправильное значение ChangeStateEnum");
            }
        }

        /// <summary>
        /// Добавляет позицию заказа.
        /// </summary>
        /// <param name="order">Заказ.</param>
        /// <param name="dto">Транспортная модель позиции заказа.</param>
        private async Task AddItemToOrder(Order order, OrderItemDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductNumber);

            if (product == null)
                throw new ArgumentNullException($"Не удалось добавать товар с номером {dto.ProductNumber} в заказ {order.Id}");

            AddItemToOrder(order, product, dto);
        }

        /// <summary>
        /// Добавляет позицию заказа.
        /// </summary>
        /// <param name="order">Заказ.</param>
        /// <param name="product">Товар.</param>
        /// <param name="dto">Транспортная модель позиции заказа.</param>
        private void AddItemToOrder(Order order, Product product, OrderItemDto dto)
        {
            order.Items.Add(new OrderItem
            {
                Quantity = dto.Quantity,
                Price = product.Price,
                Sum = dto.Quantity * product.Price,
                Product = product,
                Order = order
            });
        }

        /// <summary>
        /// Изменяет количество позиции заказа.
        /// </summary>
        /// <param name="order">Заказ.</param>
        /// <param name="dto">Транспортная модель позиции заказа.</param>
        private void ChangeItemQuantityInOrder(Order order, OrderItemDto dto)
        {
            var orderItem = order.Items.Where(i => i.Product.Id == dto.ProductNumber).FirstOrDefault();

            if (orderItem == null || dto?.Quantity <= 0)
                throw new ArgumentNullException($"Не удалось изменить количество товара {dto.ProductNumber} по заказу {order?.Id}");

            orderItem.Quantity = dto.Quantity;
            orderItem.Sum = orderItem.Price * dto.Quantity;
        }

        /// <summary>
        /// Удаляет позицию заказа.
        /// </summary>
        /// <param name="order">Заказ.</param>
        /// <param name="orderItem">Позиция заказа для удаления.</param>
        private void RemoveItemFromOrder(Order order, OrderItemDto itemDto)
        {
            var orderItem = order.Items.Where(i => i.Product.Id == itemDto.ProductNumber).FirstOrDefault();

            if (orderItem == null)
                throw new ArgumentNullException($"Не удалось удалить товар по заказу {order?.Id}");

            order.Items.Remove(orderItem);
            _context.OrderItems.Remove(orderItem);
        }

        /// <summary>
        /// Обновляет итоговую стоимость заказа.
        /// </summary>
        /// <param name="order">Заказ.</param>
        private void UpdateOrderTotalPrice(Order order)
        {
            if (order == null)
                return;

            order.TotalPrice = order.Items.Sum(x => x.Sum);
        }

        /// <summary>
        /// Считает скидку по заказу.
        /// </summary>
        /// <param name="order">Заказ.</param>
        private void CalculateDiscount(Order order)
        {
            if (order == null)
                return;

            if (!order.Customer.IsVip)
                return;

            var discountAmount = order.Customer.Orders.Count;

            order.DiscountAmount = discountAmount < 50
                ? discountAmount
                : 50;

            order.TotalPrice = order.TotalPrice - (order.TotalPrice * order.DiscountAmount.Value / 100);
        }
    }
}
