using System.ComponentModel.DataAnnotations;

namespace bartering.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required, MaxLength(80)]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Location { get; set; }
        [Required, DataType(DataType.Password), MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
