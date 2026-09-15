# 🪵 E-Commerce Wood Shop Backend (.NET 10)

Hệ thống Backend RESTful API chuyên biệt cho trang thương mại điện tử đồ gỗ mỹ nghệ cao cấp. Dự án được phát triển trên nền tảng **.NET 10 mới nhất**, áp dụng các nguyên lý thiết kế phần mềm tiên tiến nhằm đảm bảo tính mở rộng, bảo mật cao và hiệu năng tối ưu.

---

## 🚀 Công Nghệ Sử Dụng

* **Core Framework:** .NET 10 Web API
* **Architecture:** Clean Architecture (Onion Architecture)
* **Design Pattern:** CQRS Pattern với MediatR
* **Database & ORM:** SQL Server / PostgreSQL, Entity Framework Core (Code First)
* **Authentication & Authorization:** JWT (JSON Web Token), Role-based Authorization
* **Real-time Communication:** ASP.NET Core SignalR (Thông báo đơn hàng, cập nhật trạng thái)
* **Payment Gateway:** Thanh toán trực tuyến qua **PayOS**
* **Cloud Storage:** **Cloudinary API** (Quản lý & tối ảnh sản phẩm)
* **AI Integration:** Chatbot AI tư vấn sản phẩm & kích thước chuẩn Lỗ Ban phong thủy

---

## 🏗️ Kiến Trúc Hệ Thống (Clean Architecture)

Dự án được chia làm 4 layer độc lập tuân thủ nguyên lý Dependency Inversion:

```text
├── src/
│   ├── Core/
│   │   ├── Domain/              # Entities, Enums, Value Objects, Exceptions
│   │   └── Application/         # CQRS (Commands/Queries), DTOs, Interfaces, Validators (FluentValidation)
│   ├── Infrastructure/
│   │   ├── Infrastructure/      # External Services (PayOS, Cloudinary, Chatbot AI, SignalR Hubs)
│   │   └── Persistence/         # EF Core DbContext, Migrations, Repositories Implementation
│   └── Presentation/
│       └── WebAPI/              # Controllers, Middlewares, Program.cs, Configurations
