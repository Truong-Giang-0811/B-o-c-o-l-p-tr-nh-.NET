using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    [Table("HiddenHistories")]
    public class HiddenHistories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MaKhachHang { get; set; }
        public int MaChiTietMuon { get; set; }
        public KhachHang KhachHang { get; set; }
        public ChiTietMuonSach ChiTietMuonSach { get; set; }
    }
}
