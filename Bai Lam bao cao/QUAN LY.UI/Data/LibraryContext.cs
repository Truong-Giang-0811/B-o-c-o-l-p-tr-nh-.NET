using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QUAN_LY.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Data
{
    public class LibraryContext : DbContext
    {
        // Constructor mặc định để WPF có thể gọi new LibraryContext()
        public LibraryContext() { }
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    
        public DbSet<Admin> Admins { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<Sach> Saches { get; set; }
        public DbSet<MuonSach> MuonSaches { get; set; }
        public DbSet<ChiTietMuonSach> ChiTietMuonSaches { get; set; }
        public DbSet<Giohang> Giohangs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ================== Khai báo khóa chính ==================
         

            modelBuilder.Entity<Admin>()
                .HasKey(a => a.MaNhanVien);

            modelBuilder.Entity<KhachHang>()
                .HasKey(k => k.MaKhachHang);

            modelBuilder.Entity<Sach>()
                .HasKey(s => s.MaSach);

            modelBuilder.Entity<MuonSach>()
                .HasKey(m => m.MaMuon);

            modelBuilder.Entity<ChiTietMuonSach>()
                .HasKey(c => c.MaChiTietMuon);

            modelBuilder.Entity<Giohang>()
                .HasKey(g => g.MaGioHang);

            // ================== Khai báo quan hệ ==================
            // KhachHang - TaiKhoan (1-1)


            // Admin - TaiKhoan (1-1)


            // MuonSach - KhachHang (n-1)
            modelBuilder.Entity<MuonSach>()
                .HasOne(m => m.KhachHang)
                .WithMany(k => k.MuonSaches)
                .HasForeignKey(m => m.MaKhachHang);

            modelBuilder.Entity<MuonSach>()
              .HasOne(m => m.Yeucaumuon)
              .WithMany(k => k.MuonSaches)
              .HasForeignKey(m => m.MaYeuCau);

            // MuonSach - Admin (n-1)
            modelBuilder.Entity<MuonSach>()
                .HasOne(m => m.Admin)
                .WithMany(a => a.MuonSaches)
                .HasForeignKey(m => m.MaNhanVien);

            // ChiTietMuonSach - MuonSach (n-1)
            modelBuilder.Entity<ChiTietMuonSach>()
                .HasOne(c => c.MuonSach)
                .WithMany(m => m.ChiTietMuonSaches)
                .HasForeignKey(c => c.MaMuon);

            // ChiTietMuonSach - Sach (n-1)
            modelBuilder.Entity<ChiTietMuonSach>()
                .HasOne(c => c.Sach)
                .WithMany(s => s.ChiTietMuonSaches)
                .HasForeignKey(c => c.MaSach);
            modelBuilder.Entity<Giohang>()
                .HasOne(g => g.KhachHang)
                .WithMany(k => k.Giohangs)
                .HasForeignKey(g => g.MaKhachHang);

            modelBuilder.Entity<Giohang>()
                .HasOne(g => g.Sach)
                .WithMany(s => s.Giohangs)
                .HasForeignKey(g => g.MaSach);

        }

        // Cấu hình kết nối đến SQL Server
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // chỉ chạy nếu chưa có config
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // lấy path hiện tại
                    .AddJsonFile("appsettings.json")              // load appsettings.json
                    .Build();

                var connectionString = configuration.GetConnectionString("LibraryDb");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
