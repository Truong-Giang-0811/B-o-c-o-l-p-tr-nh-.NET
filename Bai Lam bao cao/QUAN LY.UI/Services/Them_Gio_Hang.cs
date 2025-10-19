using Microsoft.EntityFrameworkCore;
using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace QUAN_LY.UI.Services
{

    public class Them_Gio_Hang
    {
        private readonly LibraryContext _context;
        public Them_Gio_Hang(LibraryContext context)
        {
            _context = context;
        }
        public bool themvaogio(int masach) 
        {
            int makh = UserSession.CurrentKhachHang.MaKhachHang;

                var newItem = new Giohang
                {
                    MaKhachHang = makh,
                    MaSach = masach,
                    SoLuongmuon = 1,
                    TrangThai = "Đang chọn"
                };
                _context.Giohangs.Add(newItem);

            _context.SaveChanges();
            return true;
        }


    }
}
