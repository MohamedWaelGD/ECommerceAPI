using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Auth.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(IIdentityService identityService) : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
{
    public Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken) =>
        identityService.GetCurrentUserAsync(request.UserId, cancellationToken);
}
