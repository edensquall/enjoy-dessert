using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class AddressDto
    {
        [Display(Name = "姓")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? FirstName { get; set; }
        [Display(Name = "名")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? LastName { get; set; }
        [Display(Name = "縣")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? County { get; set; }
        [Display(Name = "市/鄉鎮")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? City { get; set; }
        [Display(Name = "街")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}