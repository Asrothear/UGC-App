namespace UGC_App.Order
{
    partial class OrderEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderEditor));
            label1 = new Label();
            button_abort = new Button();
            numericUpDown_Prio = new NumericUpDown();
            comboBox_Type = new ComboBox();
            textBox_Orders = new TextBox();
            label2 = new Label();
            button_Save = new Button();
            label3 = new Label();
            button_Remove = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Prio).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(189, 63);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 0;
            label1.Text = "Priority";
            // 
            // button_abort
            // 
            button_abort.Location = new Point(95, 146);
            button_abort.Name = "button_abort";
            button_abort.Size = new Size(75, 23);
            button_abort.TabIndex = 4;
            button_abort.Text = "Abbruch";
            button_abort.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_Prio
            // 
            numericUpDown_Prio.Location = new Point(240, 61);
            numericUpDown_Prio.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            numericUpDown_Prio.Name = "numericUpDown_Prio";
            numericUpDown_Prio.Size = new Size(33, 23);
            numericUpDown_Prio.TabIndex = 2;
            // 
            // comboBox_Type
            // 
            comboBox_Type.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox_Type.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_Type.FormattingEnabled = true;
            comboBox_Type.Items.AddRange(new object[] { "Unterstützung", "Wahlen", "Rückzug verhindern", "Konfliktzonen", "Thargoiden", "Spezial" });
            comboBox_Type.Location = new Point(49, 60);
            comboBox_Type.Name = "comboBox_Type";
            comboBox_Type.Size = new Size(121, 23);
            comboBox_Type.TabIndex = 1;
            // 
            // textBox_Orders
            // 
            textBox_Orders.Location = new Point(12, 89);
            textBox_Orders.Multiline = true;
            textBox_Orders.Name = "textBox_Orders";
            textBox_Orders.Size = new Size(261, 51);
            textBox_Orders.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 63);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 5;
            label2.Text = "Type";
            // 
            // button_Save
            // 
            button_Save.Location = new Point(187, 146);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(86, 23);
            button_Save.TabIndex = 5;
            button_Save.Text = "Übernehmen";
            button_Save.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(115, 25);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 7;
            label3.Text = "Title";
            // 
            // button_Remove
            // 
            button_Remove.Location = new Point(12, 146);
            button_Remove.Name = "button_Remove";
            button_Remove.Size = new Size(40, 23);
            button_Remove.TabIndex = 8;
            button_Remove.TabStop = false;
            button_Remove.Text = "Entf";
            button_Remove.UseVisualStyleBackColor = true;
            // 
            // OrderEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(285, 181);
            Controls.Add(button_Remove);
            Controls.Add(label3);
            Controls.Add(button_Save);
            Controls.Add(label2);
            Controls.Add(textBox_Orders);
            Controls.Add(comboBox_Type);
            Controls.Add(numericUpDown_Prio);
            Controls.Add(button_abort);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OrderEditor";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "OrderEditor";
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Prio).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button_abort;
        private NumericUpDown numericUpDown_Prio;
        private ComboBox comboBox_Type;
        private TextBox textBox_Orders;
        private Label label2;
        private Button button_Save;
        private Label label3;
        private Button button_Remove;
    }
}