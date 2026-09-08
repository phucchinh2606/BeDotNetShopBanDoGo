using MediatR;

namespace Application.Commands.Users.DeleteUser
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
