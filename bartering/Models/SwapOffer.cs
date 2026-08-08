using System.ComponentModel.DataAnnotations;
using static bartering.Models.Enum;

namespace bartering.Models
{
    public class SwapOffer
    {
        public int Id { get; set; }
        public string FromUserId { get; set; } = string.Empty;
        public User FromUser { get; set; } = null!;
        public string ToUserId { get; set; } = string.Empty;
        public User ToUser { get; set; } = null!;
        public int OfferedItemId { get; set; }
        public Item OfferedItem { get; set; } = null!;
        public int RequestedItemId { get; set; }
        public Item RequestedItem { get; set; } = null!;
        public SwapOfferStatus Status { get; set; } = SwapOfferStatus.Pending;
        [MaxLength(500)]
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
