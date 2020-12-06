using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TestApi.Core.Services.ProductService;


namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(
            IProductService productService)
        {
            _productService = productService;
        }
        
        /// <summary>
        /// Получает список товаров.
        /// </summary>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        [HttpGet]
        public IActionResult Get(int pageNumber, int pageSize)
        {
            var validationResult = ValidateInput(pageNumber-1, pageSize);

            if (validationResult.Length != 0)
                return BadRequest(validationResult);

            var result = _productService.GetProducts(new Contracts.Models.ProductsListRequest { PageNumber = pageNumber-1, Take = pageSize });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result.Errors);
          
        }

        /// <summary>
        /// Валидирует входные параметры.
        /// </summary>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        private string[] ValidateInput(int pageNumber, int pageSize)
        {
            var result = new List<string>();
            if (pageNumber < 0)
                result.Add("Номер запрашивамоей страницы не может быть меньше нуля");

            if (pageSize <= 0 || pageSize > 100)
                result.Add("Задано неправильно значение параметра pageSize. Оно не может быть меньше либо равно 0 и больше 100 ");

            return result.ToArray();
        }
    }
}
