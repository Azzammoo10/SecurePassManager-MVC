namespace WatchlistV2.Models
{
    public class Compte
    {
       
        public int Id { get; set; } // Identifiant unique du compte
        public string Service { get; set; } = string.Empty; // Nom du service ou de la plateforme (ex. : Google, Facebook)
        public string Username { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty; // Mot de passe chiffré
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;  // Valeur par défaut
        public string LogoUrl { get; set; }= string.Empty; // URL du logo

        // Clé étrangère pour lier le compte à un utilisateur spécifique
        public string? UtilisateurId { get; set; }
        public  Utilisateur? Utilisateur { get; set; } // Relation avec l'utilisateur

    }

}
