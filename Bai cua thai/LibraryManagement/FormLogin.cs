using System;
using System.Linq;
using System.Windows.Forms;
using LibraryManagement.Models;
using LibraryManagement.Utils;
using System.Drawing; // Thêm để làm việc với Image
using System.IO;    // Thêm để làm việc với File

namespace LibraryManagement
{
    public partial class FormLogin : Form
    {
        private LibraryContext _context = new LibraryContext();

        public FormLogin()
        {
            InitializeComponent();
            
            // ⭐️ GỌI HÀM THIẾT LẬP NỀN KHI FORM KHỞI TẠO
            // Đảm bảo đường dẫn này khớp với cấu hình trong .csproj
            SetFormBackground("Images/Logo.png"); 
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text;
            
            string hash = HashHelper.HashPassword(pass);
            
            // ⚠️ Lưu ý: Lỗi "Invalid object name 'Users'." xảy ra ở dòng này nếu bảng không tồn tại.
            var u = _context.Users.FirstOrDefault(x => x.Username == user && x.PasswordHash == hash);

            if (u != null)
            {
                MessageBox.Show($"Đăng nhập thành công với vai trò: {u.Role}", "Thành công");
                
                this.Hide();
                var main = new FormBook(); // Chuyển đến FormBook
                main.Show();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu. Vui lòng thử lại.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🖼️ PHƯƠNG THỨC HỖ TRỢ TẢI ẢNH TỪ FILE VÀ THIẾT LẬP NỀN
        private void SetFormBackground(string relativePath)
        {
            try
            {
                // Kiểm tra file có tồn tại trong thư mục chạy không
                if (File.Exists(relativePath))
                {
                    // Tải ảnh từ đường dẫn file
                    this.BackgroundImage = Image.FromFile(relativePath); 
                    
                    // Thiết lập kiểu hiển thị ảnh:
                    this.BackgroundImageLayout = ImageLayout.Stretch; // Kéo giãn để vừa form
                    // Có thể thay bằng ImageLayout.Zoom nếu bạn muốn giữ tỷ lệ ảnh
                }
                else
                {
                    // Thông báo lỗi nhỏ trong Console nếu không tìm thấy file
                    Console.WriteLine($"Lỗi: Không tìm thấy file ảnh nền tại đường dẫn: {relativePath}");
                }
            }
            catch (Exception ex)
            {
                // Thông báo lỗi nhỏ trong Console nếu có lỗi tải ảnh
                Console.WriteLine($"Đã xảy ra lỗi khi thiết lập ảnh nền: {ex.Message}");
            }
        }
    }
}