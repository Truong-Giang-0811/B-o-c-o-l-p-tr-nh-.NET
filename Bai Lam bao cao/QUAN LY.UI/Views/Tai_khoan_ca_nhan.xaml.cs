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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Tai_khoan_ca_nhan.xaml
    /// </summary>
    public partial class Tai_khoan_ca_nhan : UserControl
    {
        private readonly LibraryContext _context;
        private KhachHang _nguoiDung;
        private Admin _nhanvien;
        public Tai_khoan_ca_nhan()
        {
            InitializeComponent();
            _context = new LibraryContext();
            var userId = UserSession.CurrentKhachHang?.MaKhachHang;
            var adminId = UserSession.CurrentAdmin?.MaNhanVien;

            if (userId != null) // Nếu là khách hàng
            {
                _nguoiDung = _context.KhachHangs.FirstOrDefault(u => u.MaKhachHang == userId);
                HienThiThongTinKhachHang(_nguoiDung);
            }
            else if (adminId != null) // Nếu là nhân viên
            {
                _nhanvien = _context.Admins.FirstOrDefault(a => a.MaNhanVien == adminId);
                HienThiThongTinNhanVien(_nhanvien);
            }

            // Hiển thị ảnh hiện có
            if (!string.IsNullOrEmpty(_nguoiDung?.Anhcanhan) && File.Exists(_nguoiDung.Anhcanhan))
                imgAvatar.Source = new BitmapImage(new Uri(_nguoiDung.Anhcanhan, UriKind.Absolute));
            else
                imgAvatar.Source = new BitmapImage(new Uri("/Anh/avatarzoro.jpg", UriKind.Relative));
        }

        private void HienThiThongTinKhachHang(KhachHang kh)
        {
            if (kh == null) return;

            txtTenTaiKhoan.Text = kh.HoTen;
            txtHoTen.Text = kh.HoTen;
            txtSDT.Text = kh.SoDienThoai;
            txtEmail.Text = kh.Email;
            txtDiaChi.Text = kh.DiaChi;
            txtGioiTinh.Text = kh.GioiTinh;
            txtNgaySinh.Text = kh.NgaySinh?.ToString("dd/MM/yyyy");

            // Hiển thị ảnh
            if (!string.IsNullOrEmpty(kh.Anhcanhan) && File.Exists(kh.Anhcanhan))
                imgAvatar.Source = new BitmapImage(new Uri(kh.Anhcanhan, UriKind.Absolute));
            else
                imgAvatar.Source = new BitmapImage(new Uri("/Anh/avatarzoro.jpg", UriKind.Relative));
        }

        private void HienThiThongTinNhanVien(Admin ad)
        {
            if (ad == null) return;

            txtTenTaiKhoan.Text = ad.HoTen;
            txtHoTen.Text = ad.HoTen;
            txtSDT.Text = ad.SoDienThoai;
            txtEmail.Text = ad.Email;
            txtDiaChi.Text = ad.DiaChi;
            txtGioiTinh.Text = ad.GioiTinh;
            txtNgaySinh.Text = ad.NgaySinh?.ToString("dd/MM/yyyy");

            // Hiển thị ảnh
            if (!string.IsNullOrEmpty(ad.Anhcanhan) && File.Exists(ad.Anhcanhan))
                imgAvatar.Source = new BitmapImage(new Uri(ad.Anhcanhan, UriKind.Absolute));
            else
                imgAvatar.Source = new BitmapImage(new Uri("/Anh/avatarzoro.jpg", UriKind.Relative));
        }

        private void imgAvatar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Hộp thoại chọn ảnh
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.Title = "Chọn ảnh đại diện mới";

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedPath = openFileDialog.FileName;
                   // ✅ Lấy thư mục gốc của dự án (tránh nằm trong bin/Debug)
                    string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                    string imagesDir = System.IO.Path.Combine(projectDir, "Anh", "Users");


                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);

                    // Tên file mới (ví dụ: kh_5.png)
                    string newFileName = $"kh_{UserSession.CurrentKhachHang.MaKhachHang}{System.IO.Path.GetExtension(selectedPath)}";
                    string destPath = System.IO.Path.Combine(imagesDir, newFileName);

                    File.Copy(selectedPath, destPath, true);

                    // Cập nhật giao diện ngay
                    imgAvatar.Source = new BitmapImage(new Uri(destPath, UriKind.Absolute));

                    // Lưu đường dẫn ảnh vào cơ sở dữ liệu
                    var userId = UserSession.CurrentKhachHang?.MaKhachHang;
                    if (userId != null)
                    {
                        var khachHang = _context.KhachHangs.FirstOrDefault(u => u.MaKhachHang == userId);
                        if (khachHang != null)
                        {
                            khachHang.Anhcanhan = destPath;
                            _context.SaveChanges();

                            // Cập nhật session hiện tại
                            UserSession.SetKhachHang(khachHang);

                            MessageBox.Show("Đã cập nhật ảnh đại diện thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đổi ảnh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void chkHienMK_Checked(object sender, RoutedEventArgs e)
        {
            // Mật khẩu cũ
            txtMKCu.Text = pwbMKCu.Password;
            txtMKCu.Visibility = Visibility.Visible;
            pwbMKCu.Visibility = Visibility.Collapsed;

            // Mật khẩu mới
            txtMKMoi.Text = pwbMKMoi.Password;
            txtMKMoi.Visibility = Visibility.Visible;
            pwbMKMoi.Visibility = Visibility.Collapsed;

            // Nhập lại mật khẩu
            txtNhapLai.Text = pwbNhapLai.Password;
            txtNhapLai.Visibility = Visibility.Visible;
            pwbNhapLai.Visibility = Visibility.Collapsed;
        }
        private void chkHienMK_Unchecked(object sender, RoutedEventArgs e)
        {
            // Mật khẩu cũ
            pwbMKCu.Password = txtMKCu.Text;
            pwbMKCu.Visibility = Visibility.Visible;
            txtMKCu.Visibility = Visibility.Collapsed;

            // Mật khẩu mới
            pwbMKMoi.Password = txtMKMoi.Text;
            pwbMKMoi.Visibility = Visibility.Visible;
            txtMKMoi.Visibility = Visibility.Collapsed;

            // Nhập lại mật khẩu
            pwbNhapLai.Password = txtNhapLai.Text;
            pwbNhapLai.Visibility = Visibility.Visible;
            txtNhapLai.Visibility = Visibility.Collapsed;
        }
        private void btnDoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            popupDoiMK.Visibility = Visibility.Visible;
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            popupCapNhat.Visibility = Visibility.Visible;
        }

        private void btnDongY_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkHienMK.IsChecked == true)
                {
                    chkHienMK.IsChecked = false;
                }
                string mkCu = pwbMKCu.Password.Trim();
                string mkMoi = pwbMKMoi.Password.Trim();
                string nhapLai = pwbNhapLai.Password.Trim();

                // 🔸 Kiểm tra nhập đủ
                if (string.IsNullOrEmpty(mkCu) || string.IsNullOrEmpty(mkMoi) || string.IsNullOrEmpty(nhapLai))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 🔸 Kiểm tra mật khẩu mới khớp nhau
                if (mkMoi != nhapLai)
                {
                    MessageBox.Show("Mật khẩu mới và nhập lại không khớp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 🔸 Nếu là khách hàng
                if (_nguoiDung != null)
                {
                    // Kiểm tra mật khẩu cũ
                    bool kiemTraMKCu = BCrypt.Net.BCrypt.Verify(mkCu, _nguoiDung.Matkhau);
                    if (!kiemTraMKCu)
                    {
                        MessageBox.Show("Mật khẩu hiện tại không đúng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Hash mật khẩu mới
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(mkMoi);
                    _nguoiDung.Matkhau = passwordHash;

                    _context.KhachHangs.Update(_nguoiDung);
                    _context.SaveChanges();

                    MessageBox.Show("Đổi mật khẩu thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    popupDoiMK.Visibility = Visibility.Collapsed;
                }
                // 🔸 Nếu là nhân viên
                else if (_nhanvien != null)
                {
                    bool kiemTraMKCu = BCrypt.Net.BCrypt.Verify(mkCu, _nhanvien.Matkhau);
                    if (!kiemTraMKCu)
                    {
                        MessageBox.Show("Mật khẩu hiện tại không đúng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(mkMoi);
                    _nhanvien.Matkhau = passwordHash;

                    _context.Admins.Update(_nhanvien);
                    _context.SaveChanges();

                    MessageBox.Show("Đổi mật khẩu thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    popupDoiMK.Visibility = Visibility.Collapsed;
                }

                // 🔁 Xóa ô nhập
                txtMKCu.Clear();
                txtMKMoi.Clear();
                txtNhapLai.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đổi mật khẩu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBoQuaDoiMK_Click(object sender, RoutedEventArgs e)
        {
            popupDoiMK.Visibility = Visibility.Collapsed;
        }

        private void btnLuuCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? ngaySinh = dpCNNgaySinh.SelectedDate;

                // 🧩 Kiểm tra có chọn ngày sinh không
                if (ngaySinh == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày sinh!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra đủ 18 tuổi
                int tuoi = DateTime.Now.Year - ngaySinh.Value.Year;
                if (ngaySinh.Value.Date > DateTime.Now.AddYears(-tuoi)) tuoi--; // kiểm tra chính xác theo ngày
                if (tuoi < 18)
                {
                    MessageBox.Show("Bạn phải đủ 18 tuổi để cập nhật thông tin!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //Nếu qua kiểm tra tuổi thì cập nhật
                if (_nguoiDung != null) // khách hàng
                {
                    _nguoiDung.HoTen = txtCNHoTen.Text;
                    _nguoiDung.SoDienThoai = txtCNSDT.Text;
                    _nguoiDung.Email = txtCNEmail.Text;
                    _nguoiDung.DiaChi = txtCNDiaChi.Text;
                    _nguoiDung.GioiTinh = ((ComboBoxItem)cbCNGioiTinh.SelectedItem)?.Content.ToString();
                    _nguoiDung.NgaySinh = ngaySinh;

                    _context.KhachHangs.Update(_nguoiDung);
                    _context.SaveChanges();

                    UserSession.SetKhachHang(_nguoiDung);
                    HienThiThongTinKhachHang(_nguoiDung);
                }
                else if (_nhanvien != null) // nhân viên
                {
                    _nhanvien.HoTen = txtCNHoTen.Text;
                    _nhanvien.SoDienThoai = txtCNSDT.Text;
                    _nhanvien.Email = txtCNEmail.Text;
                    _nhanvien.DiaChi = txtCNDiaChi.Text;
                    _nhanvien.GioiTinh = ((ComboBoxItem)cbCNGioiTinh.SelectedItem)?.Content.ToString();
                    _nhanvien.NgaySinh = ngaySinh;

                    _context.Admins.Update(_nhanvien);
                    _context.SaveChanges();

                    UserSession.SetAdmin(_nhanvien);
                    HienThiThongTinNhanVien(_nhanvien);
                }

                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                popupCapNhat.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnHuyCapNhat_Click(object sender, RoutedEventArgs e)
        {
            popupCapNhat.Visibility = Visibility.Collapsed;
        }
    }
}
