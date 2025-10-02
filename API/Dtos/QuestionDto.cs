using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class QuestionDto
    {
        [Display(Name = "問題")]
        [Required(ErrorMessage = "{0} 為必填")]
        public required string Question { get; set; }
    }
}