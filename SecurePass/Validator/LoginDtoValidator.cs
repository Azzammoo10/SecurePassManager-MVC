using FluentValidation;
using WatchlistV2.DTOs;

namespace WatchlistV2.Validator
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            // Validation pour Username
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Le nom d'utilisateur est requis.")
                .MinimumLength(3).WithMessage("Le nom d'utilisateur doit contenir au moins 3 caractères.")
                .MaximumLength(20).WithMessage("Le nom d'utilisateur doit contenir au maximum 20 caractères.");

            // Validation pour Password
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Le mot de passe est requis.")
                .MinimumLength(8).WithMessage("Le mot de passe doit contenir au moins 8 caractères.")
                .MaximumLength(16).WithMessage("Le mot de passe doit contenir 16 caractères au maximum.");
        }
    }
}
