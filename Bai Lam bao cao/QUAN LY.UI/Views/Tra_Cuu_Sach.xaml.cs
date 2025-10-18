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

namespace QUAN_LY.UI.Views
{
    /// <summary>
    /// Interaction logic for Tra_Cuu_Sach.xaml
    /// </summary>
    public partial class Tra_Cuu_Sach : UserControl
    {

        private readonly LibraryContext _context;
        private readonly Them_Gio_Hang _gioHangService;
       


        public Tra_Cuu_Sach()
        {
            InitializeComponent();
            _context = new Data.LibraryContext();
            _gioHangService = new Them_Gio_Hang(_context);
            dgSach.ItemsSource = _context.Saches.ToList();
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
