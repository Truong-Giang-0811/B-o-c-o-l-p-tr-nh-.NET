using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement
{
    public partial class FormBook
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        // ------------------------------------
        // KHAI BÁO CÁC BIẾN CONTROL
        // ------------------------------------
        private System.Windows.Forms.DataGridView dgvBooks;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSearch;
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 700); 
            this.Text = "Quản Lý Sách";

            // 1. KHỞI TẠO CÁC CONTROLS
            this.dgvBooks = new System.Windows.Forms.DataGridView();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();

            // 2. CẤU HÌNH VÀ VỊ TRÍ
            int startY = 30;
            int labelWidth = 100;
            int inputWidth = 250;
            int rowSpacing = 30;
            int buttonWidth = 100;
            int currentX = 50;

            // --- Input Area ---
            
            // Tiêu đề
            this.Controls.Add(new System.Windows.Forms.Label { Text = "Tiêu đề:", Location = new System.Drawing.Point(currentX, startY), Size = new System.Drawing.Size(labelWidth, 20) });
            this.txtTitle.Location = new System.Drawing.Point(currentX + labelWidth, startY);
            this.txtTitle.Size = new System.Drawing.Size(inputWidth, 20);
            this.Controls.Add(this.txtTitle);

            // Tác giả
            startY += rowSpacing;
            this.Controls.Add(new System.Windows.Forms.Label { Text = "Tác giả:", Location = new System.Drawing.Point(currentX, startY), Size = new System.Drawing.Size(labelWidth, 20) });
            this.txtAuthor.Location = new System.Drawing.Point(currentX + labelWidth, startY);
            this.txtAuthor.Size = new System.Drawing.Size(inputWidth, 20);
            this.Controls.Add(this.txtAuthor);

            // Thể loại
            startY += rowSpacing;
            this.Controls.Add(new System.Windows.Forms.Label { Text = "Thể loại:", Location = new System.Drawing.Point(currentX, startY), Size = new System.Drawing.Size(labelWidth, 20) });
            this.txtCategory.Location = new System.Drawing.Point(currentX + labelWidth, startY);
            this.txtCategory.Size = new System.Drawing.Size(inputWidth, 20);
            this.Controls.Add(this.txtCategory);

            // Số lượng
            startY += rowSpacing;
            this.Controls.Add(new System.Windows.Forms.Label { Text = "Số lượng:", Location = new System.Drawing.Point(currentX, startY), Size = new System.Drawing.Size(labelWidth, 20) });
            this.txtQuantity.Location = new System.Drawing.Point(currentX + labelWidth, startY);
            this.txtQuantity.Size = new System.Drawing.Size(inputWidth, 20);
            this.Controls.Add(this.txtQuantity);

            // --- Buttons (Thao tác) ---
            startY += rowSpacing * 2;
            currentX = 50; 
            
            // Thêm
            this.btnAdd.Location = new System.Drawing.Point(currentX, startY);
            this.btnAdd.Text = "Thêm";
            this.btnAdd.Size = new System.Drawing.Size(buttonWidth, 30);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click); // GẮN SỰ KIỆN
            this.Controls.Add(this.btnAdd);

            // Sửa
            currentX += buttonWidth + 10;
            this.btnUpdate.Location = new System.Drawing.Point(currentX, startY);
            this.btnUpdate.Text = "Sửa";
            this.btnUpdate.Size = new System.Drawing.Size(buttonWidth, 30);
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click); // GẮN SỰ KIỆN
            this.Controls.Add(this.btnUpdate);

            // Xóa
            currentX += buttonWidth + 10;
            this.btnDelete.Location = new System.Drawing.Point(currentX, startY);
            this.btnDelete.Text = "Xóa";
            this.btnDelete.Size = new System.Drawing.Size(buttonWidth, 30);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click); // GẮN SỰ KIỆN
            this.Controls.Add(this.btnDelete);

            // --- Search Area ---
            startY += rowSpacing * 2;
            
            this.txtSearch.Location = new System.Drawing.Point(50, startY);
            this.txtSearch.Size = new System.Drawing.Size(400, 20);
            this.Controls.Add(this.txtSearch);

            this.btnSearch.Location = new System.Drawing.Point(460, startY);
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Size = new System.Drawing.Size(100, 20);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click); // GẮN SỰ KIỆN
            this.Controls.Add(this.btnSearch);

            // --- DataGridView ---
            startY += rowSpacing;
            this.dgvBooks.Location = new System.Drawing.Point(50, startY);
            this.dgvBooks.Size = new System.Drawing.Size(800, 300);
            this.dgvBooks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect; 
            this.dgvBooks.ReadOnly = true; 
            this.dgvBooks.SelectionChanged += new System.EventHandler(this.dgvBooks_SelectionChanged); // GẮN SỰ KIỆN
            
            this.Controls.Add(this.dgvBooks);
        }

        #endregion
    }
}