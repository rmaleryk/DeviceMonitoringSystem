using DMS.Monitor.Domain.Boilers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.Monitor.Infrastructure.Persistence.Boilers;

internal sealed class BoilerTypeConfiguration : IEntityTypeConfiguration<Boiler>
{
    public void Configure(EntityTypeBuilder<Boiler> builder)
    {
        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.Name)
            .IsRequired();

        builder
            .Property(e => e.State)
            .IsRequired();

        builder.OwnsOne(e => e.CurrentTemperature);
    }
}
