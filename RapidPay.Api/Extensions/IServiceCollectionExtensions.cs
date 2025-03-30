using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RapidPay.Application.Services;
using RapidPay.Application.Services.Factories;
using RapidPay.Domain.Constants;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories.Common;
using RapidPay.Domain.Interfaces.Services;
using RapidPay.Infrastructure.Persistence;
using RapidPay.Infrastructure.Persistence.Repositories;
using RapidPay.Infrastructure.Persistence.Repositories.Common;
using System.Text;

namespace RapidPay.Api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                };
            });

            return services;
        }

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

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RapidPayDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IAuthorizationLogRepository, AuthorizationLogRepository>();
            services.AddScoped<IFeeRepository, FeeRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IManualCardUpdateRepository, ManualCardUpdateRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

            services.AddScoped<ICardFactory, CardFactory>();
            services.AddScoped<IAuthorizationLogFactory, AuthorizationLogFactory>();
            services.AddScoped<ITransactionFactory, TransactionFactory>();
            services.AddScoped<IManualCardUpdateFactory, ManualCardUpdateFactory>();
            services.AddScoped<IUserFactory, UserFactory>();

            services.AddHostedService<FeeService>();

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
