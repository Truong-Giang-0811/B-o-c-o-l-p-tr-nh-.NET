using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;
using BCrypt.Net;

namespace QUAN_LY.UI.Services
{
    public class DichvuDangKy
    {
        private readonly LibraryContext _context;

        public DichvuDangKy(LibraryContext context)
        {
            _context = context;
        }

        // ✅ Kiểm tra độ mạnh mật khẩu
        private bool KiemTraDoManhMatKhau(string password)
        {
            if (password.Length < 8) return false; // Ít nhất 8 ký tự
            if (!password.Any(char.IsUpper)) return false; // Có chữ hoa
            if (!password.Any(char.IsLower)) return false; // Có chữ thường
            if (!password.Any(char.IsDigit)) return false; // Có số
            if (!password.Any(ch => !char.IsLetterOrDigit(ch))) return false; // Có ký tự đặc biệt
            return true;
        }

        // ✅ Hàm đăng ký (đã tích hợp kiểm tra và mã hóa BCrypt)
        public bool DangKy(
            string username,
            string password,
            string rePassword,
            string hoTen,
            DateTime? ngaySinh,
            string diaChi,
            string email,
            string soDienThoai,
            bool gioiTinh,
            out string message)
        {
            message = "";

            // Kiểm tra thông tin trống
            if (string.IsNullOrWhiteSpace(username) ||
                !ngaySinh.HasValue ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(soDienThoai) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(hoTen))
            {
                message = "Vui lòng nhập đầy đủ thông tin bắt buộc!";
                return false;
            }

            // Kiểm tra độ mạnh mật khẩu
            if (!KiemTraDoManhMatKhau(password))
            {
                message = "Mật khẩu phải có ít nhất 8 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt!";
                return false;
            }

            // Kiểm tra khớp mật khẩu
            if (password != rePassword)
            {
                message = "Mật khẩu nhập lại không khớp!";
                return false;
            }

            // Kiểm tra username đã tồn tại chưa
            var existingUser = _context.KhachHangs.FirstOrDefault(u => u.Tendangnhap == username);
            if (existingUser != null)
            {
                message = "Tên đăng nhập đã tồn tại!";
                return false;
            }

            // ✅ Mã hoá mật khẩu bằng BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Tạo tài khoản mới
            var newUser = new KhachHang
            {
                Tendangnhap = username,
                Matkhau = passwordHash,
                HoTen = hoTen,
                NgaySinh = ngaySinh,
                DiaChi = diaChi,
                Email = email,
                SoDienThoai = soDienThoai,
                GioiTinh = gioiTinh
            };

            _context.KhachHangs.Add(newUser);
            _context.SaveChanges();

            message = "Đăng ký thành công!";
            return true;
        }
    }
}
