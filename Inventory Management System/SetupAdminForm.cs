using Inventory_Management_System.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class SetupAdminForm : Form
    {
        private readonly UserRepository _userRepository;
        public SetupAdminForm()
        {
            InitializeComponent();
            _userRepository = new UserRepository();
        }

        private void btnCreateAdmin_Click(object sender, EventArgs e)
        {
             string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                bool isCreated = _userRepository.CreateUser(
                    username,
                    password,
                    "Admin");

                if (isCreated)
                {
                    MessageBox.Show("Admin account created successfully!");

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("This username already exists.");
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
