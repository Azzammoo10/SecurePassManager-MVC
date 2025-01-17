using FluentValidation;
using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using WatchlistV2.Data;
    using WatchlistV2.Interface;
    using WatchlistV2.Models;
    using WatchlistV2.Repository;
    using WatchlistV2.Services;
    using WatchlistV2.Validator;

    var builder = WebApplication.CreateBuilder(args);

    // Ajouter la cha�ne de connexion et configurer le DbContext pour SQL Server
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    // Ajouter Identity avec la configuration par d�faut mais avec la classe Utilisateur
    builder.Services.AddIdentity<Utilisateur, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    }).AddEntityFrameworkStores<ApplicationDbContext>();

    // Configuration de l'authentification bas�e sur les cookies
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Authentification/Login"; // Redirige vers la page de connexion
            options.AccessDeniedPath = "/Authentification/AccessDenied"; // Redirige en cas d'acc�s refus�
            options.ExpireTimeSpan = TimeSpan.FromMinutes(2); // Dur�e de validit� du cookie
            options.SlidingExpiration = true; // Renouvelle le cookie si l'utilisateur est actif
        });

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Add FluentValidation services
        builder.Services.AddFluentValidationAutoValidation(); // Enable automatic validation
        builder.Services.AddFluentValidationClientsideAdapters(); // Enable client-side validation

        // Register validators from the assembly containing CompteDtoValidator
        builder.Services.AddValidatorsFromAssemblyContaining<CompteDtoValidator>();

    // Enregistrement des services
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<ILogoutBackgroundService, LogoutBackgroundService>();
    builder.Services.AddHttpContextAccessor(); // Nécessaire pour accéder au HttpContext dans le service
    builder.Services.AddSingleton<IEncryptionService>(new AesEncryptionService(
        key: "Add_Your_Key_Here", // Nouvelle clé Base64 
        iv: "Add_Your_IV_Here" // Nouvel IV Base64
    ));


    builder.Services.AddScoped<IPwnedService, PwnedService>();
    builder.Services.AddHttpClient(); // Enregistrer HttpClient

    var app = builder.Build();

    // Configurer le pipeline HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage(); // Affiche des d�tails d'erreur en d�veloppement
    }
    else
    {
        app.UseExceptionHandler("/Compte/Error"); // Redirige vers une vue Razor pour les erreurs en production
        app.UseHsts(); // Active HSTS (HTTP Strict Transport Security)
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication(); // Active l'authentification
    app.UseAuthorization(); // Active l'autorisation

    app.MapControllerRoute(
        name: "Compte",
        pattern: "Compte/{action=Index}/{id?}",
        defaults: new { controller = "Compte" });

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Authentification}/{action=Login}/{id?}");

    app.Run();
