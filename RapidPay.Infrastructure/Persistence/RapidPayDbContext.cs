using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Entities;
using System.Reflection;

namespace RapidPay.Infrastructure.Persistence
{
    public class RapidPayDbContext(DbContextOptions<RapidPayDbContext> options) : DbContext(options)
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<AuthorizationLog> AuthorizationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
