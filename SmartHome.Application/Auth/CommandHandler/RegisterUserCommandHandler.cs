using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Auth.Commands;
using SmartHome.Application.Common.Interfaces;
using SmartHome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Auth.CommandHandler
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly IAppDbContext _context;
        public RegisterUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var exists=await _context.Users.AnyAsync(u => u.Email == request.Email.ToLower(), cancellationToken);
            if (exists)
            {
                throw new Exception("User with this email already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email.ToLower().Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            //дефолтные настройки тарифа для нового пользователя
            var tariffSettings = new TariffSettings
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Provider = "PREMIER_ENERGY",
                PlanType = "SINGLE_ZONE",
                SingleRate = 3.59m,
                DayRate = 3.89m,
                NightRate = 3.31m
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.TariffSettings.AddAsync(tariffSettings, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new RegisterUserResult
            (
                user.Id,
                user.Email,
                user.Name
            );
        }
    }
}
