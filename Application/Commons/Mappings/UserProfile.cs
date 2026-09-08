using Application.Commands.Users.CreateUser;
using Application.Commands.Users.UpdateUser;
using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map từ CreateUserCommand -> User (Bỏ qua PasswordHash để gán riêng bằng BCrypt)
            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // Map từ UpdateUserCommand -> User
            CreateMap<UpdateUserCommand, User>();

            // Map từ User -> UserDto
            CreateMap<User, UserDto>();
        }
    }
}
