using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RapidPay.Domain.Entities;

namespace RapidPay.Infrastructure.Persistence.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.Balance).HasColumnType("decimal(18,2)");
            builder.Property(c => c.CreditLimit).HasColumnType("decimal(18,2)").IsRequired(false);
        }
    }
}
