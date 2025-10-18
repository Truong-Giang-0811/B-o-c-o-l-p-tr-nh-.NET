using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaKhachHang { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string Tendangnhap { get; set; }
        public string Matkhau { get; set; }
        public bool GioiTinh { get; set; }

        public ICollection<Giohang> Giohangs { get; set; }

        // Khách hàng có nhiều phiếu mượn
        public ICollection<MuonSach> MuonSaches { get; set; }
    }
}
