using Evently.Common.Domain.Results;
using MediatR;

namespace Evently.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
