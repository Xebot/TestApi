namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Транспортная модель заказа клиента.
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Общая сумма заказа.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Описание заказа.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Размер скидки по заказу.
        /// </summary>
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// Товары в заказе.
        /// </summary>
        public OrderItemDto[] OrderItems { get; set; }
    }
}
