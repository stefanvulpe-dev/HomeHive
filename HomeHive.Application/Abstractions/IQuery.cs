using MediatR;

namespace HomeHive.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<TResponse> where TResponse : class
{
}