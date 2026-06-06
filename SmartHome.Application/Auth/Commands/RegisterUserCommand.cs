using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Auth.Commands
{
    public record RegisterUserCommand(
     string Name,
     string Email,
     string Password
 ) : IRequest<RegisterUserResult>;

    public record RegisterUserResult(
    string Email,
    string Error
);
}
