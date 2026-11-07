using Inventory_Management_System.DataAccess;
using System;
using System.Windows.Forms;
using Inventory_Management_System.DataAccess;
using Inventory_Management_System.Models;

namespace Inventory_Management_System
{
    public partial class AddProductForm : BaseProductForm
    {
        private readonly ProductRepository _productRepository;

        public AddProductForm()
        {
            InitializeComponent();

            _productRepository = new ProductRepository();

            btnSave.Click += btnSave_Click;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateProductData())
                return;

            try
            {
                Product product = GetProductFromForm();

                int newProductId =
                    _productRepository.Add(product);

                MessageBox.Show(
                    "Product added successfully!\nProduct ID: "
                    + newProductId,
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to add product: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

    }
}
