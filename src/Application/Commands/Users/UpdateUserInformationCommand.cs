using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Users
{
    public record UpdateUserInformationCommand(UpdateUserInformationDto UpdateUserInformationDto) : IRequestWrapper<bool>{}

    public class UpdateUserInformationCommandHandler : IHandlerWrapper<UpdateUserInformationCommand, bool>
    {
        private readonly UserManager<User> _userManager;

        public UpdateUserInformationCommandHandler(UserManager<User> userManager) => _userManager = userManager;
        
        public async Task<IResponse<bool>> Handle(UpdateUserInformationCommand request, CancellationToken cancellationToken)
        {
            var updateInfoDto = request.UpdateUserInformationDto;
            var user = await _userManager.FindByEmailAsync(updateInfoDto.Email);
            if(user is null) return Response.Fail<bool>("Can't find user with provided email");
            user.Address = updateInfoDto.Address;
            user.Compensation = updateInfoDto.Compensation;
            user.Employed = updateInfoDto.Employed;
            user.IsMarried = updateInfoDto.IsMarried;
            user.Pin = user.Pin;
            var updateResult= await _userManager.UpdateAsync(user);
            return new Response<bool>(updateResult.Succeeded, updateResult.Succeeded);
        }
    }
}