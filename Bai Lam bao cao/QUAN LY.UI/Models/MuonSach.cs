using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    [Table("MuonSach")]
    public  class MuonSach
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaMuon { get; set; }
        public int MaKhachHang { get; set; }
     
        public int Soluong   { get; set; }
        public string GhiChu { get; set; }
        public DateTime? NgayYeuCau { get; set; }

        public string TrangThai { get; set; }
        // Navigation
        public KhachHang KhachHang { get; set; }
    
        public ICollection<Giohang> Giohangs { get; set; }
        public ICollection<ChiTietMuonSach> ChiTietMuonSaches { get; set; }
    }
}
