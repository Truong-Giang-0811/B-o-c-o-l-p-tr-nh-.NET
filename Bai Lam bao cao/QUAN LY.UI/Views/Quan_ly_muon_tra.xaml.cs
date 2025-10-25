using Microsoft.EntityFrameworkCore;
using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;
using QUAN_LY.UI.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Quan_ly_muon_tra.xaml
    /// </summary>
    public partial class Quan_ly_muon_tra : UserControl
    {
        private readonly LibraryContext db;

        private ChiTietMuonSach selectedChiTiet;
        public Quan_ly_muon_tra()
        {
            InitializeComponent();
            db = new Data.LibraryContext();
            LoadDatamuonsach();
        }

        private void LoadDatamuonsach()
        {
            var donmuon = db.MuonSaches
                .Include(g => g.KhachHang)
                .Select(g => new DonmuonViewModel
                {
                    MaDonMuon = g.MaMuon,
                    TenKhachHang = g.KhachHang.HoTen,
                    SoLuong = g.Soluong,
                    NgayYeuCau = g.NgayYeuCau,
                    TrangThai = g.TrangThai
                })
                .ToList();

            dgmuontra.ItemsSource = donmuon;
        }
        private void LoadDatatrasach()
        {
            // Chỉ lấy các sách đang mượn
            var data = db.ChiTietMuonSaches
                .Include(ct => ct.Sach)
                .Include(ct => ct.MuonSach)
                    .ThenInclude(m => m.KhachHang)
                .Where(ct => ct.TrangThai == "Đang mượn")
                .Select(ct => new
                {
                    ct.MaChiTietMuon,
                    ct.MaSach,
                    TenSach = ct.Sach.TieuDe,
                    ct.SoLuong,
                    ct.NgayMuon,
                    ct.HanTra,
                    ct.TrangThai,
                    ct.MuonSach.KhachHang.MaKhachHang,
                    TenKhachHang = ct.MuonSach.KhachHang.HoTen
                })
                .ToList();

            dgTraSach.ItemsSource = data;
        }
        private void dgTraSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTraSach.SelectedItem == null) return;

            dynamic item = dgTraSach.SelectedItem;

            // Lấy dữ liệu từ dynamic ra biến thường
            int maSach = (int)item.MaSach;
            int maKhachHang = (int)item.MaKhachHang;

            // Hiển thị thông tin
            txtMaDocGiaTras.Text = maKhachHang.ToString();
            txtMaSachTras.Text = maSach.ToString();
            txtSoLuongTrs.Text = item.SoLuong.ToString();
            dpNgayMuonTrs.SelectedDate = item.NgayMuon;
            dpNgayHenTraTrs.SelectedDate = item.HanTra;
            dpNgayTraTrs.SelectedDate = DateTime.Now;

            // Dùng biến kiểu rõ ràng (không dùng dynamic trong LINQ)
            selectedChiTiet = db.ChiTietMuonSaches
                .Include(ct => ct.Sach)
                .Include(ct => ct.MuonSach)
                    .ThenInclude(m => m.KhachHang)
                .FirstOrDefault(ct =>
                    ct.Sach.MaSach == maSach &&
                    ct.MuonSach.KhachHang.MaKhachHang == maKhachHang);
        }
        private void BtnTraSach_Click(object sender, RoutedEventArgs e)
        {
            if (selectedChiTiet == null)
            {
                MessageBox.Show("Vui lòng chọn sách cần trả!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DateTime ngayTra = DateTime.Now;

                // Gán ngày trả đúng cú pháp (không dùng var trước thuộc tính)
                selectedChiTiet.NgayTra = ngayTra;

                // So sánh ngày trả và ngày hẹn trả
                if (selectedChiTiet.HanTra.HasValue && ngayTra > selectedChiTiet.HanTra.Value)
                {
                    selectedChiTiet.TrangThai = "Trễ hạn";
                }
                else
                {
                    selectedChiTiet.TrangThai = "Đã trả";
                }

                // Nếu tất cả sách trong đơn đã trả hoặc trễ hạn => cập nhật trạng thái đơn mượn
                var muonSach = db.MuonSaches
                    .Include(m => m.ChiTietMuonSaches)
                    .FirstOrDefault(m => m.MaMuon == selectedChiTiet.MaMuon);

                if (muonSach != null)
                {
                    bool tatCaDaTra = muonSach.ChiTietMuonSaches.All(ct =>
                        ct.TrangThai == "Đã trả" || ct.TrangThai == "Trễ hạn");

                    if (tatCaDaTra)
                        muonSach.TrangThai = "Hoàn tất";
                }

                // (Tùy chọn) - nếu bạn quản lý tồn kho sách, tăng lại số lượng trong bảng Sach ở đây
                // var sach = db.Saches.Find(selectedChiTiet.MaSach);
                // if (sach != null) { sach.SoLuong += selectedChiTiet.SoLuong; }

                db.SaveChanges();

                MessageBox.Show("Trả sách thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh cả 2 danh sách để đồng bộ giao diện
                LoadDatatrasach();
                LoadDatamuonsach();

                ClearForm();
                selectedChiTiet = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi trả sách: {ex.InnerException?.Message ?? ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ClearForm()
        {
            txtMaDocGiaTras.Clear();
            txtMaSachTras.Clear();
            txtSoLuongTrs.Clear();
            dpNgayMuonTrs.SelectedDate = null;
            dpNgayHenTraTrs.SelectedDate = null;
            dpNgayTraTrs.SelectedDate = null;

            // reset biến class-level
            selectedChiTiet = null;
        }
        private void BtnThoat(object sender, RoutedEventArgs e)
        {
            // Bỏ chọn dòng đang chọn trong DataGrid trả sách
            dgTraSach.SelectedItem = null;

            // Xóa hết nội dung các ô nhập phần trả sách
            txtMaDocGiaTras.Clear();
            txtMaSachTras.Clear();
            txtSoLuongTrs.Clear();
            dpNgayMuonTrs.SelectedDate = null;
            dpNgayHenTraTrs.SelectedDate = null;
            dpNgayTraTrs.SelectedDate = null;

            // Làm mới danh sách trả sách
            LoadDatatrasach();

            // Xóa biến đang giữ chi tiết đang chọn (nếu có)
            selectedChiTiet = null;
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgmuontra.SelectedItem is DonmuonViewModel selectedDonMuon)
            {
                var muonSach = db.MuonSaches
                    .Include(m => m.KhachHang)
                    .FirstOrDefault(m => m.MaMuon == selectedDonMuon.MaDonMuon);

                if (muonSach == null) return;

                // Hiển thị thông tin độc giả
                var khachHang = muonSach.KhachHang;
                txtmadocgia.Text = khachHang.MaKhachHang.ToString();
                txttendocgia.Text = khachHang.HoTen;
                txtdiachi.Text = khachHang.DiaChi;
                txtsodienthoai.Text = khachHang.SoDienThoai;

                // Lấy danh sách chi tiết mượn sách
                var chitietmuonsach = db.ChiTietMuonSaches
                    .Include(ct => ct.Sach)
                    .Where(ct => ct.MaMuon == muonSach.MaMuon)
                    .ToList();

                // Gán nguồn dữ liệu cho ComboBox
                cbotensach.ItemsSource = chitietmuonsach.Select(ct => new
                {
                    ChiTiet = ct,
                    TieuDe = ct.Sach.TieuDe
                }).ToList();

                cbotensach.DisplayMemberPath = "TieuDe";
                cbotensach.SelectedValuePath = "ChiTiet";

                // Chọn dòng đầu tiên nếu có dữ liệu
                if (chitietmuonsach.Any())
                {
                    cbotensach.SelectedIndex = 0;
                }
            }
        }

        private void cbotensach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbotensach.SelectedValue is ChiTietMuonSach selectedChiTiet)
            {
                var sach = selectedChiTiet.Sach;
                if (sach != null)
                {
                    txtTrangthai.Text = selectedChiTiet.TrangThai;
                    dpNgaymuon.SelectedDate = selectedChiTiet.NgayMuon;
                    dpNgayHenTra1.SelectedDate = selectedChiTiet.HanTra;
                    txtSoLuong.Text = selectedChiTiet.SoLuong.ToString();
                }
            }
        }

        private void DuyetSach_Click(object sender, RoutedEventArgs e)
        {
            if (dgmuontra.SelectedItem is not DonmuonViewModel selectedDonMuon)
            {
                MessageBox.Show("Vui lòng chọn đơn mượn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cbotensach.SelectedValue is not ChiTietMuonSach chiTiet)
            {
                MessageBox.Show("Vui lòng chọn sách để duyệt!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var muonSach = db.MuonSaches
                .Include(m => m.ChiTietMuonSaches)
                .ThenInclude(ct => ct.Sach)
                .FirstOrDefault(m => m.MaMuon == selectedDonMuon.MaDonMuon);

            if (muonSach == null)
            {
                MessageBox.Show("Không tìm thấy đơn mượn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Cập nhật chi tiết sách
            chiTiet.NgayMuon = DateTime.Now;
            chiTiet.HanTra = chiTiet.NgayMuon.Value.AddDays(chiTiet.Sach.Thoihanmuon);
            chiTiet.TrangThai = "Đang mượn";

            // Nếu tất cả chi tiết đã được duyệt hoặc từ chối
            bool tatCaDaDuyet = muonSach.ChiTietMuonSaches.All(ct => ct.TrangThai != "Đang chờ duyệt");
            if (tatCaDaDuyet)
            {
                muonSach.TrangThai = "Đã duyệt";
            }

            try
            {
                db.SaveChanges();
                MessageBox.Show("Cập nhật trạng thái thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                dpNgaymuon.SelectedDate = chiTiet.NgayMuon;
                dpNgayHenTra1.SelectedDate = chiTiet.HanTra;
                LoadDatamuonsach();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.InnerException?.Message ?? ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TuChoi_Click(object sender, RoutedEventArgs e)
        {
            if (dgmuontra.SelectedItem is not DonmuonViewModel selectedDonMuon)
            {
                MessageBox.Show("Vui lòng chọn đơn mượn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cbotensach.SelectedValue is not ChiTietMuonSach chiTiet)
            {
                MessageBox.Show("Vui lòng chọn sách để từ chối!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var muonSach = db.MuonSaches
                .Include(m => m.ChiTietMuonSaches)
                .FirstOrDefault(m => m.MaMuon == selectedDonMuon.MaDonMuon);

            if (muonSach == null)
            {
                MessageBox.Show("Không tìm thấy đơn mượn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            chiTiet.TrangThai = "Đã từ chối";

            bool khongConChoDuyet = muonSach.ChiTietMuonSaches.All(ct => ct.TrangThai != "Đang chờ duyệt");
            if (khongConChoDuyet)
            {
                muonSach.TrangThai = "Đã duyệt"; // hoặc "Hoàn tất"
            }

            db.SaveChanges();
            MessageBox.Show("Đã cập nhật trạng thái chi tiết!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadDatamuonsach();
        }
    }
}
