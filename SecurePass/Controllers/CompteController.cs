using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WatchlistV2.Exceptions;
using WatchlistV2.Helpers;
using WatchlistV2.Interface;
using Microsoft.AspNetCore.Authorization;
using WatchlistV2.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using WatchlistV2.Models;

namespace WatchlistV2.Controllers
{
    [Authorize] // Sécuriser l'accès aux utilisateurs authentifiés
    public class CompteController : Controller
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ILogger<CompteController> _logger;
        private readonly IEncryptionService _encryptionService;
        private readonly UserManager<Utilisateur> _userManager;

        // Constructeur pour l'injection de dépendances
        public CompteController(IAccountRepository accountRepo, ILogger<CompteController> logger, IEncryptionService encryptionService, UserManager<Utilisateur> userManager)
        {
            _accountRepo = accountRepo;
            _logger = logger;
            _encryptionService = encryptionService;
            _userManager = userManager;
        }

        // Récupérer l'ID de l'utilisateur connecté
        private string GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("L'utilisateur n'est pas authentifié ou l'ID est manquant.");
            }
            return userId;
        }

        // Afficher la liste des comptes
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            try
            {
                var allAccounts = await _accountRepo.GetAll(userId);

                // Ajouter un message si aucun compte n'est trouvé
                if (!allAccounts.Any())
                {
                    ViewBag.Message = "Aucun compte trouvé pour cet utilisateur.";
                }

                return View(allAccounts);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        // Afficher les détails d'un compte
        public async Task<IActionResult> Details(int id)
        {
            var userId = GetCurrentUserId();
            try
            {
                var account = await _accountRepo.GetById(id, userId);

                // Déchiffrer le mot de passe
                account.Password = _encryptionService.Decrypt(account.Password);

                return View(account);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        // Afficher le formulaire de création d'un compte
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Traiter le formulaire de création d'un compte
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompteDTO compteDto)
        {
            var userId = GetCurrentUserId();
            try
            {
                // Générer l'URL du logo
                compteDto.LogoUrl = GetLogoFromClearbit(compteDto.Service);
                compteDto.Password = _encryptionService.Encrypt(compteDto.Password);
                var modelAccount = await _accountRepo.Add(compteDto, userId);
                return RedirectToAction(nameof(Index)); // Rediriger vers la page Index après la création
            }
            catch (Exception ex)
            {
                // Log l'erreur et rediriger vers la page d'erreur
                _logger.LogError(ex, "Erreur lors de la création d'un compte.");
                return HandleError(ex);
            }
        }

        // Afficher le formulaire de modification d'un compte
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetCurrentUserId();
            try
            {
                var account = await _accountRepo.GetById(id, userId);

                // Déchiffrer le mot de passe
                account.Password = _encryptionService.Decrypt(account.Password);

                var accountUpdateDto = new CompteUpdateDTO()
                {
                    Service = account.Service,
                    Username = account.Username,
                    Password = account.Password,
                    DateAdded = account.DateAdded
                };
                return View(accountUpdateDto);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        // Traiter le formulaire de modification d'un compte
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompteUpdateDTO accountUpdateDto)
        {
            var userId = GetCurrentUserId();
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si le modèle n'est pas valide, retournez une réponse JSON avec les erreurs
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new { success = false, message = "Erreur de validation.", errors });
                }

                // Chiffrer le mot de passe avant la mise à jour
                accountUpdateDto.Password = _encryptionService.Encrypt(accountUpdateDto.Password);

                await _accountRepo.Update(id, accountUpdateDto, userId);

                // Retournez une réponse JSON en cas de succès
                return Json(new { success = true, message = "Les modifications ont été enregistrées avec succès." });
            }
            catch (Exception ex)
            {
                // Retournez une réponse JSON en cas d'erreur
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Afficher la confirmation de suppression d'un compte
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            try
            {
                var account = await _accountRepo.GetById(id, userId);
                return View(account); // Affiche la vue Delete.cshtml
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        // Traiter la suppression d'un compte
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            try
            {
                var deleteAccount = await _accountRepo.Delete(id, userId);
                return RedirectToAction(nameof(Index)); // Redirige vers la page Index
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        // Rechercher des comptes par service ou nom d'utilisateur
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] QueryObject query)
        {
            var userId = GetCurrentUserId();
            try
            {
                var accounts = await _accountRepo.GetAccountByServiceAndName(query, userId);
                return View("Index", accounts);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        // Gestion des erreurs
        private IActionResult HandleError(Exception ex)
        {
            _logger.LogError(ex, "Erreur dans CompteController");

            if (ex is AccountNotFoundException)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            else
            {
                ViewBag.ErrorMessage = "Une erreur s'est produite. Veuillez réessayer plus tard.";
            }

            return View("Error");
        }

        // Récupérer l'URL du logo depuis Clearbit
        private string GetLogoFromClearbit(string serviceName)
        {
            // Si serviceName est null ou vide, retourne le logo de Sophos
            if (string.IsNullOrWhiteSpace(serviceName))
                return "https://logo.clearbit.com/test.com";

            // Construit l'URL Clearbit pour le serviceName
            string clearbitUrl = $"https://logo.clearbit.com/{serviceName.ToLower()}.com";

            // Vérifie si le logo existe via Clearbit
            bool logoExists = CheckIfLogoExists(clearbitUrl);

            // Si le logo n'existe pas, retourne le logo de Sophos
            if (!logoExists)
                return "https://logo.clearbit.com/test.com";

            // Sinon, retourne le logo Clearbit
            return clearbitUrl;
        }

        // Méthode pour vérifier si le logo existe via Clearbit
        private bool CheckIfLogoExists(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync(url).Result;
                    return response.IsSuccessStatusCode; // Retourne true si le logo existe
                }
            }
            catch
            {
                return false; // En cas d'erreur, retourne false
            }
        }

        // Afficher le profil de l'utilisateur
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = GetCurrentUserId();
            try
            {
                // Récupérer les informations de l'utilisateur authentifié via UserManager
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    throw new AccountNotFoundException("Utilisateur non trouvé.");
                }

                // Récupérer le nombre de comptes associés à l'utilisateur
                var numberOfAccounts = await _accountRepo.GetNumberOfAccountsByUserId(userId);

                // Passer les données à la vue
                ViewBag.NumberOfAccounts = numberOfAccounts;

                return View(user); // Retourne la vue Profile avec les données de l'utilisateur
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}