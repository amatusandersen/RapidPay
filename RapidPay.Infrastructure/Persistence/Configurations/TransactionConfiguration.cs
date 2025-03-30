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

            builder.HasOne(t => t.SenderCard)
                .WithMany(c => c.OutcomingTransactions)
                .HasForeignKey(t => t.SenderCardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.RecipientCard)
                .WithMany(c => c.IncomingTransactions)
                .HasForeignKey(t => t.RecipientCardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.TransactionAmount).HasColumnType("decimal(18,2)");
            builder.Property(t => t.FeeAmount).HasColumnType("decimal(18,2)");
        }
    }
}
