using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AwsWorkshop.Product.Api.Core.Application.Abstracts;
using AwsWorkshop.Product.Api.Core.Application.Dtos;
using AwsWorkshop.Product.Api.Core.Application.Settings;

namespace AwsWorkshop.Product.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBased
    {
        private readonly IProductService _productService;
        private ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(List<ProductDto> products)
        {
            try
            {
                return ActionResultInstance(await _productService.AddRange(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ActionResultInstance(Core.Application.Dtos.Response<string>.Fail("An error has occurred.", 500));
            }
        }

        [HttpGet("{start}/{limit}")]
        public async Task<IActionResult> Get(int start, int limit)
        {
            try
            {
                var products = await _productService.GetAll(start, limit);
                return ActionResultInstance(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ActionResultInstance(Core.Application.Dtos.Response<string>.Fail("An error has occurred.", 500));
            }
        }
    }
}
