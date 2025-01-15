namespace WatchlistV2.Models
{
    public class LeakInfo
    {
        public bool IsPwned { get; set; }
        public required BreachedAccount[] Breaches { get; set; }  // Propriété requise
    }
}
