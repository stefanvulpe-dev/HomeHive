using MediatR;

namespace HomeHive.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse>: IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : class
{
}