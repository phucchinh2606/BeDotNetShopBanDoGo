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

✨ Tính Năng Chính
Quản lý Sản Phẩm & Danh Mục: Quản lý thông tin đồ gỗ, chất liệu (Gụ, Hương, Gõ...), loại vân, kích thước chuẩn Lỗ Ban.

Xác thực & Phân quyền (JWT): Đăng ký, đăng nhập, cấp Refresh Token, phân quyền Client / Admin.

Thanh Toán Trực Tuyến (PayOS): Tạo mã QR thanh toán ngân hàng tự động, xử lý Webhook cập nhật đơn hàng tức thì.

Thông Báo Real-time (SignalR): Áp dụng cho chatbotAI phản hồi thời gian thực.

Tư Vấn AI (Chatbot AI): Hỗ trợ khách hàng tìm kiếm sản phẩm theo nhu cầu và tư vấn phong thủy đồ gỗ.

Tải Ảnh Lên Cloud (Cloudinary): Tự động tối ưu dung lượng và phân giải ảnh gỗ trước khi lưu trữ trên mây.

## 🏗️ Kiến Trúc Hệ Thống (Clean Architecture)

Dự án được chia làm 4 layer độc lập tuân thủ nguyên lý Dependency Inversion:

```text
├── BeWoodProductEcom.sln
│
├── Domain/              # Core Layer: Entities, Enums, Interface repositories
│
├── Application/         # Core Layer: CQRS (Commands/Queries), DTOs, Interfaces, FluentValidation
│
├── Infrastructure/      # External Layer: EF Core DbContext, Migrations, Repositories, Services (PayOS, Cloudinary, Chatbot AI, SignalR)
│
└── API/                 # Presentation Layer: Controllers, Middlewares, Program.cs, AppSettings Configurations


