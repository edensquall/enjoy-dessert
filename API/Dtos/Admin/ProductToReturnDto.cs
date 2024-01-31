namespace API.Dtos.Admin
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsBestseller { get; set; } = false;
        public bool IsShow { get; set; } = true;
        public bool IsShowByDate { get; set; } = false;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ProductTypeId { get; set; }
        public int[]? ImageUrlsIndex { get; set; }
        public IReadOnlyList<string>? ImageUrls { get; set; }
    }
}