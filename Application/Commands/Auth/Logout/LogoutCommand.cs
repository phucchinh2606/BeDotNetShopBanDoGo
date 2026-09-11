using MediatR;

namespace Application.Commands.Auth.Logout
{
    public class LogoutCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }

        public LogoutCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
