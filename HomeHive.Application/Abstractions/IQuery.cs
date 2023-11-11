using HomeHive.Domain.Common;
using MediatR;

namespace HomeHive.Application.Abstractions;

public interface IQuery<TResponse>: IRequest<Result<TResponse>> where TResponse : class
{
}