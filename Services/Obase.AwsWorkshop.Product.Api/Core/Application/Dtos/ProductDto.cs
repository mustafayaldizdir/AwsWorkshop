
namespace AwsWorkshop.Product.Api.Core.Application.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string LanguageCode { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string CategoryPath { get; set; }
        public decimal? Price { get; set; }
        public string Brand { get; set; }
        public int OrderCount { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
