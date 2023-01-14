namespace API.Dtos
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Caption { get; set; }
        public string? Content { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? StartDate { get; set; }
    }
}