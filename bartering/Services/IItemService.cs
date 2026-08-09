using bartering.Models;
using bartering.Models.ViewModels;

namespace bartering.Services
{
    public interface IItemService
    {
        Task<BrowseViewModel> BrowseAsync(string? search, string? category);
        Task<Item?> GetByIdAsync(int id);
        Task<IReadOnlyList<Item>> GetUserItemsAsync(string userId);
        Task<IReadOnlyList<Item>> GetAvailableItemsForUserAsync(string userId);
        Task<Item> CreateAsync(string userId, ItemFormViewModel model);
        Task<Item?> UpdateAsync(int id, string userId, ItemFormViewModel model);
        Task<bool> DeleteAsync(int id, string userId);
    }
}
