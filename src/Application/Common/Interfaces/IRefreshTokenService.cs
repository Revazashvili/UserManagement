using Netjection;

namespace Application.Common.Interfaces;

/// <inheritdoc cref="ITokenService"/>
[InjectAsScoped]
public interface IRefreshTokenService : ITokenService { }