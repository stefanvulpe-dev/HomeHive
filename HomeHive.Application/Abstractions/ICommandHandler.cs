using MediatR;

namespace HomeHive.Application.Abstractions;

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : class
{
}