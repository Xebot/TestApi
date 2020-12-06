namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Запрос на получение деталей заказа.
    /// </summary>
    public class OrderDetailsRequest
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Номер заказа.
        /// </summary>
        public int OrderNumber { get; set; }
    }
}
