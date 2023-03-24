using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using Forbids;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Users;

public record UpdateUserInformationCommand
    (UpdateUserInformationRequest UpdateUserInformationRequest) : IRequestWrapper<bool>;

internal sealed class UpdateUserInformationCommandHandler : IHandlerWrapper<UpdateUserInformationCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IForbid _forbid;

    public UpdateUserInformationCommandHandler(UserManager<User> userManager, IMapper mapper, IForbid forbid) =>
        (_userManager, _mapper, _forbid) = (userManager, mapper, forbid);
        

    public async Task<IResponse<bool>> Handle(UpdateUserInformationCommand request, CancellationToken cancellationToken)
    {
        var updateInfoDto = request.UpdateUserInformationRequest;
        var user = await _userManager.FindByEmailAsync(updateInfoDto.Email);
        _forbid.Null(user, UserNotFoundException.Instance);
        _mapper.Map(updateInfoDto,user);
        var updateResult= await _userManager.UpdateAsync(user);
        return new Response<bool>(updateResult.Succeeded, updateResult.Succeeded);
    }
}