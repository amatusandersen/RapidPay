using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RapidPay.Domain.Entities;

namespace RapidPay.Infrastructure.Persistence.Configurations
{
    public class AuthorizationLogConfiguration : IEntityTypeConfiguration<AuthorizationLog>
    {
        public void Configure(EntityTypeBuilder<AuthorizationLog> builder)
        {
            builder.HasKey(l => l.Id);

            builder.HasOne(l => l.Card)
                .WithMany(c => c.AuthorizationLogs)
                .HasForeignKey(l => l.CardId);
        }
    }
}
