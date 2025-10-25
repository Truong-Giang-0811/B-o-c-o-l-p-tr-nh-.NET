using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    public class GioHangViewModel
    {
        public bool IsChecked { get; set; }
        public int MaGioHang { get; set; }
        public int MaSach { get; set; }
        public string TieuDe { get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public string NhaXuatBan { get; set; }
        public int SoLuongmuon { get; set; }
    }

    public class DonmuonViewModel
    {
        public int MaDonMuon { get; set; }
        public string TenKhachHang { get; set; }
        public int SoLuong { get; set; }
        public DateTime? NgayYeuCau { get; set; }
        public string TrangThai { get; set; }
       
    }
    public class  Lichsumuonsach 
    {
        public int MaChiTietMuon { get; set; }
        public string TieuDe { get; set; }
        public string TheLoai { get; set; }
        public DateTime? NgayMuon { get; set; }
        public DateTime? NgayTra { get; set; }
        public DateTime? HanTra { get; set; }
        public string TrangThai { get; set; }
        public bool IsChecked { get; set; }

    }
}
