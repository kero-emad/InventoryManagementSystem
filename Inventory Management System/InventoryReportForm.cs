using CrystalDecisions.CrystalReports.Engine;
using Inventory_Management_System.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class InventoryReportForm : Form
    {
        private readonly ProductRepository _productRepository;
        private ReportDocument _reportDocument;
        public InventoryReportForm()
        {
            InitializeComponent();
            _productRepository = new ProductRepository();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_reportDocument != null)
            {
                _reportDocument.Close();
                _reportDocument.Dispose();
            }

            base.OnFormClosed(e);
        }


        private void InventoryReportForm_Load(object sender, EventArgs e)
        {
            try
            {
                var products = _productRepository.GetAllProductsForReport();
                _reportDocument = new ReportDocument();
                string reportPath = System.IO.Path.Combine(
                    Application.StartupPath,
                    "InventoryReport.rpt");

                _reportDocument.Load(reportPath);

                _reportDocument.SetDataSource(products);

                crystalReportViewer1.ReportSource = _reportDocument;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }

}
