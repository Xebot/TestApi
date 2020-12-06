namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Запрос на создание заказа.
    /// </summary>
    public class MakeOrderRequest
    {
        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Описание заказа.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Товары.
        /// </summary>
        public OrderItemDto[] Items { get; set; }
    }
}
