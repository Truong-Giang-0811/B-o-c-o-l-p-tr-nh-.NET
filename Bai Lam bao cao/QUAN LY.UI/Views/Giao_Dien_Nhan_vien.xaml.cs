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
using System.Windows.Shapes;

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Giao_Dien_Nhan_vien.xaml
    /// </summary>
    public partial class Giao_Dien_Nhan_vien : Window
    {
        public Giao_Dien_Nhan_vien()
        {
            InitializeComponent();
            Mainconten.Content = new Views.Quan_Ly_Sach();
        }
        private void btn_dangxuat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
               "Bạn có chắc chắn muốn đăng xuất không?",
               "Xác nhận đăng xuất",
           MessageBoxButton.YesNo,
           MessageBoxImage.Question);

            // Kiểm tra nếu người dùng chọn 'Yes'
            if (result == MessageBoxResult.Yes)
            {
                // 1. Tạo và hiển thị cửa sổ Đăng nhập mới
                Dangnhap dangnhap = new Dangnhap();
                dangnhap.Show();

                // 2. Đóng cửa sổ hiện tại (ví dụ: cửa sổ chính/admin)
                this.Close();
            }
        }

        private void Quanlysach(object sender, RoutedEventArgs e)
        {
            Mainconten.Content = new Views.Quan_Ly_Sach();
        }

        private void Quanlydonmuon(object sender, RoutedEventArgs e)
        {
            Mainconten.Content = new Views.Quan_ly_muon_tra();
        }

        private void Taikhoancanhan(object sender, RoutedEventArgs e)
        {
            Mainconten.Content = new Views.Tai_khoan_ca_nhan();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mainconten.Content = new Views.Quan_ly_nguoi_dung();
        }

        private void Baocaothongke(object sender, RoutedEventArgs e)
        {
            Mainconten.Content = new Views.Thong_Ke();
        }
    }
}