using QUAN_LY.UI.Data;
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
using System.Windows.Shapes;

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Dangky.xaml
    /// </summary>
    public partial class Dangky : Window
    {
        private readonly DichvuDangKy dangKy;
        public Dangky()
        {
            InitializeComponent();
            pwdbox.Visibility = Visibility.Visible;
            txtpassword.Visibility = Visibility.Collapsed;
            pwdbox1.Visibility = Visibility.Visible;
            txtpassword1.Visibility = Visibility.Collapsed;
            rdNam.IsChecked = true;
            var db = new LibraryContext();
            dangKy = new DichvuDangKy(db);
        }

        private void btbdangnhap_Click(object sender, RoutedEventArgs e)
        {
            Dangnhap dangnhap = new Dangnhap();
            dangnhap.Show();
            this.Close();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            txtpassword.Text = pwdbox.Password;
            txtpassword.Visibility = Visibility.Visible;
            pwdbox.Visibility = Visibility.Collapsed;
            txtpassword1.Text = pwdbox1.Password;
            txtpassword1.Visibility = Visibility.Visible;
            pwdbox1.Visibility = Visibility.Collapsed;
        }
        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            pwdbox.Password = txtpassword.Text;
            pwdbox.Visibility = Visibility.Visible;
            txtpassword.Visibility = Visibility.Collapsed;
            pwdbox1.Password = txtpassword1.Text;
            pwdbox1.Visibility = Visibility.Visible;
            txtpassword1.Visibility = Visibility.Collapsed;
        }

        private void btbdangki_Click(object sender, RoutedEventArgs e)
        {
            string username = txttendangnhap.Text.Trim();
            string password = pwdbox.Password.Trim();
            string rePassword = pwdbox1.Password.Trim();
            string hoTen = txthoten.Text.Trim();
            DateTime? ngaySinh = datengaysinh.SelectedDate;
            string diaChi = txtdiachi.Text.Trim();
            string email = txtemail.Text.Trim();
            string soDienThoai = txtsodienthoai.Text.Trim();
            string gioiTinh = (rdNam.IsChecked == true) ? "Nam" : "Nữ";

            if (dangKy.DangKy(username, password, rePassword, hoTen, ngaySinh, diaChi, email, soDienThoai, gioiTinh, out string message))
            {
                MessageBox.Show("Dang ky thanh cong");
               
            }
            else
            {
                MessageBox.Show(message);
            }
        }

    }
}
