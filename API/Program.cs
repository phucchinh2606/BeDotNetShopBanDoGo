using API;
using API.Hubs;
using API.Middlewares;
using Application.Commons.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://fe-next-js-shop-ban-go.vercel.app/") // Thêm URL của Frontend Next.js
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Bắt buộc nếu dùng Cookie/Auth Header
    });
});

// Đăng ký toàn bộ DI cho API, Application và Infrastructure
builder.Services.AddApiDI(builder.Configuration);

// Cấu hình JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddHttpContextAccessor();

// 1. Thêm MemoryCache & SignalR
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();



var app = builder.Build();
// 2. Map SignalR Hub Endpoint
app.MapHub<ChatHub>("/chatHub");
// Tự động seed tài khoản Admin khi khởi động ứng dụng
using (var scope = app.Services.CreateScope())
{
    await Infrastructure.Data.Seeders.AdminSeeder.SeedAdminAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Artistic Wood Products");
        c.RoutePrefix = string.Empty; // mở Swagger UI tại root
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// 2. Kích hoạt CORS (Lưu ý: Bắt buộc đặt trước UseAuthentication và UseAuthorization)
app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();