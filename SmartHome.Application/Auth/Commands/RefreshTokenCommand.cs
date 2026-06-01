using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Auth.Commands
{
    public record RefreshTokenCommand(
     string RefreshToken
 ) : IRequest<LoginUserResult>;
}
