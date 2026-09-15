# 1. Build Stage (.NET 10 SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy tất cả file .csproj vào đúng thư mục tương ứng để tối ưu Docker Layer Caching
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restore dependencies
RUN dotnet restore "API/API.csproj"

# Copy toàn bộ mã nguồn vào container
COPY . .

# Build và Publish duy nhất project API (tự động kéo các project liên quan)
WORKDIR "/src/API"
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime Stage (.NET 10 ASP.NET Core Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render tự động cấp cổng thông qua biến môi trường PORT (mặc định 8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Chạy file DLL chính của project API
ENTRYPOINT ["dotnet", "API.dll"]