using System;
using System.Linq;
using System.Windows.Forms;
using LibraryManagement.Models;

namespace LibraryManagement
{
    public partial class FormBook : Form
    {
        private LibraryContext _context;

        public FormBook()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadData();
        }

        private void LoadData()
        {
            // Tải danh sách sách và gán cho DataGridView
            dgvBooks.DataSource = _context.Books.ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var book = new Book
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Category = txtCategory.Text,
                Quantity = int.TryParse(txtQuantity.Text, out int q) ? q : 0
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            LoadData();
            MessageBox.Show("Thêm sách thành công");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow == null) return;
            
            // Lấy BookId từ hàng đang chọn
            if (!int.TryParse(dgvBooks.CurrentRow.Cells["BookId"].Value?.ToString(), out int id)) return;
            
            var book = _context.Books.Find(id);
            if (book == null) return;
            
            book.Title = txtTitle.Text;
            book.Author = txtAuthor.Text;
            book.Category = txtCategory.Text;
            book.Quantity = int.TryParse(txtQuantity.Text, out int q) ? q : 0;
            
            _context.SaveChanges();
            LoadData();
            MessageBox.Show("Cập nhật thành công");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow == null) return;
            
            if (!int.TryParse(dgvBooks.CurrentRow.Cells["BookId"].Value?.ToString(), out int id)) return;
            
            var book = _context.Books.Find(id);
            
            if (book != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa sách này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _context.Books.Remove(book);
                    _context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Xóa thành công");
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim();
            // Lọc dữ liệu theo Title, Author, hoặc Category
            var list = _context.Books
                .Where(b => (b.Title != null && b.Title.Contains(key)) || 
                            (b.Author != null && b.Author.Contains(key)) || 
                            (b.Category != null && b.Category.Contains(key)))
                .ToList();
            dgvBooks.DataSource = list;
        }

        private void dgvBooks_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow == null || dgvBooks.CurrentRow.Index < 0) return;
            
            // Đổ dữ liệu từ hàng đang chọn lên các TextBox
            txtTitle.Text = dgvBooks.CurrentRow.Cells["Title"].Value?.ToString();
            txtAuthor.Text = dgvBooks.CurrentRow.Cells["Author"].Value?.ToString();
            txtCategory.Text = dgvBooks.CurrentRow.Cells["Category"].Value?.ToString();
            txtQuantity.Text = dgvBooks.CurrentRow.Cells["Quantity"].Value?.ToString();
        }
    }
}