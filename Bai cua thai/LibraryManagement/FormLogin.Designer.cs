using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement
{
    public partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;

        // KHAI BÁO CÁC BIẾN CONTROL
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 250); // Kích thước nhỏ hơn
            this.Text = "Đăng Nhập Hệ Thống";

            // 1. KHỞI TẠO
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();

            int startY = 40;
            int labelWidth = 80;
            int inputWidth = 200;
            int currentX = 50;
            int rowSpacing = 40;

            // Username
            this.Controls.Add(new System.Windows.Forms.Label { Text = "Tên đăng nhập:", Location = new System.Drawing.Point(currentX, startY), Size = new System.Drawing.Size(labelWidth, 20) });
            this.txtUsername.Location = new System.Drawing.Point(currentX + labelWidth, startY);
            this.txtUsername.Size = new System.Drawing.Size(inputWidth, 20);
            this.Controls.Add(this.txtUsername);

            // Password
            startY += rowSpacing;
            this.Controls.Add(new System.Windows.Forms.Label { Text = "Mật khẩu:", Location = new System.Drawing.Point(currentX, startY), Size = new System.Drawing.Size(labelWidth, 20) });
            this.txtPassword.Location = new System.Drawing.Point(currentX + labelWidth, startY);
            this.txtPassword.Size = new System.Drawing.Size(inputWidth, 20);
            this.txtPassword.PasswordChar = '*'; // Ẩn mật khẩu
            this.Controls.Add(this.txtPassword);

            // Button Login
            startY += rowSpacing + 10;
            this.btnLogin.Location = new System.Drawing.Point(currentX + labelWidth + inputWidth - 100, startY);
            this.btnLogin.Text = "Đăng Nhập";
            this.btnLogin.Size = new System.Drawing.Size(100, 30);
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click); // GẮN SỰ KIỆN
            this.Controls.Add(this.btnLogin);
        }

        #endregion
    }
}