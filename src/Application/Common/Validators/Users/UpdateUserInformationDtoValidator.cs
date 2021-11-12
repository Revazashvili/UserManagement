using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Validators.Users;

public class UpdateUserInformationDtoValidator : AbstractValidator<UpdateUserInformationRequest>
{
    private readonly UserManager<User> _userManager;

    public UpdateUserInformationDtoValidator(UserManager<User> userManager)
    {
        _userManager = userManager;
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().EmailAddress()
            .MustAsync(ExistsAsync);
            
        RuleFor(x => x.Pin)
            .Length(11)
            .NotEmpty().NotNull();
            
        RuleFor(x => x.Employed).NotNull();
        RuleFor(x => x.IsMarried).NotNull();
    }

    private async Task<bool> ExistsAsync(string email,CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user is not null;
    }
}