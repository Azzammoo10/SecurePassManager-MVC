using Microsoft.EntityFrameworkCore;
using WatchlistV2.Data;
using WatchlistV2.DTOs;
using WatchlistV2.Exceptions;
using WatchlistV2.Helpers;
using WatchlistV2.Interface;
using WatchlistV2.Models;

namespace WatchlistV2.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        

        public AccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Compte>> GetAll(string userId)
        {
            var comptes = await _dbContext.Comptes
                .Where(a => a.UtilisateurId == userId)
                .ToListAsync();

            return comptes; // Retourne une liste vide si aucun compte n'est trouvé
        }

        public async Task<Compte?> GetById(int id, string userId)
        {
            var Compte = await _dbContext.Comptes.FirstOrDefaultAsync(c => c.Id == id && c.UtilisateurId == userId);

            if (Compte == null) {
                throw new AccountNotFoundException($"Le compte avec l'ID {id} est introuvable ou ne vous appartient pas.");
            }
            return Compte;  
        }


       public async Task<Compte> Add(CompteDTO compteDto, string userId)
        {
            var compte = new Compte
            {
                Service = compteDto.Service,
                Username = compteDto.Username,
                Password = compteDto.Password,
                UtilisateurId = userId,
                LogoUrl = compteDto.LogoUrl,
                DateAdded = DateTime.UtcNow
            };

            _dbContext.Comptes.Add(compte);
            await _dbContext.SaveChangesAsync();

            return compte;
        }

        public async Task<Compte?> Delete(int id, string userId)
        {
           var accountToRemove = await _dbContext.Comptes.FirstOrDefaultAsync(a => a.Id == id && a.UtilisateurId == userId);

            if (accountToRemove == null)
            {
                // Si le compte n'existe pas ou n'appartient pas à l'utilisateur, on lève une exception
                throw new AccountNotFoundException($"Le compte avec l'ID {id} est introuvable ou ne vous appartient pas.");
            }

            _dbContext.Comptes.Remove(accountToRemove);
            await _dbContext.SaveChangesAsync();

            return accountToRemove;
        }

       

       

        public async Task<Compte?> Update(int id, CompteUpdateDTO Ac, string userId)
        {
            var accountExists = await _dbContext.Comptes
                 .FirstOrDefaultAsync(a => a.Id == id && a.UtilisateurId == userId);

            if (accountExists == null)
            {
                throw new AccountNotFoundException($"Le compte avec l'ID {id} est introuvable ou ne vous appartient pas.");
            }

            accountExists.Service = Ac.Service;
            accountExists.Username = Ac.Username;
            accountExists.Password = Ac.Password;
            accountExists.DateAdded = DateTime.UtcNow;


            await _dbContext.SaveChangesAsync();
            return accountExists;
        }

        public async Task<List<Compte>> GetAccountByServiceAndName(QueryObject query, string userId)
        {
            var accountsQuery = _dbContext.Comptes
               .Where(a => a.UtilisateurId == userId) // Filtre par utilisateur
               .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Username))
            {
                accountsQuery = accountsQuery.Where(a => a.Username.Contains(query.Username));
            }

            if (!string.IsNullOrWhiteSpace(query.Service))
            {
                accountsQuery = accountsQuery.Where(a => a.Service.Contains(query.Service));
            }

            // Pagination
            var paginatedAccounts = await accountsQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            if (!paginatedAccounts.Any())
            {
                throw new AccountNotFoundException("Aucun compte trouvé avec les critères spécifiés.");
            }

            return paginatedAccounts;
        }

        public async Task<int> GetNumberOfAccountsByUserId(string userId)
        {
            return await _dbContext.Comptes
                .Where(c => c.UtilisateurId == userId)
                .CountAsync();
        }
        
    }
}
