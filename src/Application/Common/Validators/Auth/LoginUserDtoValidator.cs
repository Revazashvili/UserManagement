using Application.Common.DTOs.Auth;
using FluentValidation;

namespace Application.Common.Validators.Auth
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().NotNull()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull();
        }
    }
}