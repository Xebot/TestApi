namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Модель для запроса списка товаров.
    /// </summary>
    public class ProductsListRequest
    {
        /// <summary>
        /// Номер страницы.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Количество элементов на странице.
        /// </summary>
        public int Take { get; set; }
    }
}
