using System;

namespace TestApi.Contracts.Models
{
    /// <summary>
    /// Модель постраничного ответа при запросе сущностей.
    /// </summary>
    /// <typeparam name="T">Запрашиваемая сущность.</typeparam>
    public class PagedResult<T> where T : class
    {
        /// <summary>
        /// Общее количество.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Текущая страница.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Размер страницы.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Сущности.
        /// </summary>
        public T[] Items { get; set; }

        /// <summary>
        /// Признак успешного выполнения.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Список ошибок.
        /// </summary>
        public string[] Errors { get; set; }

        public PagedResult(int totalCount, int currentPage, int pageSize, T[] items, bool isSuccess, string[] errors)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
            Items = items;
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public PagedResult(int totalCount, int currentPage, int pageSize)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
            Items = Array.Empty<T>();
            IsSuccess = true;
            Errors = Array.Empty<string>();
        }

        public PagedResult(int totalCount, int currentPage, int pageSize, T[] items, bool isSuccess)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
            Items = items;
            IsSuccess = isSuccess;
            Errors = Array.Empty<string>();
        }

        public PagedResult(bool isSuccess, string[] errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
            Items = Array.Empty<T>();
        }
    }
}
