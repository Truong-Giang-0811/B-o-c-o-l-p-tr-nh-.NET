using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    [Table("Yeucaumuon")]
    public class Yeucaumuon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaYeuCau { get; set; }
        public int MaGioHang { get; set; }
        public int SoLuong { get; set; }
        public DateTime? NgayTaoDon { get; set; }
        public String Trangthai { get; set; }

        public Giohang Giohang { get; set; }
        public MuonSach? MuonSach { get; set; } // có thể null (nếu chưa được duyệt)
    }
}
