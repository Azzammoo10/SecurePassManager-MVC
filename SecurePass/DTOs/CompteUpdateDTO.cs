namespace WatchlistV2.DTOs
{
    public class CompteUpdateDTO
    {

        public string Service { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
    }
}
