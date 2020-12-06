namespace TestApi.Contracts.Enums
{
    /// <summary>
    /// Перечисление с действиями при изменении заказа.
    /// </summary>
    public enum ChangeStateEnum
    {
        /// <summary>
        /// Добавить товар.
        /// </summary>
        Add = 1,

        /// <summary>
        /// Изменить количество товара.
        /// </summary>
        ChangeQuantity = 2,
        
        /// <summary>
        /// Удалить позицию из заказа.
        /// </summary>
        Delete = 3
    }
}
