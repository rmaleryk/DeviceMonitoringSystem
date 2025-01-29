using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.Monitor.Infrastructure.Persistence.EventStore;

internal sealed class StoredEventConfiguration : IEntityTypeConfiguration<StoredEvent>
{
    public void Configure(EntityTypeBuilder<StoredEvent> builder)
    {
        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.AggregateId)
            .IsRequired();

        builder
            .Property(e => e.AggregateType)
            .IsRequired();

        builder
            .Property(e => e.EventType)
            .IsRequired();

        builder
            .Property(e => e.Data)
            .IsRequired();

        builder
            .Property(e => e.OccurredOn)
            .IsRequired();

        builder
            .Property(e => e.Version)
            .IsRequired();
    }
}
