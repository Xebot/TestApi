using System.Threading.Tasks;
using TestApi.Contracts.Models;

namespace TestApi.Core.Services.OrderService
{
    /// <summary>
    /// Интерфес по работе с заказами.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Возвращает краткую информацию по заказам пользователя.
        /// </summary>
        /// <param name="request">Запрос на получение заказов.</param>
        Task<PagedResult<OrderShortInfoDto>> GetCustomerOrdersShortInfo(CustomerOrdersRequest request);

        /// <summary>
        /// Возвращает детализацию по конкретному заказу.
        /// </summary>
        /// <param name="request">Запрос на получение заказа.</param>
        Task<ServiceResult<OrderDto>> GetOrderDetails(OrderDetailsRequest request);

        /// <summary>
        /// Создает заказ клиента.
        /// </summary>
        /// <param name="request">Данные для создания заказа.</param>
        Task<ServiceResult<OrderDto>> MakeOrder(MakeOrderRequest request);

        /// <summary>
        /// Редактирует заказ пользователя.
        /// </summary>
        /// <param name="request">Данные для редактирования.</param>
        Task<ServiceResult<OrderDto>> EditOrder(EditOrderRequest request);
    }
}
