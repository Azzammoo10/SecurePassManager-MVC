using WatchlistV2.Models;

namespace WatchlistV2.Interface
{
    public interface ITokenService
    {
        string CreateToken(Utilisateur user);
    }
}
