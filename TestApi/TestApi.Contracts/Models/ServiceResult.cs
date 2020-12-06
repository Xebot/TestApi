using System;

namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Модель универсального ответа от сервиса.
    /// </summary>
    public class ServiceResult<T> where T : class
    {
        /// <summary>
        /// Сущность.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// Успешное ли выполнение.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Список ошибок.
        /// </summary>
        public string[] Errors { get;set; }

        public ServiceResult(T item, bool isSuccess)
        {
            Item = item;
            IsSuccess = isSuccess;
            Errors = Array.Empty<string>();
        }

        public ServiceResult(T item, bool isSuccess, string[] errors)
        {
            Item = item;
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public ServiceResult(bool isSuccess, string[] errors)
        {
            Item = null;
            IsSuccess = isSuccess;
            Errors = errors;
        }
    }
}
