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
using static System.Collections.Specialized.BitVector32;

namespace Docgia
{
    /// <summary>
    /// Interaction logic for GioMuon.xaml
    /// </summary>
    public partial class GioMuon : UserControl
    {
        public GioMuon()
        {
            InitializeComponent();
            dgGioMuon.ItemsSource = Session.GioMuon;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgGioMuon.SelectedItem is Sach sach)
            {
                Session.GioMuon.Remove(sach);
                dgGioMuon.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sách để xóa khỏi giỏ.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            // Lưu các thay đổi đang nhập trong DataGrid
            dgGioMuon.CommitEdit(DataGridEditingUnit.Cell, true);
            dgGioMuon.CommitEdit(DataGridEditingUnit.Row, true);

            // Lấy danh sách sách được chọn
            var sachChon = Session.GioMuon.Where(s => s.IsChecked).ToList();

            if (sachChon.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sách để mượn.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Tính tổng số lượng mượn thật sự
            int tongSoLuong = 0;
            foreach (var sach in sachChon)
            {
                if (sach.SLMuon <= 0)
                {
                    MessageBox.Show($"Số lượng mượn của sách \"{sach.TenSach}\" không hợp lệ!",
                                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (sach.SLMuon > sach.SLTon)
                {
                    MessageBox.Show($"Không đủ số lượng tồn cho sách \"{sach.TenSach}\"!",
                                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                tongSoLuong += sach.SLMuon;
            }

            MessageBox.Show($"✅ Đã xác nhận mượn {tongSoLuong} cuốn từ {sachChon.Count} sách được chọn.",
                            "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            // Xóa những sách đã được mượn ra khỏi giỏ
            foreach (var sach in sachChon)
            {
                Session.GioMuon.Remove(sach);
            }

            dgGioMuon.Items.Refresh();
        }
        

        private void dgGioMuon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
