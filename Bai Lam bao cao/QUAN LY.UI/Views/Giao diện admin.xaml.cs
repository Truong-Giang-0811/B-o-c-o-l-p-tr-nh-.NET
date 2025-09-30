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
    /// Interaction logic for Giao_diện_admin.xaml
    /// </summary>
    public partial class Giao_diện_admin : Window
    {
        public Giao_diện_admin()
        {
            InitializeComponent();
        }

        private void btn_dangxuat_Click(object sender, RoutedEventArgs e)
        {
            Dangnhap dangnhap = new Dangnhap();
            dangnhap.Show();
            this.Close();
        }
    }
}
