using Microsoft.EntityFrameworkCore;
using RapidPay.Application.Services.Factories;
using RapidPay.Domain.Constants;
using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories.Common;
using RapidPay.Infrastructure.Persistence;
using RapidPay.Infrastructure.Persistence.Repositories;
using RapidPay.Infrastructure.Persistence.Repositories.Common;

namespace RapidPay.Api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringName = DatabaseConnectionStrings.SqlServer;
            var connectionString = configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NoConnectionStringException(connectionStringName);
            }

            services.AddInfrastructureServices(connectionString)
                .AddApplicationServices();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RapidPayDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IAuthorizationLogRepository, AuthorizationLogRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            services.AddSingleton<Random>();

            services.AddSingleton<ICardFactory, CardFactory>();

            return services;
        }
    }
}
