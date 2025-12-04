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
    /// Interaction logic for Giao_dien_doc_gia.xaml
    /// </summary>
    public partial class Giao_dien_doc_gia : Window
    {
        public Giao_dien_doc_gia()
        {
            InitializeComponent();
            Mainconten.Content = new Views.Tra_Cuu_Sach();
        }

        private void btnTraCuuSach_Click(object sender, RoutedEventArgs e)
        {
            Mainconten.Content = new Views.Tra_Cuu_Sach();
        }

        private void btnGioMuon_Click(object sender, RoutedEventArgs e)
        {
            Mainconten.Content = new Views.Gio_muon();
        }

        private void btnLichSuMuon_Click(object sender, RoutedEventArgs e)
        {
            Mainconten.Content = new Views.Lich_su_muon();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Đăng xuất tài khoản",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) 
            {
                Dangnhap dangnhap = new Dangnhap();
                dangnhap.Show();
                this.Close();
            }
                
        }

        private void btnTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            Mainconten.Content = new Views.Tai_khoan_ca_nhan();
        }
    }
}
