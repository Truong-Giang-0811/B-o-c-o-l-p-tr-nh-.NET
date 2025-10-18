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
            int makh = UserSession.CurrentKhachHang.MaKhachHang;
            var giohang = db.Giohangs.Include(g => g.Sach)
                                    .Where(g => g.MaKhachHang == makh)
                                    .Select(g => new Models.GioHangViewModel
                              {
                                 MaGioHang =g.MaGioHang,
                                 MaSach = g.MaSach,

                                 TieuDe = g.Sach.TieuDe,
                                 TacGia = g.Sach.TacGia,
                                 TheLoai = g.Sach.TheLoai,
                                 NhaXuatBan = g.Sach.NhaXuatBan,
                                 SoLuongmuon = g.SoLuongmuon
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
                            MessageBox.Show($"Số lượng mượn vượt quá số lượng tồn ({sach.SoLuongTon})!",
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

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
