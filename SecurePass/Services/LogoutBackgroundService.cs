using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WatchlistV2.Models;
using WatchlistV2.Interface;

public class LogoutBackgroundService : BackgroundService, ILogoutBackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public LogoutBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Surveiller l'état du serveur
            await Task.Delay(1000, stoppingToken); // Vérifier toutes les secondes
        }

        // Déconnecter tous les utilisateurs lorsque le serveur s'arrête
        await SignOutAllUsersAsync();
    }

    public async Task SignOutAllUsersAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<Utilisateur>>();
            await signInManager.SignOutAsync();
        }
    }
}