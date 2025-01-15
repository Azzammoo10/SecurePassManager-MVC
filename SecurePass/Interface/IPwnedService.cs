using WatchlistV2.Models;

namespace WatchlistV2.Interface
{
    public interface IPwnedService
    {
        Task<LeakInfo> CheckIfPwnedAsync(string username);
    }
}
