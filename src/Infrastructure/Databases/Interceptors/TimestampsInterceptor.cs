using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Databases.Interceptors;

public class TimestampsInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateTimestamps(eventData.Context!);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateTimestamps(eventData.Context!);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateTimestamps(DbContext context)
    {
        var now = DateTime.UtcNow;
        foreach (var entry in context.ChangeTracker.Entries()
                     .Where(e => e is {Entity: IHasTimestamps, State: EntityState.Added or EntityState.Modified}))
        {
            var entity = (IHasTimestamps) entry.Entity;
            if (entry.State == EntityState.Added)
                entity.CreatedAt = now;
            entity.UpdatedAt = now;
        }
    }
}