using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    [Table("ChiTietMuonSach")]
    public class ChiTietMuonSach
    {
        public int MaChiTietMuon { get; set; }
        public int MaMuon { get; set; }
        public int MaSach { get; set; }
        public int SoLuong { get; set; }
        public string GhiChu { get; set; }

        // Navigation
        public MuonSach MuonSach { get; set; }
        public Sach Sach { get; set; }
    }
}
