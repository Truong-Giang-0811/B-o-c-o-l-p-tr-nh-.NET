using Microsoft.EntityFrameworkCore;
using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;
using QUAN_LY.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
                .Where(g => g.MaKhachHang == makh)
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
            // 1. Lấy danh sách sách đã chọn trong Giỏ mượn
            var danhSachHienThi = dgGioMuon.ItemsSource as List<Models.GioHangViewModel>;
            if (danhSachHienThi == null || !danhSachHienThi.Any())
            {
                MessageBox.Show("Giỏ mượn trống. Vui lòng thêm sách trước khi xác nhận!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lọc những sách được chọn (checkbox)
            // LƯU Ý: Đảm bảo class GioHangViewModel có thuộc tính 'IsChecked'
            var danhSachChon = danhSachHienThi.Where(g => g.IsChecked).ToList();
            if (danhSachChon.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sách để xác nhận mượn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int maKhachHangHienTai = UserSession.CurrentKhachHang.MaKhachHang;
           
            int tongSoLuongMuon = danhSachChon.Sum(g => g.SoLuongmuon);
            

            // Thiết lập Transaction Scope để đảm bảo tính nhất quán dữ liệu
            using (var dbTransaction = db.Database.BeginTransaction())
            {
                try
                {
              
                    // BƯỚC 1: TẠO BẢN GHI MỚI TRONG BẢNG MuonSach (HEADER)
                   
                    var donMuonMoi = new MuonSach
                    {
                        MaKhachHang = maKhachHangHienTai,
                        Soluong = tongSoLuongMuon, 
                        NgayYeuCau = DateTime.Now,
                        TrangThai = "Đang chờ duyệt", 
                        GhiChu = "Yêu cầu mượn từ khách hàng",
                    };
                    db.MuonSaches.Add(donMuonMoi);
                    db.SaveChanges(); // Lệnh này giúp lấy được MaMuon vừa được sinh ra

                    int maMuonVuaTao = donMuonMoi.MaMuon;
                    var maGioHangCanXoa = new List<int>();

                    // =======================================================
                    // BƯỚC 2 & 3: TẠO CHI TIẾT ĐƠN MƯỢN & CẬP NHẬT TỒN KHO
                    // =======================================================
                    foreach (var item in danhSachChon)
                    {
                        // 3. Kiểm tra và Cập nhật tồn kho (Giảm SoLuongTon)
                        var sachCanCapNhat = db.Saches.FirstOrDefault(s => s.MaSach == item.MaSach);
                        if (sachCanCapNhat != null)
                        {
                            if (sachCanCapNhat.SoLuongTon < item.SoLuongmuon)
                            {
                                // Hủy giao dịch nếu không đủ sách tồn
                                throw new Exception($"Sách '{item.TieuDe}' chỉ còn {sachCanCapNhat.SoLuongTon} cuốn. Vui lòng cập nhật số lượng.");
                            }
                            sachCanCapNhat.SoLuongTon -= item.SoLuongmuon;
                        }

                        int thoihanmuon = sachCanCapNhat.Thoihanmuon;
                        // 2. Tạo bản ghi ChiTietMuonSach
                        var chiTiet = new ChiTietMuonSach
                        {
                            MaMuon = maMuonVuaTao, // Khóa ngoại trỏ về Đơn mượn vừa tạo
                            MaSach = item.MaSach,
                            SoLuong = item.SoLuongmuon,
                            TrangThai = "Đang chờ duyệt",
                            // NgayTra để NULL
                            GhiChu = ""
                        };
                        db.ChiTietMuonSaches.Add(chiTiet);

                        // Thêm MaGioHang vào danh sách cần xóa
                        maGioHangCanXoa.Add(item.MaGioHang);
                    }
                   

                    // =======================================================
                    // BƯỚC 4: XÓA DỮ LIỆU KHỎI BẢNG Giohang
                    // =======================================================
                    var gioHangCanXoa = db.Giohangs.Where(g => maGioHangCanXoa.Contains(g.MaGioHang)).ToList();
                    db.Giohangs.RemoveRange(gioHangCanXoa);
                    db.SaveChanges(); // Lưu thay đổi (xóa giỏ hàng)

                    // Hoàn tất giao dịch nếu tất cả các bước thành công
                    dbTransaction.Commit();

                    MessageBox.Show($"Đã tạo đơn mượn thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                   

                }
                catch (Exception ex)
                {
                    string innerExMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                    // Tìm lỗi chi tiết nhất từ Inner Exception nếu có nhiều lớp lỗi
                    var detailEx = ex;
                    while (detailEx.InnerException != null)
                    {
                        detailEx = detailEx.InnerException;
                    }
                    string finalMessage = detailEx.Message;


                    MessageBox.Show($"Lỗi trong quá trình tạo đơn mượn. Đơn hàng chưa được tạo. Chi tiết: {finalMessage}",
                                    "LỖI CƠ SỞ DỮ LIỆU", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadData();
            }
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
