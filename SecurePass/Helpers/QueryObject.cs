namespace WatchlistV2.Helpers
{
    public class QueryObject
    {
        public string? Service { get; set; } = null;
        public string? Username { get; set; } = null;

        public int PageNumber { get; set; } = 1; // Valeur par défaut
        public int PageSize { get; set; } = 10; // Valeur par défaut
    }
}
