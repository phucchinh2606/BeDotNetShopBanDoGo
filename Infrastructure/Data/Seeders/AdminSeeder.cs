using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Seeders
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                // Kiểm tra xem trong hệ thống đã có tài khoản nào mang quyền Admin chưa
                // Ở đây chúng ta có thể duyệt qua user hoặc query email mặc định
                var adminEmail = "admin123@gmail.com";
                var existingAdmin = await unitOfWork.Users.GetByEmailAsync(adminEmail);

                if (existingAdmin == null)
                {
                    var adminUser = new User
                    {
                        FullName = "System Administrator",
                        Email = adminEmail,
                        // Mã hóa mật khẩu mặc định (Ví dụ: Admin@123)
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        PhoneNumber = "0123456789",
                        Address = "Hà Nội, Việt Nam",
                        Role = UserRole.Admin, // Gán role Admin
                        CreatedAt = DateTime.UtcNow
                    };

                    await unitOfWork.Users.AddAsync(adminUser);
                    await unitOfWork.SaveChangesAsync();

                    logger.LogInformation("Đã tự động khởi tạo tài khoản Admin thành công: {Email}", adminEmail);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Đã xảy ra lỗi trong quá trình khởi tạo tài khoản Admin.");
            }
        }
    }
}
