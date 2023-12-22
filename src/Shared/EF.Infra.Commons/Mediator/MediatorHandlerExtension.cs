using EF.Domain.Commons.DomainObjects;
using EF.Domain.Commons.Mediator;
using Microsoft.EntityFrameworkCore;

namespace EF.Infra.Commons.Mediator;

public static class MediatorHandlerExtension
{
    public static async Task PublishEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notifications)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearEvents());

        var tasks = domainEvents
            .Select(async (domainEvent) => { await mediator.Publish(domainEvent); });

        await Task.WhenAll(tasks);
    }
}