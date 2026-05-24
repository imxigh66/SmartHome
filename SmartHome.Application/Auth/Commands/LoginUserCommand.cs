using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Auth.Commands
{
    public record LoginUserCommand(
    string Email,
    string Password
) : IRequest<LoginUserResult>;

    public record LoginUserResult(
        string AccessToken,
        string RefreshToken,
        Guid UserId,
        string Name,
        string Email
    );
}
