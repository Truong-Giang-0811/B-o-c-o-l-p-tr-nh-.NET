using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docgia
{
    public class Sach
    {
        
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public string NXB { get; set; }
        public int SLTon { get; set; }
        public string MoTa { get; set; }
        public int SLMuon { get; set; } = 1;
        public bool IsChecked { get; set; } = false;
    }
}
