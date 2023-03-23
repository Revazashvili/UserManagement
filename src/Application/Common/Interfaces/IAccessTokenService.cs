using Netjection;

namespace Application.Common.Interfaces;

/// <summary>
/// Interface for generating access token.
/// </summary>
[InjectAsScoped]
public interface IAccessTokenService : ITokenService { }