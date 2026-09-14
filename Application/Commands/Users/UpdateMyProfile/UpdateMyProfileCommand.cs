using Application.Commons.DTOs;
using MediatR;

namespace Application.Commands.Users.UpdateMyProfile
{
    public class UpdateMyProfileCommand : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
        public UpdateMyProfileDto ProfileDto { get; set; }

        public UpdateMyProfileCommand(Guid userId, UpdateMyProfileDto profileDto)
        {
            UserId = userId;
            ProfileDto = profileDto;
        }
    }
}
