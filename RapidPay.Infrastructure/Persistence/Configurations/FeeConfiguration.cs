using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RapidPay.Domain.Entities;

namespace RapidPay.Infrastructure.Persistence.Configurations
{
    public class FeeConfiguration : IEntityTypeConfiguration<Fee>
    {
        public void Configure(EntityTypeBuilder<Fee> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Amount).HasColumnType("decimal(18,2)");
        }
    }
}
