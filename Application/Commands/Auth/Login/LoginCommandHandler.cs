using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Tìm user theo email bằng hàm tối ưu đã định nghĩa trong IUserRepository[cite: 20]
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new System.Exception("Email không tồn tại trong hệ thống.");
            }

            // 2. Kiểm tra tính chính xác của mật khẩu đã được mã hóa bằng BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new System.Exception("Mật khẩu không chính xác.");
            }

            // 3. Sinh JWT Token thông qua ITokenService[cite: 18]
            var token = _tokenService.GenerateToken(user);

            // 4. Trả về thông tin kèm token cho client
            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
        }
    }
}
