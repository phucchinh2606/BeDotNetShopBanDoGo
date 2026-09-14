using Application.Commons.DTOs;
using Application.Commons.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Users.UpdateMyProfile
{
    public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateMyProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException("Người dùng", request.UserId);
            }

            user.FullName = request.ProfileDto.FullName;
            user.PhoneNumber = request.ProfileDto.PhoneNumber;
            user.Address = request.ProfileDto.Address;

            _unitOfWork.Users.Update(user); 
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
