using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;

namespace QUAN_LY.UI.Services
{
    public class Xac_Thuc_Dangnhap
    {
        private readonly LibraryContext xacthuc;
        public Xac_Thuc_Dangnhap(LibraryContext context)
        {
            xacthuc = context;
        }
        // Đăng nhập cho Khách hàng
        public KhachHang DangNhap(string tenDangNhap, string matKhau)
        {
            // Tìm tài khoản khớp username + password
            var taiKhoan = xacthuc.KhachHangs
                                      .FirstOrDefault(t => t.Tendangnhap == tenDangNhap
                                                        && t.Matkhau == matKhau);

            return taiKhoan; // Trả về tài khoản (null nếu sai)
        }
        // Đăng nhập cho Admin và Nhân viên
        public Admin DangNhap2(string tenDangNhap, string matKhau)
        {
            // Tìm tài khoản khớp username + password
            var taiKhoan = xacthuc.Admins
                                      .FirstOrDefault(t => t.Tendangnhap == tenDangNhap
                                                        && t.Matkhau == matKhau);

            return taiKhoan; // Trả về tài khoản (null nếu sai)
        }
        // Kiểm tra tài khoản có phải Admin không
        public bool LaAdmin(Admin taiKhoan)
        {
            return taiKhoan != null && taiKhoan.Chucvu == "Admin";
        }

        // Kiểm tra tài khoản có phải Nhân viên không
        public bool LaNhanVien(Admin taiKhoan)
        {
            return taiKhoan != null && taiKhoan.Chucvu == "nhanvien";
        }

        // Kiểm tra tài khoản có phải Khách hàng không
        public bool LaKhachHang(KhachHang taiKhoan)
        {
            return taiKhoan != null;

        }

    }
}
