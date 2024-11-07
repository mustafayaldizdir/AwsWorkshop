using AwsWorkshop.Product.Api.Core.Application.Abstracts;
using AwsWorkshop.Product.Api.Core.Application.Dtos;
using AwsWorkshop.Product.Api.Core.Domain.Entities;

namespace AwsWorkshop.Product.Api.Infrastructure.Persistence.Concretes.Services
{
    public class ProductService : IProductService
    {
        private readonly IServiceGeneric<AppProduct, ProductDto> _productService;
        private ILogger<ProductService> _logger;

        public ProductService(IServiceGeneric<AppProduct, ProductDto> productService, ILogger<ProductService> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<Response<string>> AddRange(List<ProductDto> productDtos)
        {
            var response = await _productService.AddRangeAsync(productDtos);

            return response.IsSuccessful ? Response<string>.Success("Success",200) : Response<string>.Success("Products were not added", 200);
        }

        public async Task<Response<IEnumerable<ProductDto>>> GetAll(int start,int limit)
        {
            var products = await _productService.GetAllAsync();
            return Response<IEnumerable<ProductDto>>.Success(products.Data.Skip(start).Take(limit),200);
        }
    }
}
