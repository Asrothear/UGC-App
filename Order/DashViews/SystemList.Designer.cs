namespace UGC_App.Order.DashViews
{
    partial class SystemList
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView_SystemList = new DataGridView();
            menuStrip1 = new MenuStrip();
            seinToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridView_SystemList).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView_SystemList
            // 
            dataGridView_SystemList.AllowUserToAddRows = false;
            dataGridView_SystemList.AllowUserToDeleteRows = false;
            dataGridView_SystemList.AllowUserToResizeColumns = false;
            dataGridView_SystemList.AllowUserToResizeRows = false;
            dataGridView_SystemList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_SystemList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_SystemList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_SystemList.Dock = DockStyle.Fill;
            dataGridView_SystemList.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_SystemList.Location = new Point(0, 24);
            dataGridView_SystemList.MultiSelect = false;
            dataGridView_SystemList.Name = "dataGridView_SystemList";
            dataGridView_SystemList.RowTemplate.Height = 25;
            dataGridView_SystemList.ScrollBars = ScrollBars.Vertical;
            dataGridView_SystemList.Size = new Size(500, 587);
            dataGridView_SystemList.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { seinToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(500, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // seinToolStripMenuItem
            // 
            seinToolStripMenuItem.Name = "seinToolStripMenuItem";
            seinToolStripMenuItem.Size = new Size(58, 20);
            seinToolStripMenuItem.Text = "Refresh";
            // 
            // SystemList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(dataGridView_SystemList);
            Controls.Add(menuStrip1);
            MinimumSize = new Size(500, 500);
            Name = "SystemList";
            Size = new Size(500, 611);
            ((System.ComponentModel.ISupportInitialize)dataGridView_SystemList).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        internal DataGridView dataGridView_SystemList;
        internal MenuStrip menuStrip1;
        private ToolStripMenuItem seinToolStripMenuItem;
    }
}
