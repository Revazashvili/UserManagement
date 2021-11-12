using Application.Common.Models;
using MediatR;

namespace Application.Common.Wrappers;

/// <summary>
/// Wrapper Interface For <see cref="IRequest"/> To Return <see cref="IResponse{T}"/>
/// </summary>
public interface IRequestWrapper<T> : IRequest<IResponse<T>> { }

/// <summary>
/// Wrapper Interface For <see cref="IRequestHandler{TRequest,TResponse}"/> To Handle <see cref="IRequestWrapper{T}"/>
/// </summary>
public interface IHandlerWrapper<in TRequest, TResponse> :
    IRequestHandler<TRequest, IResponse<TResponse>> where TRequest : IRequestWrapper<TResponse> { }