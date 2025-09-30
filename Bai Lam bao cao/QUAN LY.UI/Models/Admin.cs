using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Models
{
    [Table("Admin")]
    public class Admin
    {
        public int MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string Chucvu { get; set; }
        public string Tendangnhap { get; set; }
        public string Matkhau { get; set; }




        // Admin có thể cho nhiều khách mượn
        public ICollection<MuonSach> MuonSaches { get; set; }
    
 
    }
}

