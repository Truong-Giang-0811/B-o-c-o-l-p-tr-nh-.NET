using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    [Table("Sach")]
    public  class Sach
    {
        public int MaSach { get; set; }
        public string TieuDe { get; set; }
        public string TacGia { get; set; }
        public string NhaXuatBan { get; set; }
        public int? NamXuatBan { get; set; }
        public string TheLoai { get; set; }
        public int SoLuong { get; set; }

        // Một sách có thể nằm trong nhiều chi tiết mượn
        public ICollection<ChiTietMuonSach> ChiTietMuonSaches { get; set; }
    }
}
