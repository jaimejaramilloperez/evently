using System.Data.Common;
using Dapper;
using Evently.Common.Application.Authorization;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.GetUserPermissions;

internal sealed class GetUserPermissionsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
    public async Task<Result<PermissionsResponse>> Handle(
        GetUserPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT DISTINCT
                u.id AS {nameof(UserPermission.UserId)},
                rp.permission_code AS {nameof(UserPermission.Permission)}
            FROM
                users.users AS u
            JOIN
                users.user_roles AS ur ON ur.user_id = u.id
            JOIN
                users.role_permissions AS rp ON rp.role_name = ur.role_name
            WHERE
                u.identity_id = @IdentityId::uuid
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { request.IdentityId },
            cancellationToken: cancellationToken);

        IEnumerable<UserPermission> enumerable = await connection.QueryAsync<UserPermission>(command);

        List<UserPermission> permissions = enumerable.AsList();

        if (permissions.Count == 0)
        {
            return Result.Failure<PermissionsResponse>(UserErrors.NotFound(request.IdentityId));
        }

        return new PermissionsResponse()
        {
            UserId = permissions[0].UserId,
            Permissions = permissions.Select(p => p.Permission).ToHashSet(),
        };
    }

    internal sealed class UserPermission
    {
        internal Guid UserId { get; init; }
        internal string Permission { get; init; } = string.Empty;
    }
}
