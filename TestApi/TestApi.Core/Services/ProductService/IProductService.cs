using TestApi.Contracts.Models;

namespace TestApi.Core.Services.ProductService
{
    /// <summary>
    /// Интерфейс сервиса по работе с товарами.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Возвращает список товаров.
        /// </summary>
        /// <param name="request">Запрос на получение товаров.</param>
        public PagedResult<ProductDto> GetProducts(ProductsListRequest request);
    }
}
