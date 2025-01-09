using System.Transactions;
using DMS.Monitor.Domain.Base;
using DMS.Monitor.Domain.Boilers;
using DMS.Monitor.Infrastructure.Persistence.Boilers;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace DMS.Monitor.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDomainEventDispatcher domainEventDispatcher)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public DbSet<Boiler> Boilers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        const string resilienceSchema = "MassTransit";

        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity(e => e.ToTable("InboxState", resilienceSchema));
        modelBuilder.AddOutboxMessageEntity(e => e.ToTable("OutboxMessage", resilienceSchema));
        modelBuilder.AddOutboxStateEntity(e => e.ToTable("OutboxState", resilienceSchema));

        modelBuilder
            .ApplyConfiguration(new BoilerTypeConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
        {
            return await SaveAndPublishEvents(cancellationToken);
        }

        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            },
            TransactionScopeAsyncFlowOption.Enabled);

        var result = await SaveAndPublishEvents(cancellationToken);
        transaction.Complete();

        return result;
    }

    private async Task<int> SaveAndPublishEvents(CancellationToken cancellationToken)
    {
        await PublishDomainEvents();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEvents()
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = new List<DomainEvent>();

        foreach (AggregateRoot aggregateRoot in aggregateRoots)
        {
            domainEvents.AddRange(aggregateRoot.DomainEvents);
            aggregateRoot.ClearDomainEvents();
        }

        foreach (DomainEvent domainEvent in domainEvents)
        {
            await _domainEventDispatcher.Dispatch(domainEvent);
        }
    }
}
