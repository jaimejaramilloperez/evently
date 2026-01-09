using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Categories;

internal sealed class CategoryRepository(EventsDbContext dbContext)
    : ICategoryRepository
{
    public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Insert(Category category)
    {
        dbContext.Categories.Add(category);
    }
}
