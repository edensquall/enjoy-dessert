using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "產品名稱")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? ProductName { get; set; }
        [Display(Name = "價格")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(0.1, double.MaxValue, ErrorMessage = "{0} 必須大於0")]
        public decimal Price { get; set; }
        [Display(Name = "數量")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, double.MaxValue, ErrorMessage = "{0} 至少為1")]
        public int Quantity { get; set; }
        [Display(Name = "圖片網址")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? ImageUrl { get; set; }
        [Display(Name = "產品類型")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? Type { get; set; }
    }
}