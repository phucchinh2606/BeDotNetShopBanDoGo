using Application.Commons.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
