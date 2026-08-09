using bartering.Data;
using bartering.Models;
using bartering.Models.ViewModels;
using static bartering.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace bartering.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _db;
        private readonly IFileStorageService _files;
        public ItemService(ApplicationDbContext db, IFileStorageService files)
        {
            _db = db;
            _files = files;
        }
        public async Task<BrowseViewModel> BrowseAsync(string? search, string? category)
        {
            var query = _db.Items
                .Include(i => i.Owner)
                .Where(i => i.Status == ItemStatus.Available)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(i =>
                    i.Title.Contains(term) ||
                    i.Description.Contains(term) ||
                    i.Category.Contains(term));
            }
            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(i => i.Category == category);
            var items = await query
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
            var categories = await _db.Items
                .Where(i => i.Status == ItemStatus.Available)
                .Select(i => i.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
            return new BrowseViewModel
            {
                Items = items,
                Search = search,
                Category = category,
                Categories = categories
            };
        }
        public Task<Item?> GetByIdAsync(int id) =>
            _db.Items
                .Include(i => i.Owner)
                .FirstOrDefaultAsync(i => i.Id == id);
        public async Task<IReadOnlyList<Item>> GetUserItemsAsync(string userId) =>
            await _db.Items
                .Where(i => i.OwnerId == userId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        public async Task<IReadOnlyList<Item>> GetAvailableItemsForUserAsync(string userId) =>
            await _db.Items
                .Where(i => i.OwnerId == userId && i.Status == ItemStatus.Available)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        public async Task<Item> CreateAsync(string userId, ItemFormViewModel model)
        {
            string? imageUrl = null;
            if (model.Image is { Length: > 0 })
                imageUrl = await _files.SaveImageAsync(model.Image);
            var item = new Item
            {
                Title = model.Title.Trim(),
                Description = model.Description.Trim(),
                Category = model.Category.Trim(),
                Condition = model.Condition,
                ImageUrl = imageUrl,
                OwnerId = userId,
                Status = ItemStatus.Available
            };
            _db.Items.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<Item?> UpdateAsync(int id, string userId, ItemFormViewModel model)
        {
            var item = await _db.Items.FirstOrDefaultAsync(i => i.Id == id && i.OwnerId == userId);
            if (item is null || item.Status != ItemStatus.Available)
                return null;
            item.Title = model.Title.Trim();
            item.Description = model.Description.Trim();
            item.Category = model.Category.Trim();
            item.Condition = model.Condition;
            if (model.Image is { Length: > 0 })
            {
                _files.DeleteImage(item.ImageUrl);
                item.ImageUrl = await _files.SaveImageAsync(model.Image);
            }
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var item = await _db.Items.FirstOrDefaultAsync(i => i.Id == id && i.OwnerId == userId);
            if (item is null || item.Status != ItemStatus.Available)
                return false;
            var hasPendingOffers = await _db.SwapOffers.AnyAsync(s =>
                s.Status == SwapOfferStatus.Pending &&
                (s.OfferedItemId == id || s.RequestedItemId == id));
            if (hasPendingOffers)
                return false;
            _files.DeleteImage(item.ImageUrl);
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
