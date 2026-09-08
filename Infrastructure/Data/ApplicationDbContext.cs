using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình chuyển đổi Enum sang dạng chuỗi (String) trong Postgres
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion<string>();

            modelBuilder.Entity<News>()
                .Property(n => n.Status)
                .HasConversion<string>();

            // 2. Cấu hình tự động sinh Guid cho các khóa chính bằng hàm gen_random_uuid() của PostgreSQL
            modelBuilder.Entity<User>().Property(e => e.UserId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Category>().Property(e => e.CategoryId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Product>().Property(e => e.ProductId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<ProductImage>().Property(e => e.ProductImageId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Order>().Property(e => e.OrderId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<OrderDetail>().Property(e => e.OrderDetailId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Review>().Property(e => e.ReviewId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<News>().Property(e => e.NewsId).HasDefaultValueSql("gen_random_uuid()");

            // 3. Thiết lập các mối quan hệ và quy tắc xóa (Relationships & Cascade Rules)

            // Category: Self-referencing (Danh mục cha - con)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // News -> User (Author)
            modelBuilder.Entity<News>()
                .HasOne(n => n.Author)
                .WithMany(u => u.NewsList)
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product -> Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductImage -> Product
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa sản phẩm thì xóa luôn ảnh phụ

            // Order -> User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderDetail -> Order & Product
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa đơn hàng thì xóa chi tiết đơn hàng

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review -> Product & User
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình Cart (1 - 1 với User)
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.CartId);

                entity.HasOne(c => c.User)
                      .WithOne(u => u.Cart)
                      .HasForeignKey<Cart>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình CartItem (N - 1 với Cart & Product)
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.CartItemId);

                entity.HasOne(ci => ci.Cart)
                      .WithMany(c => c.CartItems)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Product)
                      .WithMany(p => p.CartItems)
                      .HasForeignKey(ci => ci.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
