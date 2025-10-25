using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;


namespace QUAN_LY.UI.Services
{
    public class DichvuDangKy
    {
        private readonly LibraryContext _context;
        public DichvuDangKy(LibraryContext context)
        {
            _context = context;
        }

        public bool DangKy(string username, string password, string rePassword,
                                    string hoTen, DateTime? ngaySinh, string diaChi,
                                    string email, string soDienThoai, bool gioiTinh,
                                    out string message)
        {
            message = "";
            if (string.IsNullOrWhiteSpace(username) || 
                !ngaySinh.HasValue || 
                string.IsNullOrWhiteSpace(email) || 
                string.IsNullOrWhiteSpace(soDienThoai)||  
                string.IsNullOrWhiteSpace(password) || 
                string.IsNullOrWhiteSpace(hoTen))
            {
                message = "Vui lòng nhập đầy đủ thông tin bắt buộc!";
                return false;
            }
            if (password != rePassword)
            {
                message = "Mật khẩu nhập lại không khớp!";
                return false;
            }
            var existingUser = _context.KhachHangs.FirstOrDefault(u => u.Tendangnhap == username);
            if (existingUser != null)
            {
                message = "Tên đăng nhập đã tồn tại!";
                return false;
            }
            var newUser = new KhachHang
            {
                Tendangnhap = username,
                Matkhau = password, // thực tế nên mã hoá
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
