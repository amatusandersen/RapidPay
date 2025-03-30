using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RapidPay.Domain.Entities;

namespace RapidPay.Infrastructure.Persistence.Configurations
{
    public class ManualCardUpdateConfguration : IEntityTypeConfiguration<ManualCardUpdate>
    {
        public void Configure(EntityTypeBuilder<ManualCardUpdate> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Card)
                .WithMany(c => c.ManualUpdates)
                .HasForeignKey(u => u.CardId);
        }
    }
}
