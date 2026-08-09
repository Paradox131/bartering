using System.ComponentModel.DataAnnotations;

namespace bartering.Models
{
    public class SwapProposalViewModel
    {
        public int RequestedItemId { get; set; }
        public string RequestedItemTitle { get; set; } = string.Empty;
        public string RequestedItemOwner { get; set; } = string.Empty;
        [Required(ErrorMessage = "Select an item to offer.")]
        [Display(Name = "Your item to offer")]
        public int OfferedItemId { get; set; }
        [MaxLength(500)]
        [Display(Name = "Message (optional)")]
        public string? Message { get; set; }
        public IReadOnlyList<ItemOption> MyAvailableItems { get; set; } = Array.Empty<ItemOption>();
    }
    public class ItemOption
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
