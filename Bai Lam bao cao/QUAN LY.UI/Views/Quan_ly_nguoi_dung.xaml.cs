using QUAN_LY.UI.Data;
using QUAN_LY.UI.Models;
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
    /// Interaction logic for Quan_ly_nguoi_dung.xaml
    /// </summary>
    public partial class Quan_ly_nguoi_dung : UserControl
    {
        private readonly LibraryContext _context;

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
            dgDocGia.ItemsSource = _context.KhachHangs.ToList();
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
            if (dgDocGia.SelectedItem is KhachHang selected)
            {
                var confirm = MessageBox.Show($"Bạn có chắc muốn xóa độc giả '{selected.HoTen}' không?",
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        _context.KhachHangs.Remove(selected);
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
        private void btnInDG_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            dgDocGia.SelectedItem = null;
        }
    }
}
