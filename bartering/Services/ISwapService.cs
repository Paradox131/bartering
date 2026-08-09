using bartering.Models;
using bartering.Models.ViewModels;

namespace bartering.Services
{
    public interface ISwapService
    {
        Task<SwapOffer?> ProposeAsync(string fromUserId, SwapProposalViewModel model);
        Task<IReadOnlyList<SwapOffer>> GetIncomingAsync(string userId);
        Task<IReadOnlyList<SwapOffer>> GetOutgoingAsync(string userId);
        Task<SwapOffer?> GetByIdAsync(int id);
        Task<bool> AcceptAsync(int offerId, string userId);
        Task<bool> DeclineAsync(int offerId, string userId);
        Task<bool> CancelAsync(int offerId, string userId);
        Task<bool> CompleteAsync(int offerId, string userId);
    }
}
