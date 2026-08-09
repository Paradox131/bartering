using System.ComponentModel.DataAnnotations;
using static bartering.Models.Enum;

namespace bartering.Models.ViewModels
{
    public class ItemFormViewModel
    {
        public int? Id { get; set; }
        [Required, MaxLength(120)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;
        [Required, MaxLength(2000)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        [Required, MaxLength(60)]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Condition")]
        public ItemCondition Condition { get; set; }
        [Display(Name = "Photo")]
        public IFormFile? Image { get; set; }
        public string? ExistingImageUrl { get; set; }
    }
}
