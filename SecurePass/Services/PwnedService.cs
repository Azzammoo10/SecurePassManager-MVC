using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WatchlistV2.Interface;
using WatchlistV2.Models;

namespace WatchlistV2.Services
{
    public class PwnedService : IPwnedService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public PwnedService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<LeakInfo> CheckIfPwnedAsync(string account)
        {
            try
            {
                // Appel à l'API HIBP pour vérifier si le nom d'utilisateur ou l'email est dans une fuite
                var result = await _httpClient.GetAsync($"https://haveibeenpwned.com/api/v2/breachedaccount/{account}");

                if (result.IsSuccessStatusCode)
                {
                    // Si l'appel réussit, désérialise la réponse JSON
                    var content = await result.Content.ReadAsStringAsync();
                    var breaches = JsonSerializer.Deserialize<BreachedAccount[]>(content);

                    // Si des fuites sont trouvées, retourne les informations de fuite
                    return new LeakInfo
                    {
                        IsPwned = breaches != null && breaches.Length > 0,  // Vérifie si des fuites existent
                        Breaches = breaches ?? Array.Empty<BreachedAccount>()  // Si null, utilise un tableau vide
                    };
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Si la requête retourne 404, cela signifie qu'il n'y a pas de fuite
                    return new LeakInfo
                    {
                        IsPwned = false,
                        Breaches = Array.Empty<BreachedAccount>()  // Aucune fuite trouvée
                    };
                }
                else
                {
                    // Si une autre erreur survient, crée une instance sans fuites et log l'erreur
                    return new LeakInfo
                    {
                        IsPwned = false,
                        Breaches = Array.Empty<BreachedAccount>()
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
