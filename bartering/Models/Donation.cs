using System.ComponentModel.DataAnnotations;

namespace bartering.Models
{

    public class Donation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a donation amount.")]
        [Range(1, 100000, ErrorMessage = "Donation must be between €1 and €100,000.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(100)]
        [Display(Name = "Your Name")]
        public string DonorName { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Message")]
        public string? Message { get; set; }

        [Display(Name = "Anonymous Donation")]
        public bool IsAnonymous { get; set; }

        public DateTime DonationDate { get; set; } = DateTime.UtcNow;
    }
}
