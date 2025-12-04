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
using System.Windows.Shapes;

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Dangnhap.xaml
    /// </summary>
    public partial class Dangnhap : Window
    {
        private readonly Xac_Thuc_Dangnhap xacthuc;
        private readonly LibraryContext db;
        public Dangnhap()
        {
            InitializeComponent();
            pwdbox.Visibility = Visibility.Visible;
            txtpassword.Visibility = Visibility.Collapsed;
            db = new LibraryContext();
            xacthuc = new Xac_Thuc_Dangnhap(db);

        }

        private void btbdangki_Click(object sender, RoutedEventArgs e)
        {
            Dangky dangky = new Dangky();
            dangky.Show();
            this.Close();
        }

        private void btbdangnhap_Click(object sender, RoutedEventArgs e)
        {
            if (chkHienMK.IsChecked == true)
            {
                chkHienMK.IsChecked = false;
            }
            string tendangnhap = txtbtendangnhap.Text;
            string matkhau = pwdbox.Password;
            var taikhoanadmin = xacthuc.DangNhap2(tendangnhap, matkhau);
            var taikhoankh = xacthuc.DangNhap(tendangnhap, matkhau);

            if (taikhoanadmin != null)
            {
                if (xacthuc.LaAdmin(taikhoanadmin))
                {
                    UserSession.SetAdmin(taikhoanadmin);
                    Giao_diện_admin giao_Diện_Admin = new Giao_diện_admin();
                    giao_Diện_Admin.Show();
                    this.Close();
                    // mở giao diện quản trị ở đây
                }
                else if (xacthuc.LaNhanVien(taikhoanadmin))
                {
                    UserSession.SetAdmin(taikhoanadmin);
                    Giao_Dien_Nhan_vien giao_Dien_Nhan_Vien = new Giao_Dien_Nhan_vien();
                    giao_Dien_Nhan_Vien.Show();
                    this.Close();

                    // mở giao diện nhân viên
                }
            }

            else if (taikhoankh != null)
            {
                if (xacthuc.LaKhachHang(taikhoankh))
                {
                    UserSession.SetKhachHang(taikhoankh);
                    Giao_dien_doc_gia giao_Dien_Doc_Gia = new Giao_dien_doc_gia();
                    giao_Dien_Doc_Gia.Show();
                    this.Close();

                    // mở giao diện khách hàng ở đây
                }
            }

            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
            }
        }
       
        

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            txtpassword.Text = pwdbox.Password;
            txtpassword.Visibility = Visibility.Visible;
            pwdbox.Visibility = Visibility.Collapsed;
        }
        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            pwdbox.Password = txtpassword.Text;
            pwdbox.Visibility = Visibility.Visible;
            txtpassword.Visibility = Visibility.Collapsed;
        }

        private void txtpassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
