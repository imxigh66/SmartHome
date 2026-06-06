using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Application.Common.Behaviors;


namespace SmartHome.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(
					typeof(DependencyInjection).Assembly);
				cfg.AddBehavior(
					typeof(IPipelineBehavior<,>),
					typeof(ValidationBehavior<,>));
			});

			services.AddValidatorsFromAssembly(
				typeof(DependencyInjection).Assembly);

			return services;
        }
    }
}
