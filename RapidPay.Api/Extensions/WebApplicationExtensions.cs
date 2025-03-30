using Microsoft.EntityFrameworkCore;
using RapidPay.Infrastructure.Persistence;

namespace RapidPay.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<RapidPayDbContext>();

            await context.Database.MigrateAsync();
        }

    }
}
