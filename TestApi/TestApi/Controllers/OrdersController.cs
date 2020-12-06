using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApi.Contracts.Models;
using TestApi.Core.Services.OrderService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Возвращает список заказов пользователя.
        /// </summary>
        /// <param name="customerId">Идентификатор пользователя.</param>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="pageSize">Количество запрашиваемых элементов.</param>
        public async Task<IActionResult> Get(int customerId, int pageNumber, int pageSize)
        {
            var validationResult = ValidatePagedInput(pageNumber - 1, pageSize, customerId);

            if (validationResult.Length != 0)
                return BadRequest(validationResult);

            var result = await _orderService.GetCustomerOrdersShortInfo(new CustomerOrdersRequest { PageNumber = pageNumber - 1, PageSize = pageSize, CustomerId = customerId });

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Возвращает детализированную информацию о заказе.
        /// </summary>
        /// <param name="customerId">Идентификатор клиента.</param>
        /// <param name="orderNumber">Номер заказа.</param>
        [HttpGet]
        [Route("OrderDetails")]
        public async Task<IActionResult> GetOrderDetails(int customerId, int orderNumber)
        {
            if (customerId <= 0 || orderNumber <= 0)
                return BadRequest("Неправильные входящие параметры. Идентификатор пользователя и номер заказа не могут быть меньше или равны нулю.");

            var result = await _orderService.GetOrderDetails(new OrderDetailsRequest { CustomerId = customerId, OrderNumber = orderNumber });

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Метод для редактирования заказа.
        /// </summary>
        /// <remarks>
        /// В качестве входных параметров требуется указать идентификатор клиента, номер заказа и набор элементов которые предполагается редактировать.
        /// Можно добавить новые элементы, изменить количество у существующих или удалить товары из заказа.
        /// </remarks>
        /// <param name="request">Запрос на редактирование.</param>
        [HttpPost]
        [Route("EditOrder")]
        public async Task<IActionResult> EditOrder([FromBody]EditOrderRequest request)
        {
            if (request == null || request?.CustomerId <= 0 || request?.OrderNumber <= 0 || request?.Items?.Length == 0)
                return BadRequest();

            var result = await _orderService.EditOrder(request);

            return Ok(result);
        }

        /// <summary>
        /// Метод для совершения заказа.
        /// </summary>
        /// <param name="request">Запрос для создания заказа.</param>
        [HttpPost]
        [Route("MakeOrder")]
        public async Task<IActionResult> MakeOrder([FromBody]MakeOrderRequest request)
        {
            if (request == null || request?.CustomerId <= 0 || request?.Items.Length == 0)
                return BadRequest();

            var result = await _orderService.MakeOrder(request);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Errors);
        }        

        /// <summary>
        /// Валидирует входящие параметры.
        /// </summary>
        private string[] ValidatePagedInput(int pageNumber, int pageSize, int? id)
        {
            var result = new List<string>();
            if (pageNumber < 0)
                result.Add("Номер запрашивамоей страницы не может быть меньше нуля");

            if (pageSize <= 0 || pageSize > 100)
                result.Add("Задано неправильно значение параметра pageSize. Оно не может быть меньше либо равно 0 и больше 100 ");

            if (id.HasValue && id <= 0)
                result.Add("Идентификатор не должен быть меньше либо равен нулю.");

            return result.ToArray();
        }
    }
}
