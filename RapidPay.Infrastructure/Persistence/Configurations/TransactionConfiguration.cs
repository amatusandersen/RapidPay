using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RapidPay.Domain.Entities;

namespace RapidPay.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TransactionAmount).HasColumnType("decimal(18,2)");
            builder.Property(t => t.FeeAmount).HasColumnType("decimal(18,2)");
        }
    }
}
