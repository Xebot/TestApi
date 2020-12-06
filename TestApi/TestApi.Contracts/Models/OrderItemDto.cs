using TestApi.Contracts.Enums;

namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Транспортная модель единицы заказа.
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// Номер товара.
        /// </summary>
        public int ProductNumber { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Сумма за позицию.
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// Действие при редактировании.
        /// </summary>
        public ChangeStateEnum? Action {get;set;}
    }
}
