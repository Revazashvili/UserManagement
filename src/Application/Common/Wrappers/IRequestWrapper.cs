using Application.Common.Models;
using MediatR;

namespace Application.Common.Wrappers
{
    public interface IRequestWrapper<T> : IRequest<IResponse<T>> { }

    public interface IHandlerWrapper<in TRequest, TResponse> :
        IRequestHandler<TRequest, IResponse<TResponse>> where TRequest : IRequestWrapper<TResponse> { }
}