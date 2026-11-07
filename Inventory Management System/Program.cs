using Inventory_Management_System.DataAccess;
using Inventory_Management_System.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UserRepository userRepository = new UserRepository();

            if (!userRepository.HasAdmin())
            {
                using (SetupAdminForm setupForm = new SetupAdminForm())
                {
                    if (setupForm.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            Application.Run(new LoginForm());

            // Application.Run(new Form1());
            /*
            try
            {
                bool isConnected =
                    DatabaseInitializer.TestConnection();

                MessageBox.Show(
                    isConnected
                        ? "Database Connected Successfully!"
                        : "Database Connection Failed!",
                    "Connection Test",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            */
        }
    }
}
