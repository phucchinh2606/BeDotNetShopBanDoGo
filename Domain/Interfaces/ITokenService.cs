using Domain.Entities;
using System.Security.Claims;

namespace Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        (string Token, DateTime ExpiryTime) GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
