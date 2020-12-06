using System;
using System.Collections.Generic;
using System.Text;
using TestApi.Domain.Entities.Base;

namespace TestApi.Domain.Entities
{
    /// <summary>
    /// Доменная сущность заказа.
    /// </summary>
    public class Order : BaseEntityWithId
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
        /// Размер скидки по заказу в %.
        /// </summary>
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// Ссылка на клиента.
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Позиции заказа.
        /// </summary>
        public virtual List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
