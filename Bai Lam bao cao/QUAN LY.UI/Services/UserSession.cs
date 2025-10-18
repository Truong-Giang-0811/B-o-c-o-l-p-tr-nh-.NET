using QUAN_LY.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUAN_LY.UI.Services
{
    public static class UserSession
    {
        public static KhachHang CurrentKhachHang { get; private set; }
        public static Admin CurrentAdmin { get; private set; }

        public static void SetKhachHang(KhachHang kh)
        {
            CurrentKhachHang = kh;
            CurrentAdmin = null;
        }

        public static void SetAdmin(Admin ad)
        {
            CurrentAdmin = ad;
            CurrentKhachHang = null;
        }

        public static void Clear()
        {
            CurrentKhachHang = null;
            CurrentAdmin = null;
        }
    }
}
