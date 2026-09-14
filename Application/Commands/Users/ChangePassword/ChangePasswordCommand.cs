using Application.Commons.DTOs;
using MediatR;

namespace Application.Commands.Users.ChangePassword
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public ChangePasswordDto PasswordDto { get; set; }

        public ChangePasswordCommand(Guid userId, ChangePasswordDto passwordDto)
        {
            UserId = userId;
            PasswordDto = passwordDto;
        }
    }
}
