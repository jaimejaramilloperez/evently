using Evently.Common.Domain.Results;

namespace Evently.Modules.Users.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<Result<Guid>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
}
