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

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Quan_Ly_Nhan_Vien.xaml
    /// </summary>
    public partial class Quan_Ly_Nhan_Vien : UserControl
    {
        private readonly Quan_Ly_Nhanvien _qlNhanVien;
        private readonly LibraryContext _context;

        public Quan_Ly_Nhan_Vien()
        {
            try
            {
                InitializeComponent();
                _context = new LibraryContext();
                _qlNhanVien = new Quan_Ly_Nhanvien(_context);

                LoadDanhSachNhanVien(); // Gọi hàm load dữ liệu
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo giao diện Quản lý nhân viên:\n" + ex.Message);
            }
        }
        private List<Admin> danhSachGoc;

        private void LoadDanhSachNhanVien()
        {
            danhSachGoc = _context.Admins
                .Where(a => a.Chucvu == "Nhân viên")
                .ToList();
            dgNhanVien.ItemsSource = danhSachGoc;
        }
        private void ClearForm()
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtNhapLaiMK.Clear();

            txtTenNV.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtDienThoai.Clear();
            dpNgaySinh.SelectedDate = null;
            rdNam.IsChecked = false;
            rdNu.IsChecked = false;
        }
        private void chkHienMK_Checked(object sender, RoutedEventArgs e)
        {
            // Mật khẩu 
            txtMatKhau.Text = pwbMatKhau.Password;
            txtMatKhau.Visibility = Visibility.Visible;
            pwbMatKhau.Visibility = Visibility.Collapsed;

            // Nhập lại Mật khẩu
            txtNhapLaiMK.Text = pwbNhapLaiMK.Password;
            txtNhapLaiMK.Visibility = Visibility.Visible;
            pwbNhapLaiMK.Visibility = Visibility.Collapsed;
        }


        private void chkHienMK_Unchecked(object sender, RoutedEventArgs e)
        {
            // Mật khẩu 
            pwbMatKhau.Password = txtMatKhau.Text;
            pwbMatKhau.Visibility = Visibility.Visible;
            txtMatKhau.Visibility = Visibility.Collapsed;

            // Nhập lại Mật khẩu
            pwbNhapLaiMK.Password = txtNhapLaiMK.Text;
            pwbNhapLaiMK.Visibility = Visibility.Visible;
            txtNhapLaiMK.Visibility = Visibility.Collapsed;
        }
        private void btnThemnv_Click(object sender, RoutedEventArgs e)
        {
            if (ckbHienMK.IsChecked == true)
            {
                ckbHienMK.IsChecked = false;
            }
            string username = txtTenDangNhap.Text.Trim();
            string password = pwbMatKhau.Password.Trim();
            string rePassword = pwbNhapLaiMK.Password.Trim();
            string hoTen = txtTenNV.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string email = txtEmail.Text.Trim();
            string soDienThoai = txtDienThoai.Text.Trim();
            DateTime? ngaySinh = dpNgaySinh.SelectedDate;
            string gioiTinh = (rdNam.IsChecked == true) ? "Nam" : "Nữ";


            if (_qlNhanVien.ThemNhanVien(username, password, rePassword, hoTen, ngaySinh, diaChi, email, soDienThoai, gioiTinh, out string message))
            {
                MessageBox.Show(message, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDanhSachNhanVien();
                ClearForm();
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnXoaNV_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is Admin selected)
            {
                var confirm = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên '{selected.HoTen}' không?",
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    if (_qlNhanVien.XoaNhanVien(selected.MaNhanVien, out string message))
                    {
                        MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadDanhSachNhanVien();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCapnhatNV_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is Admin selected)
            {
                if (ckbHienMK.IsChecked == true)
                {
                    ckbHienMK.IsChecked = false;
                }

                string username = txtTenDangNhap.Text.Trim();
                string password = pwbMatKhau.Password.Trim();
                string rePassword = pwbNhapLaiMK.Password.Trim();
                string hoTen = txtTenNV.Text.Trim();
                string diaChi = txtDiaChi.Text.Trim();
                string email = txtEmail.Text.Trim();
                string soDienThoai = txtDienThoai.Text.Trim();
                DateTime? ngaySinh = dpNgaySinh.SelectedDate;
                string gioiTinh = (rdNam.IsChecked == true) ? "Nam" : "Nữ";

                if (_qlNhanVien.CapNhatNhanVien(selected.MaNhanVien, username, password, rePassword, hoTen, ngaySinh, diaChi, email, soDienThoai, gioiTinh, out string message))
                {
                    MessageBox.Show(message, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    int currentId = selected.MaNhanVien;

                    LoadDanhSachNhanVien();

                    // Chọn lại item vừa cập nhật
                    var updated = _context.Admins.FirstOrDefault(a => a.MaNhanVien == currentId);
                    if (updated != null)
                    {
                        dgNhanVien.SelectedItem = updated;
                        dgNhanVien.ScrollIntoView(updated);
                    }
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void dgNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is Admin selected)
            {
                txtTenDangNhap.Text = selected.Tendangnhap;
                txtTenNV.Text = selected.HoTen;
                txtDiaChi.Text = selected.DiaChi;
                txtEmail.Text = selected.Email;
                txtDienThoai.Text = selected.SoDienThoai;
                dpNgaySinh.SelectedDate = selected.NgaySinh;
                if (selected.GioiTinh == "Nam")
                {
                    rdNam.IsChecked = true;
                    rdNu.IsChecked = false;
                }
                else if (selected.GioiTinh == "Nữ")
                {
                    rdNam.IsChecked = false;
                    rdNu.IsChecked = true;
                }
                else
                {
                    rdNam.IsChecked = false;
                    rdNu.IsChecked = false;
                }

            }
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
            foreach (var item in dgNhanVien.Items)
            {
                if (item is not Admin nv) continue;

                var row = dgNhanVien.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row == null) continue;

                // tìm trong cột Tên nhân viên (TemplateColumn)
                var presenter = FindVisualChild<DataGridCellsPresenter>(row);
                if (presenter == null) continue;

                var cell = presenter.ItemContainerGenerator.ContainerFromIndex(1) as DataGridCell; // cột thứ 2
                if (cell == null) continue;

                var txtBlock = FindVisualChild<TextBlock>(cell);
                if (txtBlock == null) continue;

                string text = nv.HoTen ?? "";
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
                    // phần khớp
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

            // Hiển thị hoặc ẩn placeholder
            txtPlaceholder.Visibility = string.IsNullOrEmpty(keyword)
                ? Visibility.Visible
                : Visibility.Collapsed;

            if (string.IsNullOrEmpty(keyword))
            {
                dgNhanVien.ItemsSource = danhSachGoc;
            }
            else
            {
                dgNhanVien.ItemsSource = danhSachGoc
                    .Where(nv => nv.HoTen.ToLower().Contains(keyword) ||
                                 nv.MaNhanVien.ToString().ToLower().Contains(keyword))
                    .ToList();
            }

            dgNhanVien.Items.Refresh();

            // 👉 Gọi hàm tô đậm sau khi DataGrid hiển thị xong
            dgNhanVien.Dispatcher.InvokeAsync(() => HighlightMatchingText(keyword));
        }



        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            dgNhanVien.SelectedItem = null;
        }
    }
}