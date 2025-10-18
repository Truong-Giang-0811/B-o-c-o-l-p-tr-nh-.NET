using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    [Table("Giohang")]
    public class Giohang
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaGioHang { get; set; }
        public int MaSach { get; set; }
        public int MaKhachHang { get; set; }
        public int SoLuongmuon { get; set; }

        public Sach Sach { get; set; }
        public KhachHang KhachHang { get; set; }

       public Yeucaumuon Yeucaumuon { get; set; }
    }
}
