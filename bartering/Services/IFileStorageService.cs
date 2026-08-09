namespace bartering.Services
{
    public interface IFileStorageService
    {
        Task<string?> SaveImageAsync(IFormFile file, CancellationToken cancellationToken = default);
        void DeleteImage(string? relativePath);
    }
}
