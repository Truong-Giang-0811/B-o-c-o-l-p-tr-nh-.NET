using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Services
{
    class Quan_Ly_Nhanvien
    {
        private readonly LibraryContext _context;
        public Quan_Ly_Nhanvien(LibraryContext context)
        {
            _context = context;
        }
        public bool ThemNhanVien(
                    string username,
                    string password,
                    string rePassword,
                    string hoTen,
                    DateTime? ngaySinh,
                    string diaChi,
                    string email,
                    string soDienThoai,
                    string gioiTinh,
                    out string message)
        {
            message = "";

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(rePassword) ||
                string.IsNullOrWhiteSpace(hoTen) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(soDienThoai) ||
                string.IsNullOrWhiteSpace(diaChi) ||
                !ngaySinh.HasValue)
            {
                message = "Vui lòng nhập đầy đủ thông tin bắt buộc!";
                return false;
            }


            // Kiểm tra mật khẩu nhập lại
            if (password != rePassword)
            {
                message = "Mật khẩu nhập lại không khớp!";
                return false;
            }

            var tuoi = DateTime.Today.Year - ngaySinh.Value.Year;
            if (ngaySinh.Value.Date > DateTime.Today.AddYears(-tuoi)) tuoi--; // chưa tới sinh nhật trong năm nay
            if (tuoi < 18)
            {
                message = "Nhân viên phải đủ 18 tuổi trở lên!";
                return false;
            }
            // Kiểm tra username trùng
            var existingUser = _context.Admins.FirstOrDefault(a => a.Tendangnhap == username);
            if (existingUser != null)
            {
                message = "Tên đăng nhập đã tồn tại!";
                return false;
            }

            // Mã hóa mật khẩu
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            
            var admin = new Admin
            {
                Tendangnhap = username,
                Matkhau = passwordHash,
                HoTen = hoTen,
                NgaySinh = ngaySinh,
                DiaChi = diaChi,
                Email = email,
                SoDienThoai = soDienThoai,
                GioiTinh = gioiTinh,
                Chucvu = "Nhân viên"
            };

            _context.Admins.Add(admin);
            _context.SaveChanges();

            message = "Thêm nhân viên thành công!";
            return true;
        }
        public bool XoaNhanVien(int maNhanVien, out string message)
        {
            message = "";

            var admin = _context.Admins.FirstOrDefault(a => a.MaNhanVien == maNhanVien);
            if (admin == null)
            {
                message = "Không tìm thấy nhân viên!";
                return false;
            }

            _context.Admins.Remove(admin);
            _context.SaveChanges();

            message = "Xóa nhân viên thành công!";
            return true;
        }
        public bool CapNhatNhanVien(
                    int maNhanVien,
                    string username,
                    string password,
                    string rePassword,
                    
                    string hoTen,
                    DateTime? ngaySinh,
                    string diaChi,
                    string email,
                    string soDienThoai,
                    string gioiTinh,
                    out string message)
        {
            message = "";

            var admin = _context.Admins.FirstOrDefault(a => a.MaNhanVien == maNhanVien);
            if (admin == null)
            {
                message = "Không tìm thấy nhân viên cần cập nhật!";
                return false;
            }

            // Cập nhật thông tin
            admin.HoTen = hoTen;
            admin.NgaySinh = ngaySinh;
            admin.DiaChi = diaChi;
            admin.Email = email;
            admin.SoDienThoai = soDienThoai;
            admin.GioiTinh = gioiTinh;
           

            // Nếu có đổi mật khẩu mới
            if (!string.IsNullOrWhiteSpace(password))
            {
                admin.Matkhau = BCrypt.Net.BCrypt.HashPassword(password);
            }

            _context.Admins.Update(admin);
            _context.SaveChanges();

            message = "Cập nhật thông tin nhân viên thành công!";
            return true;
        }
    }
}
