using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;
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
using Microsoft.EntityFrameworkCore;


namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Gio_muon.xaml
    /// </summary>
    public partial class Gio_muon : UserControl
    {
            private readonly LibraryContext db;


        public Gio_muon()
        {
            InitializeComponent();
            db = new Data.LibraryContext();
            LoadData();
        }
        private void LoadData()
        {
            int makh = UserSession.CurrentKhachHang.MaKhachHang;

            var giohang = db.Giohangs
                .Include(g => g.Sach)
                .Where(g => g.MaKhachHang == makh && g.TrangThai == "Đang chọn")
                .Select(g => new Models.GioHangViewModel
                {
                    MaGioHang = g.MaGioHang,
                    MaSach = g.MaSach,
                    TieuDe = g.Sach.TieuDe,
                    TacGia = g.Sach.TacGia,
                    TheLoai = g.Sach.TheLoai,
                    NhaXuatBan = g.Sach.NhaXuatBan,
                    SoLuongmuon = g.SoLuongmuon,
                    
                })
                .ToList();

            dgGioMuon.ItemsSource = giohang;
        }

        private void dgGioMuon_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Chỉ cho nhập ký tự số
            e.Handled = !char.IsDigit(e.Text, 0);
        }
        private void dgGioMuon_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "SL mượn")
            {
                // Lấy TextBox trong ô đang chỉnh
                var textBox = e.EditingElement as TextBox;
                if (textBox != null)
                {
                    if (!int.TryParse(textBox.Text, out int value) || value < 1)
                    {
                        MessageBox.Show("Số lượng mượn phải là số tự nhiên lớn hơn 0!",
                                        "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                        textBox.Text = "1"; // Reset về 1 nếu nhập sai
                    }
                    var giohangItem = e.Row.Item as Models.GioHangViewModel;
                    if (giohangItem == null) return;
                    var sach = db.Saches.FirstOrDefault(s => s.MaSach == giohangItem.MaSach);
                    if (sach != null)
                    {
                        if (value > sach.SoLuongTon)
                        {
                            MessageBox.Show($"Số lượng mượn vượt quá số lượng tồn!",
                                            "Không hợp lệ", MessageBoxButton.OK, MessageBoxImage.Warning);

                            // Giới hạn lại bằng số lượng tồn
                            textBox.Text = sach.SoLuongTon.ToString();
                        }
                    }
                    var giohangDb = db.Giohangs.FirstOrDefault(g => g.MaGioHang == giohangItem.MaGioHang);
                    if (giohangDb != null)
                    {
                        giohangDb.SoLuongmuon = value;
                        db.SaveChanges(); // 💾 Lưu thay đổi vào DB

                        // Cập nhật lại giao diện
                        giohangItem.SoLuongmuon = value;
                    }
                }
               
            }
        }

        private void dgGioMuon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Xacnhan_Click(object sender, RoutedEventArgs e)
        {
            // Lấy danh sách hiển thị trên DataGrid
            var danhSachHienThi = dgGioMuon.ItemsSource as List<Models.GioHangViewModel>;
            if (danhSachHienThi == null)
            {
                MessageBox.Show("Không có dữ liệu trong giỏ mượn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lọc những sách được chọn (checkbox)
            var danhSachChon = danhSachHienThi.Where(g => g.IsChecked).ToList();
            if (danhSachChon.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sách để xác nhận mượn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lặp qua từng sách được chọn để tạo yêu cầu mượn
            foreach (var item in danhSachChon)
            {
                // Tìm lại Giohang trong DB dựa trên MaGioHang
                var gioHangDb = db.Giohangs.FirstOrDefault(g => g.MaGioHang == item.MaGioHang);
                if (gioHangDb == null) continue; // nếu không có thì bỏ qua

                // Kiểm tra xem đã có yêu cầu mượn cho giỏ này chưa (tránh trùng)
                var tonTai = db.Yeucaumuons.Any(y => y.MaGioHang == gioHangDb.MaGioHang);
                if (tonTai)
                    continue;

                // Tạo đối tượng Yeucaumuon mới
                var yeuCau = new Yeucaumuon
                {
                    MaGioHang = gioHangDb.MaGioHang,
                    SoLuong = gioHangDb.SoLuongmuon,
                    NgayTaoDon = DateTime.Now,
                    Trangthai = "Đang chờ duyệt"

                };

                // Thêm vào DB context
                db.Yeucaumuons.Add(yeuCau);
                gioHangDb.TrangThai = "Đã gửi yêu cầu";
            }

            // Lưu toàn bộ thay đổi vào DB
            db.SaveChanges();
            LoadData(); // Tải lại dữ liệu trên giao diện

            MessageBox.Show($"Đã gửi {danhSachChon.Count} yêu cầu mượn thành công!");

        }

        private void XoaGiohang_Click(object sender, RoutedEventArgs e)
        {
            var danhSachHienThi = dgGioMuon.ItemsSource as List<Models.GioHangViewModel>;
            if (danhSachHienThi == null)
            {
                MessageBox.Show("Không có dữ liệu trong giỏ mượn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var danhSachChon = danhSachHienThi.Where(g => g.IsChecked).ToList();
            if (danhSachChon.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sách để xác nhận mượn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show($"Bạn có chắc muốn xóa {danhSachChon.Count} sách khỏi giỏ mượn không?",
                                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            // Xóa trong database
            foreach (var item in danhSachChon)
            {
                var gioHangDb = db.Giohangs.FirstOrDefault(g => g.MaGioHang == item.MaGioHang);
                if (gioHangDb != null)
                {
                    db.Giohangs.Remove(gioHangDb);
                }
            }

            // Lưu thay đổi
            db.SaveChanges();

            // Tải lại dữ liệu để cập nhật giao diện
            LoadData();

            MessageBox.Show("Đã xóa thành công các sách được chọn khỏi giỏ mượn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
