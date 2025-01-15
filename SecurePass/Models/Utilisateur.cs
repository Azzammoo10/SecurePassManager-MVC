using System.Numerics;
using Microsoft.AspNetCore.Identity;

namespace WatchlistV2.Models
{
    public class Utilisateur : IdentityUser
    {
        public List<Compte> Comptes { get; set; } = new List<Compte>();
        public DateTime DateJoined { get; set; } 
    }
}
