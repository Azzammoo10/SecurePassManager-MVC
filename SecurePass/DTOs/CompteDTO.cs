namespace WatchlistV2.DTOs
{
    public class CompteDTO
    {
        public string Service { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string LogoUrl { get; set; }= string.Empty; // URL du logo

        // Ajout d'AppUserId pour lier un compte à un utilisateur
        public string? UtilisateurId { get; set; } // Rendre AppUserId optionnel
    }

}
