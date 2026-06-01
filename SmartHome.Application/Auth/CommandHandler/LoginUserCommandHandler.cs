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
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
    {
        private readonly IAppDbContext _context;
        private readonly IJwtService _jwtService;
        public LoginUserCommandHandler(IAppDbContext context,IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user=await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email.ToLower(), cancellationToken);

            if (user == null) {
                throw new Exception("Invalid email or password.");
            }


            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

            await _context.SaveChangesAsync(cancellationToken);

            return new LoginUserResult(
                accessToken,
                refreshToken,
                user.Id,
                user.Name,
                user.Email
            );

        }
    }
}
