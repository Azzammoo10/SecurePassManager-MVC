namespace WatchlistV2.Exceptions
{
    public class AccountNotFoundException : Exception
    {

        // Constructeur sans message personnalisé
        public AccountNotFoundException() : base("Le compte n'a pas été trouvé.") { }

        // Constructeur avec message personnalisé
        public AccountNotFoundException(string message) : base(message) { }

        // Constructeur avec message personnalisé et exception interne
        public AccountNotFoundException(string message, Exception innerException) : base(message, innerException) { }

    }
    
}
