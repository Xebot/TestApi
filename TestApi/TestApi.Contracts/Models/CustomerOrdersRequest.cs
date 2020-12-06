namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Запрос на получение заказов клиента.
    /// </summary>
    public class CustomerOrdersRequest
    {
        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Номер страницы.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Кол-во записей на страницу.
        /// </summary>
        public int PageSize { get; set; }
    }
}
