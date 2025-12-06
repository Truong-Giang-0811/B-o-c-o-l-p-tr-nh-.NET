using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;
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

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Quan_ly_nguoi_dung.xaml
    /// </summary>
    public partial class Quan_ly_nguoi_dung : UserControl
    {
        private readonly LibraryContext _context;
        private List<KhachHang> danhSachGoc;
        public Quan_ly_nguoi_dung()
        {
            try
            {
                InitializeComponent();
                _context = new LibraryContext();
                LoadDanhSachDocGia();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo giao diện Quản lý người dùng:\n" + ex.Message);
            }
        }

        // ======= Load danh sách độc giả =======
        private void LoadDanhSachDocGia()
        {
            danhSachGoc = _context.KhachHangs
                        .Where(kh => kh.Ghichu != "Đã xóa")
                        .ToList();

            dgDocGia.ItemsSource = danhSachGoc;
            txtTimKiem_TextChanged(txtTimKiem, null);
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
            foreach (var item in dgDocGia.Items)
            {
                if (item is not KhachHang kh) continue;

                var row = dgDocGia.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row == null) continue;

                var presenter = FindVisualChild<DataGridCellsPresenter>(row);
                if (presenter == null) continue;

                // Cột Họ Tên (index 1)
                HighlightCellText(kh.HoTen, keyword, presenter, 1);
            }
        }

        private void HighlightCellText(string text, string keyword, DataGridCellsPresenter presenter, int columnIndex)
        {
            var cell = presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
            if (cell == null) return;

            var txtBlock = FindVisualChild<TextBlock>(cell);
            if (txtBlock == null) return;

            txtBlock.Inlines.Clear();

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (string.IsNullOrEmpty(keyword))
            {
                txtBlock.Inlines.Add(new Run(text));
                return;
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

                txtBlock.Inlines.Add(new Run(text.Substring(startIndex, index - startIndex)));
                txtBlock.Inlines.Add(new Run(text.Substring(index, keyword.Length))
                {
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Red
                });

                startIndex = index + keyword.Length;
            }
        }

        // ===== Sự kiện TextChanged =====
        private void txtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            txtPlaceholder.Visibility = string.IsNullOrEmpty(keyword)
                ? Visibility.Visible
                : Visibility.Collapsed;

            if (string.IsNullOrEmpty(keyword))
            {
                dgDocGia.ItemsSource = danhSachGoc;
            }
            else
            {
                dgDocGia.ItemsSource = danhSachGoc
                    .Where(kh => !string.IsNullOrEmpty(kh.HoTen) && kh.HoTen.ToLower().Contains(keyword))
                    .ToList();
            }
            dgDocGia.Items.Refresh();
            dgDocGia.Dispatcher.InvokeAsync(() => HighlightMatchingText(keyword));
        }

        // ======= Xóa trắng form =======
        private void ClearForm()
        {
            txtMaDG.Clear();
            txtHoTen.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            dpNgaySinh.SelectedDate = null;
            cbGioiTinh.SelectedIndex = -1;
        }

        // ======= Nút Xóa =======
        private void btnXoaDG_Click(object sender, RoutedEventArgs e)
        {
            if (dgDocGia.SelectedItem is KhachHang kh)
            {
                var confirm = MessageBox.Show($"Bạn có chắc muốn xóa độc giả '{kh.HoTen}' không?",
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        kh.Ghichu="Đã xóa"; 
                        _context.SaveChanges();

                        MessageBox.Show("Xóa độc giả thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadDanhSachDocGia();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa độc giả:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn độc giả để xóa!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ======= Nút Cập nhật =======
        private void btnCapNhatDG_Click(object sender, RoutedEventArgs e)
        {
            if (dgDocGia.SelectedItem is KhachHang selected)
            {
                try
                {
                    // Lấy lại thông tin từ form
                    selected.HoTen = txtHoTen.Text.Trim();
                    selected.DiaChi = txtDiaChi.Text.Trim();
                    selected.Email = txtEmail.Text.Trim();
                    selected.SoDienThoai = txtSDT.Text.Trim();
                    selected.GioiTinh = (cbGioiTinh.SelectedItem as ComboBoxItem)?.Content.ToString();
                    selected.NgaySinh = dpNgaySinh.SelectedDate;

                    _context.KhachHangs.Update(selected);
                    _context.SaveChanges();

                    MessageBox.Show("Cập nhật thông tin độc giả thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDanhSachDocGia();

                    // Chọn lại item vừa cập nhật
                    var updated = _context.KhachHangs.FirstOrDefault(x => x.MaKhachHang == selected.MaKhachHang);
                    if (updated != null)
                    {
                        dgDocGia.SelectedItem = updated;
                        dgDocGia.ScrollIntoView(updated);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật độc giả:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn độc giả cần cập nhật!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ======= Khi chọn 1 dòng trong DataGrid =======
        private void dgDocGia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDocGia.SelectedItem is KhachHang selected)
            {
                txtMaDG.Text = selected.MaKhachHang.ToString();
                txtHoTen.Text = selected.HoTen;
                txtDiaChi.Text = selected.DiaChi;
                txtEmail.Text = selected.Email;
                txtSDT.Text = selected.SoDienThoai;
                dpNgaySinh.SelectedDate = selected.NgaySinh;


                if (!string.IsNullOrEmpty(selected.GioiTinh))
                {
                    foreach (ComboBoxItem item in cbGioiTinh.Items)
                    {
                        if (item.Content.ToString().Equals(selected.GioiTinh, StringComparison.OrdinalIgnoreCase))
                        {
                            cbGioiTinh.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    cbGioiTinh.SelectedIndex = -1;
                }
            }
        }

        // ======= Nút Reset =======
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            dgDocGia.SelectedItem = null;
        }
    }
}