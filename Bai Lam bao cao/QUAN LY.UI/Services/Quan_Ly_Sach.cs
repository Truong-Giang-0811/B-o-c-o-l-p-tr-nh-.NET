using QUAN_LY.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUAN_LY.UI.Models;

namespace QUAN_LY.UI.Services
{
    class Quan_ly_Sach
    {
        private readonly LibraryContext _context;
        public Quan_ly_Sach(LibraryContext context)
        {
            _context = context;
        }
        public bool ThemSach(string tieuDe, string tacGia, string nhaXuatBan,
                             string theLoai,int thoihanmuon, int soLuongTon,int soluongmuon, DateTime? ngayNhap,string mota,
                             out string message)
        {
            message = "";
            if (string.IsNullOrWhiteSpace(tieuDe) ||
                string.IsNullOrWhiteSpace(tacGia) ||
                string.IsNullOrWhiteSpace(nhaXuatBan) ||
                string.IsNullOrWhiteSpace(theLoai) ||
                string.IsNullOrWhiteSpace(mota)||
                soLuongTon < 0 ||
                soluongmuon < 0 ||
                thoihanmuon <= 0 ||
                
                !ngayNhap.HasValue)
            {
                message = "Vui lòng nhập đầy đủ thông tin hợp lệ!";
                return false;
            }
            var existingBook = _context.Saches.FirstOrDefault(u => u.TieuDe == tieuDe);
            if (existingBook != null)
            {
                message = "Sach đã tồn tại!";
                return false;
            }
            if (ngayNhap.Value > DateTime.Now)
            {
                message = "Ngày nhập không được lớn hơn ngày hiện tại!";
                return false;
            }
            var newBook = new Sach
            {
                Thoihanmuon = thoihanmuon,
                TieuDe = tieuDe,
                TacGia = tacGia,
                NhaXuatBan = nhaXuatBan,
                TheLoai = theLoai,
                SoLuongTon = soLuongTon,
                SoLuongMuon = 0, // Mới thêm nên số lượng mượn là 0
                NgayNhap = ngayNhap,
                Mota = mota
            };
            _context.Saches.Add(newBook);
            _context.SaveChanges();
            message = "Thêm sách thành công!";
            return true;
        }
        public bool CapNhat(Sach Sachmoi, out string message)
        {
            message = "";

            if (Sachmoi == null ||
                string.IsNullOrWhiteSpace(Sachmoi.TieuDe) ||
                string.IsNullOrWhiteSpace(Sachmoi.TacGia) ||
                string.IsNullOrWhiteSpace(Sachmoi.NhaXuatBan) ||
                string.IsNullOrWhiteSpace(Sachmoi.TheLoai) ||
                string.IsNullOrWhiteSpace(Sachmoi.Mota)||
                Sachmoi.SoLuongTon < 0 ||
                Sachmoi.SoLuongMuon < 0 ||
                Sachmoi.Thoihanmuon <= 0 ||
                !Sachmoi.NgayNhap.HasValue)
            {
                message = "Vui lòng nhập đầy đủ thông tin hợp lệ!";
                return false;
            }
            if (Sachmoi.NgayNhap.Value > DateTime.Now)
            {
                message = "Ngày nhập không được lớn hơn ngày hiện tại!";
                return false;
            }
            var sach = _context.Saches.FirstOrDefault(s => s.MaSach == Sachmoi.MaSach);
            if (sach == null)
            {
                message = "Không tìm thấy sách để cập nhật!";
                return false;
            }
            // Cập nhật thông tin sách
            sach.TieuDe = Sachmoi.TieuDe;
            sach.TacGia = Sachmoi.TacGia;
            sach.NhaXuatBan = Sachmoi.NhaXuatBan;
            sach.TheLoai = Sachmoi.TheLoai;
            sach.SoLuongTon = Sachmoi.SoLuongTon;
            sach.SoLuongMuon = Sachmoi.SoLuongMuon;
            sach.NgayNhap = Sachmoi.NgayNhap;
            sach.Thoihanmuon = Sachmoi.Thoihanmuon;
            sach.Mota = Sachmoi.Mota;
            _context.SaveChanges();
            message = "Cập nhật sách thành công!";
            return true;

        }
        public bool Xoa(int masach, out string message)
        {
            message = "";
            var sach = _context.Saches.FirstOrDefault(s => s.MaSach == masach);
            if (sach == null)
            {
                message = "Không tìm thấy sách để xóa!";
                return false;
            }
            // Kiểm tra nếu sách đang được mượn thì không cho xóa
            if (sach.SoLuongMuon > 0)
            {
                message = "Không thể xóa sách đang được mượn!";
                return false;
            }
            _context.Saches.Remove(sach);
            _context.SaveChanges();
            message = "Xóa sách thành công!";
            return true;
        }
    }
}
