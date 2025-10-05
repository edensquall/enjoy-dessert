namespace Core.Contracts
{
    public class ProductSummaryContract
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ProductTypeId { get; set; }
        public decimal Price { get; set; }
    }
}