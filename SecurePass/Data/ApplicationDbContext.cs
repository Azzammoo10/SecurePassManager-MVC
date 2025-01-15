using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WatchlistV2.Models;
using WatchlistV2.DTOs;
using System.Numerics;
using Microsoft.AspNetCore.Identity;

namespace WatchlistV2.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilisateur>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // DbSets pour chaque entité
        public DbSet<Compte> Comptes { get; set; }


        // Configuration des relations entre les entités
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            var roles = new List<IdentityRole>
            {
                new IdentityRole { 
                    Name = "Admin", 
                    NormalizedName = "ADMIN" 
                },
                new IdentityRole
                { 
                    Name = "User", 
                    NormalizedName = "USER" 
                },
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);


            // Relation un-à-plusieurs entre Utilisateur et Compte
            modelBuilder.Entity<Compte>()
                .HasOne(a => a.Utilisateur)           // Un Account a un AppUser
                .WithMany(u => u.Comptes)            // Un AppUser a plusieurs Accounts
                .HasForeignKey(a => a.UtilisateurId)  // Clé étrangère dans Account
                .OnDelete(DeleteBehavior.Cascade); // Supprime les comptes si l'utilisateur est supprimé


        }

    }
}
