using bartering.Data;
using bartering.Models;
using bartering.Models.ViewModels;
using static bartering.Models.Enum;

namespace bartering.Services
{
    public class SwapService : ISwapService
    {
        private readonly ApplicationDbContext _db;
        public SwapService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<SwapOffer?> ProposeAsync(string fromUserId, SwapProposalViewModel model)
        {
            var requested = await _db.Items.FirstOrDefaultAsync(i => i.Id == model.RequestedItemId);
            var offered = await _db.Items.FirstOrDefaultAsync(i => i.Id == model.OfferedItemId);
            if (requested is null || offered is null)
                return null;
            if (requested.OwnerId == fromUserId || offered.OwnerId != fromUserId)
                return null;
            if (requested.Status != ItemStatus.Available || offered.Status != ItemStatus.Available)
                return null;
            var duplicate = await _db.SwapOffers.AnyAsync(s =>
                s.FromUserId == fromUserId &&
                s.OfferedItemId == model.OfferedItemId &&
                s.RequestedItemId == model.RequestedItemId &&
                s.Status == SwapOfferStatus.Pending);
            if (duplicate)
                return null;
            var offer = new SwapOffer
            {
                FromUserId = fromUserId,
                ToUserId = requested.OwnerId,
                OfferedItemId = model.OfferedItemId,
                RequestedItemId = model.RequestedItemId,
                Message = model.Message?.Trim(),
                Status = SwapOfferStatus.Pending
            };
            _db.SwapOffers.Add(offer);
            await _db.SaveChangesAsync();
            return await GetByIdAsync(offer.Id);
        }
        public async Task<IReadOnlyList<SwapOffer>> GetIncomingAsync(string userId) =>
            await QueryWithDetails()
                .Where(s => s.ToUserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        public async Task<IReadOnlyList<SwapOffer>> GetOutgoingAsync(string userId) =>
            await QueryWithDetails()
                .Where(s => s.FromUserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        public Task<SwapOffer?> GetByIdAsync(int id) =>
            QueryWithDetails().FirstOrDefaultAsync(s => s.Id == id);
        public async Task<bool> AcceptAsync(int offerId, string userId)
        {
            var offer = await _db.SwapOffers
                .Include(s => s.OfferedItem)
                .Include(s => s.RequestedItem)
                .FirstOrDefaultAsync(s => s.Id == offerId);
            if (offer is null || offer.ToUserId != userId || offer.Status != SwapOfferStatus.Pending)
                return false;
            if (offer.OfferedItem.Status != ItemStatus.Available ||
                offer.RequestedItem.Status != ItemStatus.Available)
                return false;
            offer.Status = SwapOfferStatus.Accepted;
            offer.RespondedAt = DateTime.UtcNow;
            offer.OfferedItem.Status = ItemStatus.PendingSwap;
            offer.RequestedItem.Status = ItemStatus.PendingSwap;
            var conflicting = await _db.SwapOffers
                .Include(s => s.OfferedItem)
                .Include(s => s.RequestedItem)
                .Where(s => s.Id != offerId &&
                            s.Status == SwapOfferStatus.Pending &&
                            (s.OfferedItemId == offer.OfferedItemId ||
                             s.RequestedItemId == offer.OfferedItemId ||
                             s.OfferedItemId == offer.RequestedItemId ||
                             s.RequestedItemId == offer.RequestedItemId))
                .ToListAsync();
            foreach (var other in conflicting)
                other.Status = SwapOfferStatus.Declined;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeclineAsync(int offerId, string userId)
        {
            var offer = await _db.SwapOffers.FirstOrDefaultAsync(s => s.Id == offerId);
            if (offer is null || offer.ToUserId != userId || offer.Status != SwapOfferStatus.Pending)
                return false;
            offer.Status = SwapOfferStatus.Declined;
            offer.RespondedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CancelAsync(int offerId, string userId)
        {
            var offer = await _db.SwapOffers.FirstOrDefaultAsync(s => s.Id == offerId);
            if (offer is null || offer.FromUserId != userId || offer.Status != SwapOfferStatus.Pending)
                return false;
            offer.Status = SwapOfferStatus.Cancelled;
            offer.RespondedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CompleteAsync(int offerId, string userId)
        {
            var offer = await _db.SwapOffers
                .Include(s => s.OfferedItem)
                .Include(s => s.RequestedItem)
                .FirstOrDefaultAsync(s => s.Id == offerId);
            if (offer is null || offer.Status != SwapOfferStatus.Accepted)
                return false;
            if (offer.FromUserId != userId && offer.ToUserId != userId)
                return false;
            offer.Status = SwapOfferStatus.Completed;
            offer.CompletedAt = DateTime.UtcNow;
            offer.OfferedItem.Status = ItemStatus.Swapped;
            offer.RequestedItem.Status = ItemStatus.Swapped;
            await _db.SaveChangesAsync();
            return true;
        }
        private IQueryable<SwapOffer> QueryWithDetails() =>
            _db.SwapOffers
                .Include(s => s.FromUser)
                .Include(s => s.ToUser)
                .Include(s => s.OfferedItem)
                .Include(s => s.RequestedItem);
    }

}
