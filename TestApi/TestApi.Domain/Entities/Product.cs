using TestApi.Domain.Entities.Base;

namespace TestApi.Domain.Entities
{
    /// <summary>
    /// Доменная модель товара.
    /// </summary>
    public class Product : BaseEntityWithId
    {
        /// <summary>
        /// Наименование товараю.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal Price { get; set; }
    }
}
