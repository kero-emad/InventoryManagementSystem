using Inventory_Management_System.DataAccess;
using Inventory_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class BaseProductForm : Form
    {
        protected TextBox txtProductName;
        protected TextBox txtDescription;
        protected TextBox txtQuantity;
        protected TextBox txtMinimumStockLevel;
        protected TextBox txtPrice;
        protected ComboBox cmbCategory;
        protected ComboBox cmbSupplier;
        protected Button btnCancel;

        private readonly CategoryRepository _categoryRepository;
        private readonly SupplierRepository _supplierRepository;

        protected BaseProductForm()
        {
            _categoryRepository = new CategoryRepository();
            _supplierRepository = new SupplierRepository();

            InitializeComponent();
            LoadCategories();
            LoadSuppliers();
        }

        protected void LoadCategories()
        {
            try
            {
                List<Category> categories = _categoryRepository.GetAll();
                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load categories: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        protected void LoadSuppliers()
        {
            try
            {
                List<Supplier> suppliers = _supplierRepository.GetAll();
                cmbSupplier.DataSource = suppliers;
                cmbSupplier.DisplayMember = "SupplierName";
                cmbSupplier.ValueMember = "SupplierId";
                cmbSupplier.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load suppliers: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        protected bool ValidateProductData()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Product name is required.");
                txtProductName.Focus();
                return false;
            }

            if (!int.TryParse(txtMinimumStockLevel.Text, out int minimumStockLevel)
                || minimumStockLevel < 0)
            {
                MessageBox.Show(
                    "Please enter a valid non-negative minimum stock level.");
                txtMinimumStockLevel.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid non-negative quantity.");
                txtQuantity.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Please enter a valid non-negative price.");
                txtPrice.Focus();
                return false;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Please select a category.");
                cmbCategory.Focus();
                return false;
            }

            if (cmbSupplier.SelectedValue == null)
            {
                MessageBox.Show("Please select a supplier.");
                cmbSupplier.Focus();
                return false;
            }

            return true;
        }

        protected Product GetProductFromForm()
        {
            return new Product
            {
                ProductName = txtProductName.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text)
                    ? null
                    : txtDescription.Text.Trim(),
                QuantityInStock = int.Parse(txtQuantity.Text),
                MinimumStockLevel = int.Parse(txtMinimumStockLevel.Text),
                Price = decimal.Parse(txtPrice.Text),
                CategoryId = Convert.ToInt32(cmbCategory.SelectedValue),
                SupplierId = Convert.ToInt32(cmbSupplier.SelectedValue)
            };
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
