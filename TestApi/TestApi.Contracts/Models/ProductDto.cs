namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Транспортная модель товара.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Уникальный номер товара.
        /// </summary>
        public int ProductNumber { get; set; }

        /// <summary>
        /// Наименование товара.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal Price { get; set; }
    }
}
