using SimsAppJournal.Forms;
using System;
using System.Windows.Forms;

namespace SimsAppJournal
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // THIS MUST BE Application.Run to keep the app alive
                    Application.Run(new DashboardForm());
                }
            }
        }
    }
}


