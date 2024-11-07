using AwsWorkshop.Product.Api.Core.Application.Dtos;

namespace AwsWorkshop.Product.Api.Core.Application.Abstracts
{
    public interface IProductService
    {
        Task<Response<string>> AddRange(List<ProductDto> productDtos);
        Task<Response<IEnumerable<ProductDto>>> GetAll(int start, int limit);
    }
}
