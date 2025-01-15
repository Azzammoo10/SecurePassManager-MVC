using WatchlistV2.DTOs;
using WatchlistV2.Helpers;
using WatchlistV2.Models;

namespace WatchlistV2.Interface
{
    public interface IAccountRepository
    {
        Task<List<Compte>> GetAll(string userId);
        Task<Compte?> GetById(int id, string userId);
        Task<Compte> Add(CompteDTO accountDto, string userId);
        Task<Compte?> Update(int id, CompteUpdateDTO Ac, string userId);
        Task<Compte?> Delete(int id, string userId);

        Task<int> GetNumberOfAccountsByUserId(string userId);
        Task<List<Compte>> GetAccountByServiceAndName(QueryObject query, string userId);
    }
}
