using System.Collections.Generic;
using TestApi.Domain.Entities.Base;

namespace TestApi.Domain.Entities
{
    /// <summary>
    /// Доменная сущность пользователя.
    /// </summary>
    public class Customer : BaseEntityWithId
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Адрес пользователя.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Является ли ВИП-пользователем.
        /// </summary>
        public bool IsVip { get; set; }

        /// <summary>
        /// Заказы пользователя.
        /// </summary>
        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
