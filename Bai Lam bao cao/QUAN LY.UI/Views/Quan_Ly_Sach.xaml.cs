using Microsoft.EntityFrameworkCore;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Quan_Ly_Sach.xaml
    /// </summary>
    public partial class Quan_Ly_Sach : UserControl
    {
        private readonly Quan_ly_Sach Qlsach;
        private Sach selectedSach;
        private readonly LibraryContext _context;
        private List<Sach> danhSachGoc;
        public Quan_Ly_Sach()
        {
            InitializeComponent();
            var db = new LibraryContext();
            Qlsach = new Quan_ly_Sach(db);
            _context = new LibraryContext();
            dgSach.ItemsSource = db.Saches.ToList();
            dpNgayNhap.SelectedDate = DateTime.Now.Date;
            LoadDanhSachSach();

        }
        private void LoadDanhSachSach()
        {
            danhSachGoc = _context.Saches.ToList();
            dgSach.ItemsSource = danhSachGoc;
        }
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T found)
                    return found;
                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void HighlightMatchingText(string keyword)
        {
            foreach (var item in dgSach.Items)
            {
                if (item is not Sach sach) continue;

                var row = dgSach.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row == null) continue;

                var presenter = FindVisualChild<DataGridCellsPresenter>(row);
                if (presenter == null) continue;

                // Tìm cột Tên sách (index 2)
                var cell = presenter.ItemContainerGenerator.ContainerFromIndex(2) as DataGridCell;
                if (cell == null) continue;

                var txtBlock = FindVisualChild<TextBlock>(cell);
                if (txtBlock == null) continue;

                string text = sach.TieuDe ?? "";
                txtBlock.Inlines.Clear();

                if (string.IsNullOrEmpty(keyword))
                {
                    txtBlock.Inlines.Add(new Run(text));
                    continue;
                }

                int startIndex = 0;
                string lowerText = text.ToLower();
                string lowerKey = keyword.ToLower();

                while (true)
                {
                    int index = lowerText.IndexOf(lowerKey, startIndex);
                    if (index == -1)
                    {
                        txtBlock.Inlines.Add(new Run(text.Substring(startIndex)));
                        break;
                    }

                    // phần trước
                    txtBlock.Inlines.Add(new Run(text.Substring(startIndex, index - startIndex)));
                    // phần khớp -> đỏ + đậm
                    txtBlock.Inlines.Add(new Run(text.Substring(index, keyword.Length))
                    {
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Red
                    });

                    startIndex = index + keyword.Length;
                }
            }
        }


        private void txtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();

            txtPlaceholder.Visibility = string.IsNullOrEmpty(keyword)
                ? Visibility.Visible
                : Visibility.Collapsed;

            if (string.IsNullOrEmpty(keyword))
            {
                dgSach.ItemsSource = danhSachGoc;
            }
            else
            {
                dgSach.ItemsSource = danhSachGoc
                    .Where(s => s.TieuDe.ToLower().Contains(keyword) ||
                                s.TacGia.ToLower().Contains(keyword))
                    .ToList();
            }

            dgSach.Items.Refresh();

            dgSach.Dispatcher.InvokeAsync(() => HighlightMatchingText(keyword));
        }

        private void Resetform()
        {
            txtTieude.Clear();
            txtTacGia.Clear();
            txtThoihanmuon.Value = 0;
            txtNXB.Clear();
            txtTheLoai.Clear();
            txtMota.Clear();
            txtSLTon.Value = 0;
            txtSLMuon.Value = 0;
            
        }
        private void btn_Themsach_Click(object sender, RoutedEventArgs e)
        {
            string tieuDe = txtTieude.Text.Trim();
            string tacGia = txtTacGia.Text.Trim();
            int thoihanmuon = txtThoihanmuon.Value ?? 3;
            string nhaXuatBan = txtNXB.Text.Trim();
            string theLoai = txtTheLoai.Text.Trim();
            int soLuongTon = txtSLTon.Value ?? 0;
            int soLuongMuon = txtSLMuon.Value ?? 0;
            string mota = txtMota.Text.Trim();
            DateTime? ngayNhap = dpNgayNhap.SelectedDate;

            // Gọi dịch vụ để thêm sách
            if (Qlsach.ThemSach(tieuDe, tacGia, nhaXuatBan, theLoai, thoihanmuon, soLuongTon, soLuongMuon, ngayNhap, mota, out string message))
            {
                MessageBox.Show(message, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Cập nhật lại DataGrid
                dgSach.ItemsSource = null; // reset cũ
                using (var db = new LibraryContext())
                {
                    dgSach.ItemsSource = db.Saches.ToList();
                }
                // Xoá trắng các trường sau khi thêm thành công
                Resetform();
                dpNgayNhap.SelectedDate = DateTime.Now.Date;
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void dgSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSach.SelectedItem is Sach selectedBook)
            {
                // Hiển thị thông tin chi tiết của sách đã chọn
                selectedSach = selectedBook;
                txtTieude.Text = selectedBook.TieuDe;
                txtTacGia.Text = selectedBook.TacGia;
                txtNXB.Text = selectedBook.NhaXuatBan;
                txtTheLoai.Text = selectedBook.TheLoai;
                txtSLTon.Value = selectedBook.SoLuongTon;
                txtSLMuon.Value = selectedBook.SoLuongMuon;
                txtMota.Text = selectedBook.Mota;
                dpNgayNhap.SelectedDate = selectedBook.NgayNhap;
                txtThoihanmuon.Value = selectedBook.Thoihanmuon;
            }
        }

        private void Cap_Nhat_Click(object sender, RoutedEventArgs e)
        {
            var sachmoi = new Sach
            {
                MaSach = selectedSach.MaSach,
                TieuDe = txtTieude.Text.Trim(),
                TacGia = txtTacGia.Text.Trim(),
                NhaXuatBan = txtNXB.Text.Trim(),
                TheLoai = txtTheLoai.Text.Trim(),
                SoLuongTon = txtSLTon.Value ?? 0,
                SoLuongMuon = txtSLMuon.Value ?? 0,
                Mota = txtMota.Text.Trim(),
                NgayNhap = dpNgayNhap.SelectedDate,
                Thoihanmuon = txtThoihanmuon.Value ?? 1,
            };

            if (Qlsach.CapNhat(sachmoi, out string message))
            {
                MessageBox.Show(message, "Cập Nhật Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                // Cập nhật lại DataGrid
                dgSach.ItemsSource = null; // reset cũ
                using (var db = new LibraryContext())
                {
                    dgSach.ItemsSource = db.Saches.ToList();
                }
                Resetform();
                dpNgayNhap.SelectedDate = DateTime.Now.Date;
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Xoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgSach.SelectedItem is Sach sachDangChon)
            {
                // Hỏi xác nhận trước khi xóa
                var confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sách '{sachDangChon.TieuDe}' không?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    if (Qlsach.Xoa(sachDangChon.MaSach, out string message))
                    {
                        MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        dgSach.ItemsSource = null; // reset cũ
                        using (var db = new LibraryContext())
                        {
                            dgSach.ItemsSource = db.Saches.ToList();
                        }
                        Resetform();
                        dpNgayNhap.SelectedDate = DateTime.Now.Date;
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sách để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Resetform();
            dpNgayNhap.SelectedDate = DateTime.Now.Date;
        }
    }
}