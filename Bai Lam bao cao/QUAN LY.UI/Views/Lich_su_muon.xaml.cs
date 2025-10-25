using Microsoft.EntityFrameworkCore;
using QUAN_LY.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Lich_su_muon.xaml
    /// </summary>
    public partial class Lich_su_muon : UserControl
    {
        private readonly Data.LibraryContext db;
        public Lich_su_muon()
        {
            db = new Data.LibraryContext();
            InitializeComponent();
            LoadData();
            
        }
        private void LoadData()
        {
            int makh = UserSession.CurrentKhachHang.MaKhachHang;
            var lichsu = db.ChiTietMuonSaches
                .Include(a => a.MuonSach)
                .Include(a => a.MuonSach.KhachHang)
                .Where(ms => ms.MuonSach.MaKhachHang == makh &&
                     !db.HiddenHistories.Any(h => h.MaKhachHang == makh && h.MaChiTietMuon == ms.MaChiTietMuon))
                .Select(ms => new Models.Lichsumuonsach
                {
                    MaChiTietMuon = ms.MaChiTietMuon,
                    TieuDe = ms.Sach.TieuDe,
                    TheLoai = ms.Sach.TheLoai,
                    NgayMuon = ms.NgayMuon,
                    HanTra = ms.HanTra,
                    NgayTra = ms.NgayTra,
                    TrangThai = ms.TrangThai
                })
                .ToList();
            dgLichSuMuon.ItemsSource = lichsu;

        }

        private void dgLichSuMuon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnXoaLichSu_Click(object sender, RoutedEventArgs e)
        {
            var danhSachHienThi = dgLichSuMuon.ItemsSource as List<Models.Lichsumuonsach>;
            if (danhSachHienThi == null || !danhSachHienThi.Any())
            {
                MessageBox.Show("Lịch sử mượn trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var danhSachChon = danhSachHienThi.Where(g => g.IsChecked).ToList();
            if (danhSachChon.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sách để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int makh = UserSession.CurrentKhachHang.MaKhachHang;

            foreach (var item in danhSachChon)
            {
                var hidden = new Models.HiddenHistories
                {
                    MaKhachHang = makh,
                    MaChiTietMuon = item.MaChiTietMuon
                };
                db.HiddenHistories.Add(hidden);
            }

            db.SaveChanges();

            MessageBox.Show("Đã xóa các lịch sử được chọn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            LoadData(); // Cập nhật lại danh sách sau khi xóa
        }
    }
}
