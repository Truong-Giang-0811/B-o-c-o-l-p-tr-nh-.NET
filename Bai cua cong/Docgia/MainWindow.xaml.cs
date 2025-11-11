using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Docgia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTraCuuSach_Click(object sender, RoutedEventArgs e)
        {
            stkPanel.Children.Clear();
            stkPanel.Children.Add(new TraCuuSach());
        }

        private void btnGioMuon_Click(object sender, RoutedEventArgs e)
        {
            stkPanel.Children.Clear();
            stkPanel.Children.Add(new GioMuon());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLichSuMuon_Click(object sender, RoutedEventArgs e)
        {
            stkPanel.Children.Clear();
            stkPanel.Children.Add(new LichSuMuon());
        }

        private void btnTaiKhoan_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}