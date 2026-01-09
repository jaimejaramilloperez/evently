using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.TicketTypes;

internal sealed class TicketTypeRepository(EventsDbContext dbContext)
    : ITicketTypeRepository
{
    public async Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.TicketTypes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await dbContext.TicketTypes.AnyAsync(x => x.EventId == eventId, cancellationToken);
    }

    public void Insert(TicketType ticketType)
    {
        dbContext.TicketTypes.Add(ticketType);
    }
}
