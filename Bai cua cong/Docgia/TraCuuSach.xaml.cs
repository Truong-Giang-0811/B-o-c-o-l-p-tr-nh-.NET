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
using System.ComponentModel;

namespace Docgia
{
    /// <summary>
    /// Interaction logic for TraCuuSach.xaml
    /// </summary>
    public partial class TraCuuSach : UserControl, INotifyPropertyChanged
    {
        public List<Sach> danhSachSach { get; set; }

        private string _selectedTenSach;
        public string SelectedTenSach
        {
            get => _selectedTenSach;
            set
            {
                _selectedTenSach = value;
                OnPropertyChanged(nameof(SelectedTenSach));
            }
        }

        private string _selectedTacGia;
        public string SelectedTacGia
        {
            get => _selectedTacGia;
            set
            {
                _selectedTacGia = value;
                OnPropertyChanged(nameof(SelectedTacGia));
            }
        }

        private string _selectedTheLoai;
        public string SelectedTheLoai
        {
            get => _selectedTheLoai;
            set
            {
                _selectedTheLoai = value;
                OnPropertyChanged(nameof(SelectedTheLoai));
            }
        }

        private string _selectedNXB;
        public string SelectedNXB
        {
            get => _selectedNXB;
            set
            {
                _selectedNXB = value;
                OnPropertyChanged(nameof(SelectedNXB));
            }
        }

        private int _selectedSLTon;
        public int SelectedSLTon
        {
            get => _selectedSLTon;
            set
            {
                _selectedSLTon = value;
                OnPropertyChanged(nameof(SelectedSLTon));
            }
        }

        private string _selectedMoTa;
        public string SelectedMoTa
        {
            get => _selectedMoTa;
            set
            {
                _selectedMoTa = value;
                OnPropertyChanged(nameof(SelectedMoTa));
            }
        }
        public TraCuuSach()
        {
            InitializeComponent();
            NapDuLieuMau();
            dgSach.ItemsSource = danhSachSach;
            DataContext = this;
        }
        private void NapDuLieuMau()
        {
            danhSachSach = new List<Sach>
            {
                new Sach
                {
                   
                    TenSach = "Lập trình C++ cơ bản",
                    TacGia = "Nguyễn Văn A",
                    TheLoai = "Công nghệ thông tin",
                    NXB = "NXB Giáo dục",
                    SLTon = 10,
                    MoTa = "Cuốn sách cung cấp kiến thức nền tảng về ngôn ngữ C++, phù hợp cho sinh viên năm đầu CNTT."
                },
                new Sach
                {
                    
                    TenSach = "Giải tích 1",
                    TacGia = "Trần Thị B",
                    TheLoai = "Toán học",
                    NXB = "NXB Đại học Quốc Gia",
                    SLTon = 5,
                    MoTa = "Tài liệu phục vụ cho sinh viên khối kỹ thuật, trình bày các khái niệm giới hạn, đạo hàm, tích phân."
                },
                new Sach
                {
                    
                    TenSach = "Cấu trúc dữ liệu và giải thuật",
                    TacGia = "Phạm Văn C",
                    TheLoai = "Khoa học máy tính",
                    NXB = "NXB Khoa học và Kỹ thuật",
                    SLTon = 8,
                    MoTa = "Trình bày các cấu trúc dữ liệu cơ bản như danh sách, ngăn xếp, hàng đợi, cây và đồ thị."
                }
            };
        }
        private void btnThemGio_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTenSach == null)
            {
                MessageBox.Show("Vui lòng chọn sách trước khi thêm vào giỏ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Sach sach = new Sach
            {
                
                TenSach = SelectedTenSach,
                TacGia = SelectedTacGia,
                TheLoai = SelectedTheLoai,
                NXB = SelectedNXB,
                SLTon = SelectedSLTon,

            };

            Session.GioMuon.Add(sach);
            MessageBox.Show($"Đã thêm \"{SelectedTenSach}\" vào giỏ mượn!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void txtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtTimKiem.Text.ToLower();
            dgSach.ItemsSource = string.IsNullOrEmpty(keyword)
                ? danhSachSach
                : danhSachSach.FindAll(s => s.TenSach.ToLower().Contains(keyword));
        }

        private void dgSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSach.SelectedItem is Sach sach)
            {
               
                SelectedTenSach = sach.TenSach;
                SelectedTacGia = sach.TacGia;
                SelectedTheLoai = sach.TheLoai;
                SelectedNXB = sach.NXB;
                SelectedSLTon = sach.SLTon;
                SelectedMoTa = sach.MoTa;

                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
