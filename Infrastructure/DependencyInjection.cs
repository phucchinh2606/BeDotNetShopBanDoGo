using Application.Commons.Interfaces;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                ));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Đăng ký Các Repository cụ thể
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            // Đăng ký Payment Factory
            services.AddScoped<IPaymentFactory, PaymentFactory>();

            // Đăng ký tất cả các triễn khai của IPaymentService
            services.AddScoped<IPaymentService, CodPaymentService>();

            // Đăng ký Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IPhotoService, PhotoService>();

            return services;
        }
    }
}
