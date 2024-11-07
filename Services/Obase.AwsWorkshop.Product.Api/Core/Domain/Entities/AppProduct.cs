namespace AwsWorkshop.Product.Api.Core.Domain.Entities
{
    public class AppProduct
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string LanguageCode { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string CategoryPath { get; set; }
        public string? Price { get; set; }
        public string Brand { get; set; }
        public int OrderCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
