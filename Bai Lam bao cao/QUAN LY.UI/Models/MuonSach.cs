using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    [Table("MuonSach")]
    public  class MuonSach
    {
        public int MaMuon { get; set; }
        public int MaKhachHang { get; set; }
        public int MaNhanVien { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime HanTra { get; set; }
        public DateTime? NgayTra { get; set; }
        public string TrangThai { get; set; }

        // Navigation
        public KhachHang KhachHang { get; set; }
        public Admin Admin { get; set; }
        public ICollection<ChiTietMuonSach> ChiTietMuonSaches { get; set; }
    }
}
