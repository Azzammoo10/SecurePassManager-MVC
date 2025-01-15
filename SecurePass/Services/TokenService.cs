using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WatchlistV2.Interface;
using WatchlistV2.Models;

namespace WatchlistV2.Services
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _configuration;  // Pour accéder à la configuration (appsettings.json)
        private readonly SymmetricSecurityKey _key; // Clé symétrique utilisée pour signer le jeton

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;

            // Récupération de la clé de signature depuis appsettings.json
            var signingKey = _configuration["JWT:SigninKey"];

            if (string.IsNullOrEmpty(signingKey))
            {
                throw new ArgumentNullException("JWT:SigningKey", "Signing key is missing from configuration.");
            }

            // Crée la clé symétrique si la clé de signature est valide
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }

        public string CreateToken(Utilisateur user)
        {
            // Vérification si les informations de l'utilisateur sont valides avant de les ajouter aux claims
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "AppUser cannot be null.");
            }

            // Vérification des valeurs avant d'ajouter les claims
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException(nameof(user.Email), "Email cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentNullException(nameof(user.UserName), "Username cannot be null or empty.");
            }

            // Création des claims (revendications) à inclure dans le jeton
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),  // Ajout de l'email
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),  // Ajout du nom d'utilisateur
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Ajout de l'ID de l'utilisateur
            };

            // Définition des informations de signature avec la clé symétrique
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Vérification des paramètres JWT dans la configuration
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];

            if (string.IsNullOrEmpty(issuer))
            {
                throw new ArgumentNullException("JWT:Issuer", "Issuer is missing in configuration.");
            }

            if (string.IsNullOrEmpty(audience))
            {
                throw new ArgumentNullException("JWT:Audience", "Audience is missing in configuration.");
            }

            // Définition du jeton avec la liste des claims, durée d'expiration, émetteur, et audience
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),  // Ajout des revendications
                Expires = DateTime.Now.AddDays(7),     // Durée d'expiration du jeton (7 jours)
                SigningCredentials = creds,            // Algorithme de signature
                Issuer = issuer,                       // Émetteur du jeton
                Audience = audience                    // Audience du jeton
            };

            // Création du jeton avec les informations définies
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Retourner le jeton sous forme de chaîne JWT
            return tokenHandler.WriteToken(token);
        }
    }
}

