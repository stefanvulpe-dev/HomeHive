using MediatR;

namespace HomeHive.Application.Contracts.Queries;

public interface IQuery<TResponse> : IRequest<TResponse> where TResponse : class
{
}