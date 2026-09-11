using MediatR;

namespace Application.Commands.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponseDto>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
