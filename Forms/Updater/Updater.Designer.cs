namespace UGC_App
{
    partial class Updater
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
            label_InfoText = new Label();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // label_InfoText
            // 
            label_InfoText.AutoSize = true;
            label_InfoText.Location = new Point(148, 38);
            label_InfoText.Name = "label_InfoText";
            label_InfoText.Size = new Size(38, 15);
            label_InfoText.TabIndex = 0;
            label_InfoText.Text = "label1";
            // 
            // button1
            // 
            button1.Location = new Point(65, 88);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Ja";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(190, 88);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // Updater
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(336, 123);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label_InfoText);
            Name = "Updater";
            Text = "Updater";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_InfoText;
        private Button button1;
        private Button button2;
    }
}