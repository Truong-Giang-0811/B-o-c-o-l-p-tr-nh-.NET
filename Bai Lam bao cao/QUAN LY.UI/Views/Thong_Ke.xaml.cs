using LiveCharts;
using LiveCharts.Wpf;
using QUAN_LY.UI.Data;
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
    /// Interaction logic for Thong_Ke.xaml
    /// </summary>
    public partial class Thong_Ke : UserControl
    {
        private readonly LibraryContext db;
        public Thong_Ke()
        {
            InitializeComponent();

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Lấy tháng hiện tại
            int thangHienTai = DateTime.Now.Month;

            // Gán vào ComboBox
            cbMonth.SelectedIndex = thangHienTai - 1;

            // Gọi hàm hiển thị dữ liệu
            LoadData(thangHienTai);
        }
        // Hàm xử lý nút Lọc
        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = cbMonth.SelectedItem as ComboBoxItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn tháng trước khi lọc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int thang = int.Parse(selectedItem.Content.ToString().Split(' ')[1]);
            LoadData(thang);
        }
        private void LoadData(int thang)
        {
            // Lấy dữ liệu mượn theo tháng
            var duLieu = db.ChiTietMuonSaches
                .Where(ct => ct.NgayMuon.HasValue &&
                             ct.NgayMuon.Value.Month == thang &&
                             ct.Sach != null)
                .GroupBy(ct => ct.Sach.TheLoai)
                .Select(g => new
                {
                    TheLoai = g.Key,
                    TongSoLuongMuon = g.Sum(x => x.SoLuong)
                })
                .OrderBy(x => x.TheLoai)
                .ToList();

            // Xóa dữ liệu cũ của biểu đồ
            chart.Series.Clear();

            if (duLieu.Count == 0)
            {
                MessageBox.Show($"Không có dữ liệu mượn sách trong tháng {thang}.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Gán nhãn (thể loại) và giá trị (số lượt mượn)
            var labels = duLieu.Select(d => d.TheLoai).ToList();
            var values = new ChartValues<double>(duLieu.Select(d => (double)d.TongSoLuongMuon));

            // Tạo cột hiển thị
            var columnSeries = new ColumnSeries
            {
                Title = $"Số lượt mượn - Tháng {thang}",
                Values = values,
                Fill = new SolidColorBrush(Color.FromRgb(46, 125, 50)),
                DataLabels = true,
                LabelPoint = point => point.Y.ToString()
            };

            chart.Series.Add(columnSeries);

            // Cập nhật trục X
            chart.AxisX.Clear();
            chart.AxisX.Add(new Axis
            {
                Labels = labels,
                FontSize = 13,
                Foreground = new SolidColorBrush(Color.FromRgb(2, 136, 209)),
                Separator = new LiveCharts.Wpf.Separator { Step = 1 }
            });

            // Cập nhật trục Y
            chart.AxisY.Clear();
            chart.AxisY.Add(new Axis
            {
                Title = "Số lượt mượn",
                MinValue = 0,
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(2, 136, 209))
            });
        }
        private void cbMonth_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // TODO: Xử lý khi người dùng chọn tháng mới
        }
    }
}
