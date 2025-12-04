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
    /// Interaction logic for Tra_Cuu_Sach.xaml
    /// </summary>
    public partial class Tra_Cuu_Sach : UserControl
    {

        private readonly LibraryContext _context;
        private readonly Them_Gio_Hang _gioHangService;
        private List<Sach> danhSachGoc;


        public Tra_Cuu_Sach()
        {
            InitializeComponent();
            _context = new Data.LibraryContext();
            _gioHangService = new Them_Gio_Hang(_context);
            danhSachGoc = _context.Saches.ToList();
            dgSach.ItemsSource = danhSachGoc;
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
        }
        private void TxtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            txtPlaceholder.Visibility = string.IsNullOrEmpty(keyword)
                               ? Visibility.Visible
                               : Visibility.Collapsed;
            if (string.IsNullOrEmpty(keyword))
            {
                dgSach.ItemsSource = danhSachGoc;
            }
            else
            {
                dgSach.ItemsSource = danhSachGoc
                    .Where(s => !string.IsNullOrEmpty(s.TieuDe) && s.TieuDe.ToLower().Contains(keyword))
                    .ToList();
            }

            dgSach.Items.Refresh();

            dgSach.Dispatcher.InvokeAsync(() => HighlightMatchingText(keyword));
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
            foreach (var item in dgSach.Items)
            {
                if (item is not Sach sach) continue;

                var row = dgSach.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row == null) continue;

                var presenter = FindVisualChild<DataGridCellsPresenter>(row);
                if (presenter == null) continue;

                // Chỉ highlight cột Tên sách (index 0)
                var cell = presenter.ItemContainerGenerator.ContainerFromIndex(0) as DataGridCell;
                if (cell == null) continue;

                var txtBlock = FindVisualChild<TextBlock>(cell);
                if (txtBlock == null) continue;

                txtBlock.Inlines.Clear();

                string text = sach.TieuDe ?? "";
                if (string.IsNullOrEmpty(keyword))
                {
                    txtBlock.Inlines.Add(new Run(text));
                    continue;
                }

                int startIndex = 0;
                string lowerText = text.ToLower();

                while (true)
                {
                    int index = lowerText.IndexOf(keyword, startIndex);
                    if (index == -1)
                    {
                        txtBlock.Inlines.Add(new Run(text.Substring(startIndex)));
                        break;
                    }

                    // phần trước từ khóa
                    txtBlock.Inlines.Add(new Run(text.Substring(startIndex, index - startIndex)));
                    // phần từ khóa
                    txtBlock.Inlines.Add(new Run(text.Substring(index, keyword.Length))
                    {
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Red
                    });

                    startIndex = index + keyword.Length;
                }
            }
        }
        private void dgSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSach.SelectedItem is Sach selectedBook)
            {
                // Hiển thị thông tin chi tiết của sách đã chọn
                txtTensach.Text = selectedBook.TieuDe;
                txtTacgia.Text = selectedBook.TacGia;
                txtTheloai.Text = selectedBook.TheLoai;
                txtNhaxb.Text = selectedBook.NhaXuatBan;
                txtMota.Text = selectedBook.Mota;
                txtSoluongton.Text = selectedBook.SoLuongTon.ToString();
            }
        }

        private void btnThemGio_Click(object sender, RoutedEventArgs e)
        {
            if (dgSach.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn sách trước khi thêm!", "Cảnh báo");
                return;
            }

            if (dgSach.SelectedItem is Sach selected)
            {
                int maKhachHang = UserSession.CurrentKhachHang.MaKhachHang;

                // Thêm vào giỏ mượn
                bool result = _gioHangService.themvaogio(selected.MaSach);

                if (result)
                {
                    MessageBox.Show("✅ Đã thêm vào giỏ mượn thành công!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("❌ Thêm vào giỏ mượn thất bại!", "Lỗi");
                }
            }
            // Lấy mã khách hàng hiện tại (được lưu khi đăng nhập)


        }
    }
}