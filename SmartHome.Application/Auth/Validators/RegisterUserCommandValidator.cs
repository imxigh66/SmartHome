using FluentValidation;
using SmartHome.Application.Auth.Commands;

namespace SmartHome.Application.Auth.Validators
{
	public class RegisterUserCommandValidator:AbstractValidator<RegisterUserCommand>
	{
		public RegisterUserCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Name is required")
				.MinimumLength(2).WithMessage("Name must be at least 2 characters")
				.MaximumLength(100);

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required")
				.EmailAddress().WithMessage("Invalid email format")
				.MaximumLength(200);

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required")
				.MinimumLength(8).WithMessage("Password must be at least 8 characters")
				.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
				.Matches("[0-9]").WithMessage("Password must contain at least one number");
		}
	}
}
