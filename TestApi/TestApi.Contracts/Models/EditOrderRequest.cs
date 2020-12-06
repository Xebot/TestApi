namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Запрос на изменение заказа.
    /// </summary>
    public class EditOrderRequest
    {
        /// <summary>
        /// Номер заказа.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Товары.
        /// </summary>
        public OrderItemDto[] Items { get; set; }
    }
}
