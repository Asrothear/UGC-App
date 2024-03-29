﻿namespace UGC_App.Order.DashViews
{
    partial class OrderList
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridView_OrderList = new DataGridView();
            menuStrip1 = new MenuStrip();
            contextMenuStrip_Options = new ContextMenuStrip(components);
            bearbeitenToolStripMenuItem = new ToolStripMenuItem();
            systemEditorÖffnenToolStripMenuItem = new ToolStripMenuItem();
            löschenToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridView_OrderList).BeginInit();
            contextMenuStrip_Options.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView_OrderList
            // 
            dataGridView_OrderList.AllowUserToAddRows = false;
            dataGridView_OrderList.AllowUserToDeleteRows = false;
            dataGridView_OrderList.AllowUserToResizeColumns = false;
            dataGridView_OrderList.AllowUserToResizeRows = false;
            dataGridView_OrderList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView_OrderList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_OrderList.BackgroundColor = SystemColors.Control;
            dataGridView_OrderList.BorderStyle = BorderStyle.None;
            dataGridView_OrderList.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dataGridView_OrderList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.Desktop;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView_OrderList.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView_OrderList.Dock = DockStyle.Fill;
            dataGridView_OrderList.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_OrderList.Location = new Point(0, 24);
            dataGridView_OrderList.MinimumSize = new Size(300, 200);
            dataGridView_OrderList.MultiSelect = false;
            dataGridView_OrderList.Name = "dataGridView_OrderList";
            dataGridView_OrderList.RowTemplate.Height = 25;
            dataGridView_OrderList.ScrollBars = ScrollBars.Vertical;
            dataGridView_OrderList.ShowCellErrors = false;
            dataGridView_OrderList.ShowCellToolTips = false;
            dataGridView_OrderList.ShowEditingIcon = false;
            dataGridView_OrderList.ShowRowErrors = false;
            dataGridView_OrderList.Size = new Size(300, 476);
            dataGridView_OrderList.TabIndex = 2;
            // 
            // menuStrip1
            // 
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(300, 24);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // contextMenuStrip_Options
            // 
            contextMenuStrip_Options.Items.AddRange(new ToolStripItem[] { bearbeitenToolStripMenuItem, systemEditorÖffnenToolStripMenuItem,löschenToolStripMenuItem });
            contextMenuStrip_Options.Name = "contextMenuStrip_Options";
            contextMenuStrip_Options.Size = new Size(182, 48);
            // 
            // löschenToolStripMenuItem
            // 
            löschenToolStripMenuItem.Name = "löschenToolStripMenuItem";
            löschenToolStripMenuItem.Size = new Size(181, 22);
            löschenToolStripMenuItem.Text = "Löschen";
            // 
            // bearbeitenToolStripMenuItem
            // 
            bearbeitenToolStripMenuItem.Name = "bearbeitenToolStripMenuItem";
            bearbeitenToolStripMenuItem.Size = new Size(181, 22);
            bearbeitenToolStripMenuItem.Text = "Bearbeiten";
            // 
            // systemEditorÖffnenToolStripMenuItem
            // 
            systemEditorÖffnenToolStripMenuItem.Name = "systemEditorÖffnenToolStripMenuItem";
            systemEditorÖffnenToolStripMenuItem.Size = new Size(181, 22);
            systemEditorÖffnenToolStripMenuItem.Text = "SystemEditor öffnen";
            // 
            // OrderList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(dataGridView_OrderList);
            Controls.Add(menuStrip1);
            MaximumSize = new Size(0, 1000);
            MinimumSize = new Size(300, 500);
            Name = "OrderList";
            Size = new Size(300, 500);
            ((System.ComponentModel.ISupportInitialize)dataGridView_OrderList).EndInit();
            contextMenuStrip_Options.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        internal DataGridView dataGridView_OrderList;
        private MenuStrip menuStrip1;
        private ContextMenuStrip contextMenuStrip_Options;
        private ToolStripMenuItem löschenToolStripMenuItem;
        private ToolStripMenuItem bearbeitenToolStripMenuItem;
        private ToolStripMenuItem systemEditorÖffnenToolStripMenuItem;
    }
}
