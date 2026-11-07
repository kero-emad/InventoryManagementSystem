namespace Inventory_Management_System
{
    partial class EditProductForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnSave = new System.Windows.Forms.Button();
            SuspendLayout();

            btnSave.Location = new System.Drawing.Point(235, 350);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(100, 32);
            btnSave.TabIndex = 12;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSave.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            btnSave.ForeColor = System.Drawing.Color.White;
            Controls.Add(btnSave);

            Name = "EditProductForm";
            Text = "Edit Product";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
