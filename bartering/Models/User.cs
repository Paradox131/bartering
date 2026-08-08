using Microsoft.AspNetCore.Identity;

namespace bartering.Models
{
    public class User : IdentityUser
    {
      // public string Name { get; set; } 
      //  public string Email { get; set; }
       // public string Password { get; set; }
       // public DateOnly DateOfBirth { get; set; }

        public string DisplayName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<SwapOffer> SentOffers { get; set; } = new List<SwapOffer>();
        public ICollection<SwapOffer> ReceivedOffers { get; set; } = new List<SwapOffer>();

    }
}
