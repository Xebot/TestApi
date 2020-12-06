using TestApi.Domain.Entities.Base;

namespace TestApi.Domain.Entities
{
    /// <summary>
    /// Доменная сущность позиции заказа.
    /// </summary>
    public class OrderItem : BaseEntityWithId
    {
        /// <summary>
        /// Количество товара.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Общая стоимость позиции заказа.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Ссылка на заказ.
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Ссылка на товар.
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
