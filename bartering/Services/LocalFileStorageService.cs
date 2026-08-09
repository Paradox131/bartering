namespace bartering.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };
        private const long MaxBytes = 5 * 1024 * 1024;
        private readonly IWebHostEnvironment _environment;
        public LocalFileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string?> SaveImageAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file.Length == 0 || file.Length > MaxBytes)
                return null;
            var extension = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(extension))
                return null;
            var uploadsDir = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsDir);
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(uploadsDir, fileName);
            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);
            return $"/uploads/{fileName}";
        }
        public void DeleteImage(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath) || !relativePath.StartsWith("/uploads/", StringComparison.Ordinal))
                return;
            var fullPath = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }

}
