using Evently.Common.Domain.Results;
using MediatR;

namespace Evently.Common.Application.Messaging;

public interface IBaseCommand;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;
