namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Сокращенная информация по заказам клиента.
    /// </summary>
    public class OrderShortInfoDto
    {
        /// <summary>
        /// Итоговая сумма заказа.
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
    }
}
