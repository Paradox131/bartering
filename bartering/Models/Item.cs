using System.ComponentModel.DataAnnotations;
using static bartering.Models.Enum;

namespace bartering.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required, MaxLength(120)]
        public string Title { get; set; } = string.Empty;
        [Required, MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        [Required, MaxLength(60)]
        public string Category { get; set; } = string.Empty;
        public ItemCondition Condition { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.Available;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string OwnerId { get; set; } = string.Empty;
        public User Owner { get; set; } = null!;
    }
}
