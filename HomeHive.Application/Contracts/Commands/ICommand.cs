using MediatR;

namespace HomeHive.Application.Contracts.Commands;

public interface ICommand<TResponse> : IRequest<TResponse> where TResponse : class
{
}