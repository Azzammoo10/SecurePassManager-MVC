using FluentValidation;
using WatchlistV2.DTOs;

namespace WatchlistV2.Validator
{
    public class RegisterDToValidators : AbstractValidator<RegisterDTo>
    {
        public RegisterDToValidators()
        {
            // Validation pour Username
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Le nom d'utilisateur est requis.")
                .MinimumLength(3).WithMessage("Le nom d'utilisateur doit contenir au moins 3 caractères.")
                .MaximumLength(20).WithMessage("Le nom d'utilisateur doit contenir au maximum 20 caractères.")
                .Matches("^[a-zA-Z0-9_]*$").WithMessage("Le nom d'utilisateur ne doit contenir que des lettres, chiffres et underscores.");

            // Validation pour Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'email est requis.")
                .EmailAddress().WithMessage("L'email doit être valide.");

            // Validation pour Password
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Le mot de passe est requis.")
                .MinimumLength(8).WithMessage("Le mot de passe doit contenir au moins 8 caractères.")
                .MaximumLength(16).WithMessage("Le mot de passe doit contenir 16 caractères au maximum.")
                .Matches("[A-Z]").WithMessage("Le mot de passe doit contenir au moins une majuscule.")
                .Matches("[a-z]").WithMessage("Le mot de passe doit contenir au moins une minuscule.")
                .Matches("[0-9]").WithMessage("Le mot de passe doit contenir au moins un chiffre.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Le mot de passe doit contenir au moins un caractère spécial.");

        }
    }
}
