using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transfer.Application.Orchestrators;

namespace Transfer.Storage
{
    class TransferStateEntityConfiguration :
        IEntityTypeConfiguration<TransferState>
    {
        public void Configure(EntityTypeBuilder<TransferState> builder)
        {
            builder.HasKey(c => c.CorrelationId);

            builder.Property(c => c.CorrelationId)
                .ValueGeneratedNever()
                .HasColumnName("TransferId");

            builder.Property(c => c.CurrentState).IsRequired();
            builder.Property(x => x.SourceAccountId);
            builder.Property(x => x.SourceAccountVersion);
            builder.Property(x => x.TargetAccountId);
            builder.Property(x => x.TargetAccountVersion);
            builder.Property(x => x.Sum);
            builder.Property(x => x.Comment);
        }
    }
}
