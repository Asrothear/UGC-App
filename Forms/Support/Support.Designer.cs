namespace UGC_App.Forms.Support
{
    partial class Support
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Support));
            label11 = new Label();
            textBox_Support = new TextBox();
            button_Report_Senden = new Button();
            label12 = new Label();
            button_Save = new Button();
            SuspendLayout();
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(12, 9);
            label11.Name = "label11";
            label11.Size = new Size(87, 15);
            label11.TabIndex = 7;
            label11.Text = "Report Senden:";
            // 
            // textBox_Support
            // 
            textBox_Support.AcceptsTab = true;
            textBox_Support.HideSelection = false;
            textBox_Support.Location = new Point(12, 27);
            textBox_Support.Multiline = true;
            textBox_Support.Name = "textBox_Support";
            textBox_Support.ScrollBars = ScrollBars.Horizontal;
            textBox_Support.Size = new Size(318, 148);
            textBox_Support.TabIndex = 6;
            // 
            // button_Report_Senden
            // 
            button_Report_Senden.Location = new Point(255, 322);
            button_Report_Senden.Name = "button_Report_Senden";
            button_Report_Senden.Size = new Size(75, 23);
            button_Report_Senden.TabIndex = 4;
            button_Report_Senden.Text = "Senden";
            button_Report_Senden.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            label12.Location = new Point(14, 181);
            label12.Name = "label12";
            label12.Size = new Size(316, 164);
            label12.TabIndex = 8;
            label12.Text = resources.GetString("label12.Text");
            // 
            // button_Save
            // 
            button_Save.Location = new Point(124, 426);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(88, 23);
            button_Save.TabIndex = 5;
            button_Save.Text = "Speichern";
            button_Save.UseVisualStyleBackColor = true;
            // 
            // Support
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 450);
            Controls.Add(label11);
            Controls.Add(textBox_Support);
            Controls.Add(button_Report_Senden);
            Controls.Add(label12);
            Controls.Add(button_Save);
            Name = "Support";
            Text = "Support";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label11;
        private TextBox textBox_Support;
        private Button button_Report_Senden;
        private Label label12;
        private Button button_Save;
    }
}