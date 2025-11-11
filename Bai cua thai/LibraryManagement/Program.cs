using System;
using System.Windows.Forms;

namespace LibraryManagement
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Form khởi động — bạn có thể đổi FormLogin thành FormBook tùy ý
            Application.Run(new FormLogin());
        }
    }
}
