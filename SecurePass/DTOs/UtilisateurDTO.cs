namespace WatchlistV2.DTOs
{
    public class UtilisateurDTO
    {
        public string Id { get; set; } // Identifiant unique de l'utilisateur
        public string Prenom { get; set; } // Prénom de l'utilisateur
        public string UserName { get; set; } // Nom d'utilisateur (login)
        public ICollection<CompteDTO> Comptes { get; set; } // Liste des comptes associés à l'utilisateur
    }

}
