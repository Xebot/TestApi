using AutoMapper;
using System;
using System.Linq;
using TestApi.Contracts.Models;
using TestApi.Infrastructure;

namespace TestApi.Core.Services.ProductService
{
    /// <summary>
    /// Сервис по работе с товарами.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(
            AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public PagedResult<ProductDto> GetProducts(ProductsListRequest request)
        {            
            var totalCount = _context.Products.Count();

            if (totalCount == 0)
                return new PagedResult<ProductDto>(
                    totalCount: 0, 
                    currentPage: 0, 
                    pageSize: request.Take, 
                    items: Array.Empty<ProductDto>(), 
                    isSuccess: true);

            var items = _context.Products.Skip(request.PageNumber * request.Take).Take(request.Take).ToList();

            return new PagedResult<ProductDto>(
                totalCount: totalCount,
                currentPage: request.PageNumber,
                pageSize: request.Take,
                items: items.Select(x => _mapper.Map<ProductDto>(x)).ToArray(),
                isSuccess: true);
        }
    }
}
