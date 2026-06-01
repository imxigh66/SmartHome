using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Auth.Commands;
using SmartHome.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Auth.CommandHandler
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginUserResult>
    {
        private readonly IAppDbContext _context;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(
            IAppDbContext context,
            IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        public async Task<LoginUserResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);
            if (user == null) {
                throw new Exception("Invalid refresh token");
            }

            if(user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Refresh token expired");
            }

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

            await _context.SaveChangesAsync(cancellationToken);

            return new LoginUserResult(
               newAccessToken,
            newRefreshToken,
            user.Id,
            user.Name,
            user.Email
            );
        }
    }
}
