using Inventory_Management_System.DataAccess;
using Inventory_Management_System.Models;
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
    public partial class ProductsForm : Form
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        private List<Product> _allProducts;

        public ProductsForm()
        {
            InitializeComponent();
            _productRepository = new ProductRepository();
            _categoryRepository = new CategoryRepository();

            LoadCategoryFilter();
            cmbStockFilter.Items.Add("All");
            cmbStockFilter.Items.Add("Low Stock");
            cmbStockFilter.Items.Add("In Stock");
            cmbStockFilter.SelectedIndex = 0;

            btnAdd.Visible = AuthSession.IsAdmin;
            btnUserManagement.Visible = AuthSession.IsAdmin;

            txtSearch.TextChanged += FilterControlsChanged;
            cmbCategoryFilter.SelectedIndexChanged += FilterControlsChanged;
            cmbStockFilter.SelectedIndexChanged += FilterControlsChanged;

            LoadProducts();
            btnAdd.Click += btnAdd_Click;
            dgvProducts.CellContentClick += dgvProducts_CellContentClick;
        }
        private void LoadProducts()
        {
            try
            {
                List<Product> products =
                    _productRepository.GetAll();

                _allProducts = products;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load products: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadCategoryFilter()
        {
            try
            {
                List<Category> categories = _categoryRepository.GetAll();
                categories.Insert(0, new Category
                {
                    CategoryId = 0,
                    CategoryName = "All"
                });

                cmbCategoryFilter.DataSource = categories;
                cmbCategoryFilter.DisplayMember = "CategoryName";
                cmbCategoryFilter.ValueMember = "CategoryId";
                cmbCategoryFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load category filter: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FilterControlsChanged(object sender, EventArgs e)
        {
            if (_allProducts != null)
                ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = txtSearch.Text.Trim();
            int selectedCategoryId = cmbCategoryFilter.SelectedValue == null
                ? 0
                : Convert.ToInt32(cmbCategoryFilter.SelectedValue);
            string stockFilter = cmbStockFilter.SelectedItem == null
                ? "All"
                : cmbStockFilter.SelectedItem.ToString();

            IEnumerable<Product> filteredProducts = _allProducts;

            if (searchText.Length > 0)
            {
                filteredProducts = filteredProducts.Where(product =>
                    product.ProductName != null
                    && product.ProductName.IndexOf(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase) >= 0);
            }

            if (selectedCategoryId > 0)
            {
                filteredProducts = filteredProducts.Where(product =>
                    product.CategoryId == selectedCategoryId);
            }

            if (stockFilter == "Low Stock")
            {
                filteredProducts = filteredProducts.Where(product =>
                    product.QuantityInStock < product.MinimumStockLevel);
            }
            else if (stockFilter == "In Stock")
            {
                filteredProducts = filteredProducts.Where(product =>
                    product.QuantityInStock >= product.MinimumStockLevel);
            }

            dgvProducts.DataSource = filteredProducts.ToList();
            ConfigureProductColumns();
            AddActionColumns();
        }

        private void ConfigureProductColumns()
        {
            if (dgvProducts.Columns["CategoryId"] != null)
                dgvProducts.Columns["CategoryId"].Visible = false;

            if (dgvProducts.Columns["SupplierId"] != null)
                dgvProducts.Columns["SupplierId"].Visible = false;

            if (dgvProducts.Columns["CategoryName"] != null)
                dgvProducts.Columns["CategoryName"].HeaderText = "Category";

            if (dgvProducts.Columns["SupplierName"] != null)
                dgvProducts.Columns["SupplierName"].HeaderText = "Supplier";
        }

        private void AddActionColumns()
        {
            if (!AuthSession.IsAdmin)
                return;

            if (dgvProducts.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn editColumn =
                    new DataGridViewButtonColumn
                    {
                        Name = "Edit",
                        HeaderText = "Edit",
                        Text = "Edit",
                        UseColumnTextForButtonValue = true
                    };

                dgvProducts.Columns.Add(editColumn);
            }

            if (dgvProducts.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn deleteColumn =
                    new DataGridViewButtonColumn
                    {
                        Name = "Delete",
                        HeaderText = "Delete",
                        Text = "Delete",
                        UseColumnTextForButtonValue = true
                    };

                dgvProducts.Columns.Add(deleteColumn);
            }
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            if (!AuthSession.IsAdmin)
                return;

            using (UserManagementForm userManagementForm =
                   new UserManagementForm())
            {
                userManagementForm.ShowDialog();
            }
        }

        private void dgvProducts_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            Product product = dgvProducts.Rows[e.RowIndex].DataBoundItem as Product;
            if (product == null)
                return;

            if (dgvProducts.Columns[e.ColumnIndex].Name == "Edit")
            {
                using (EditProductForm editForm =
                       new EditProductForm(product))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                        LoadProducts();
                }
            }
            else if (dgvProducts.Columns[e.ColumnIndex].Name == "Delete")
            {
                DeleteProduct(product);
            }
        }

        private void DeleteProduct(Product product)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this product?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                if (_productRepository.Delete(product.ProductId))
                {
                    MessageBox.Show(
                        "Product deleted successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to delete product: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (AddProductForm addForm = new AddProductForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void btnInventoryReport_Click(object sender, EventArgs e)
        {
            using (InventoryReportForm reportForm = new InventoryReportForm())
            {
                reportForm.ShowDialog();
            }
        }
    }
}
