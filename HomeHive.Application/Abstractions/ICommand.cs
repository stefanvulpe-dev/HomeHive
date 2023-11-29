using MediatR;

namespace HomeHive.Application.Abstractions;

public interface ICommand<TResponse> : IRequest<TResponse> where TResponse : class
{
}