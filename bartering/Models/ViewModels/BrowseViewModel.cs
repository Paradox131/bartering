namespace bartering.Models.ViewModels
{
    public class BrowseViewModel
    {
        public IReadOnlyList<Item> Items { get; set; } = Array.Empty<Item>();
        public string? Search { get; set; }
        public string? Category { get; set; }
        public IReadOnlyList<string> Categories { get; set; } = Array.Empty<string>();
    }

}
