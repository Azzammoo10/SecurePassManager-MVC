using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WatchlistV2.DTOs;
using WatchlistV2.Models;
using WatchlistV2.Validator;
using WatchlistV2.Services;
using WatchlistV2.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;

namespace WatchlistV2.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly UserManager<Utilisateur> _userManager;

        private readonly IPwnedService _pwnedService;
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly ILogger<AuthentificationController> _logger;
        private readonly ILogoutBackgroundService _logoutBackgroundService;

        public AuthentificationController(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager,
            IPwnedService pwnedService,
            ILogger<AuthentificationController> logger,
            ILogoutBackgroundService logoutBackgroundService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _pwnedService = pwnedService;
            _logger = logger;
            _logoutBackgroundService = logoutBackgroundService;
        }

        // Afficher le formulaire de connexion
        [HttpGet]
        [AllowAnonymous] // Autoriser l'accès sans authentification
        public IActionResult Login()
        {
            // Déconnecter l'utilisateur s'il est déjà connecté
            return View();
        }

        // Traiter le formulaire de connexion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                // Valider les données d'entrée
                var validator = new LoginDtoValidator();
                var validationResult = validator.Validate(loginDto);

                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return View(loginDto);
                }

                // Rechercher l'utilisateur dans la base de données
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

                // Si l'utilisateur n'existe pas, retourner une erreur
                if (user == null)
                {
                    _logger.LogWarning($"Tentative de connexion avec un nom d'utilisateur inconnu : {loginDto.Username}");
                    ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
                    return View(loginDto);
                }

                // Vérifier si le mot de passe est correct
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                // Si l'authentification échoue, retourner une erreur
                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Tentative de connexion échouée pour l'utilisateur : {loginDto.Username}");
                    ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
                    return View(loginDto);
                }

                // Connexion réussie, rediriger vers la page d'accueil
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Compte");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erreur lors de la tentative de connexion.");
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de la connexion.");
                return View(loginDto);
            }
        }

        // Afficher le formulaire d'inscription
        [HttpGet]
        [AllowAnonymous] // Autoriser l'accès sans authentification
        public IActionResult Register()
        {
            return View();
        }

        // Traiter le formulaire d'inscription
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTo registerDTo)
        {
            try
            {
                // Validation des données du DTO
                var validator = new RegisterDToValidators();
                var validationResult = validator.Validate(registerDTo);

                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return View(registerDTo);
                }

                // Vérifier si le nom d'utilisateur ou l'email est exposé dans une fuite de données (en parallèle)
                var usernameLeakTask = _pwnedService.CheckIfPwnedAsync(registerDTo.Username);
                var emailLeakTask = _pwnedService.CheckIfPwnedAsync(registerDTo.Email);
                await Task.WhenAll(usernameLeakTask, emailLeakTask);

                var usernameLeakInfo = usernameLeakTask.Result;
                var emailLeakInfo = emailLeakTask.Result;

                if (usernameLeakInfo?.IsPwned == true)
                {
                    ModelState.AddModelError(string.Empty, "Le nom d'utilisateur est compromis. Veuillez en choisir un autre.");
                    return View(registerDTo);
                }

                if (emailLeakInfo?.IsPwned == true)
                {
                    ModelState.AddModelError(string.Empty, "L'adresse email est compromise. Veuillez en utiliser une autre.");
                    return View(registerDTo);
                }

                // Création de l'utilisateur
                var appUser = new Utilisateur
                {
                    UserName = registerDTo.Username,
                    Email = registerDTo.Email,
                    DateJoined = DateTime.UtcNow // Initialisation de DateJoined
                };

                var createUser = await _userManager.CreateAsync(appUser, registerDTo.Password);
                if (!createUser.Succeeded)
                {
                    _logger.LogError($"Échec de la création de l'utilisateur : {string.Join(", ", createUser.Errors)}");
                    foreach (var error in createUser.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(registerDTo);
                }

                // Attribution d'un rôle à l'utilisateur
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (!roleResult.Succeeded)
                {
                    _logger.LogError($"Échec de l'attribution du rôle : {string.Join(", ", roleResult.Errors)}");
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(registerDTo);
                }

                // Redirection vers la page Login
                return RedirectToAction("Login", "Authentification");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erreur lors de l'inscription.");
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'inscription.");
                return View(registerDTo);
            }
        }

        // Déconnexion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Authentification");
        }


         // Exemple d'utilisation du service dans une action
            [HttpPost]
            public async Task<IActionResult> ForceLogout()
            {
                await _logoutBackgroundService.SignOutAllUsersAsync();
                return RedirectToAction("Login", "Authentification");
            }
    }
}