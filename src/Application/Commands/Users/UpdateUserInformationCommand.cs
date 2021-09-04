using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Users
{
    public record UpdateUserInformationCommand(UpdateUserInformationDto UpdateUserInformationDto) : IRequestWrapper<bool>{}

    public class UpdateUserInformationCommandHandler : IHandlerWrapper<UpdateUserInformationCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UpdateUserInformationCommandHandler(UserManager<User> userManager, IMapper mapper) =>
            (_userManager, _mapper) = (userManager, mapper);
        
        public async Task<IResponse<bool>> Handle(UpdateUserInformationCommand request, CancellationToken cancellationToken)
        {
            var updateInfoDto = request.UpdateUserInformationDto;
            var user = await _userManager.FindByEmailAsync(updateInfoDto.Email);
            if (user is null) throw new Exception("Can't find user with provided email");
            _mapper.Map(updateInfoDto,user);
            var updateResult= await _userManager.UpdateAsync(user);
            return new Response<bool>(updateResult.Succeeded, updateResult.Succeeded);
        }
    }
}