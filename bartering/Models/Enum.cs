namespace bartering.Models
{
    public class Enum
    {
        public enum ItemStatus
        {
            Available,
            PendingSwap,
            Swapped
        }
        public enum ItemCondition
        {
            New,
            LikeNew,
            Good,
            Fair,
            Poor
        }
        public enum SwapOfferStatus
        {
            Pending,
            Accepted,
            Declined,
            Completed,
            Cancelled
        }
    }
}
