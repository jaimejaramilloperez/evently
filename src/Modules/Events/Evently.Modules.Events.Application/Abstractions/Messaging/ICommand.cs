using Evently.Modules.Events.Domain.Abstractions.Results;
using MediatR;

namespace Evently.Modules.Events.Application.Abstractions.Messaging;

public interface IBaseCommand;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;
