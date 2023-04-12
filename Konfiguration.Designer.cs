namespace UGC_App
{
    partial class Konfiguration
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
            button_Save = new Button();
            button2 = new Button();
            checkBox_CMDr = new CheckBox();
            checkBox_FullList = new CheckBox();
            checkBox_OnlyBGS = new CheckBox();
            checkBox_AutoUpdate = new CheckBox();
            textBox_Send = new TextBox();
            textBox_State = new TextBox();
            textBox_Token = new TextBox();
            checkBox_SlowState = new CheckBox();
            checkBox_Debug = new CheckBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // button_Save
            // 
            button_Save.Location = new Point(116, 340);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(88, 23);
            button_Save.TabIndex = 0;
            button_Save.Text = "Übernehmen";
            button_Save.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(12, 340);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 1;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // checkBox_CMDr
            // 
            checkBox_CMDr.AutoSize = true;
            checkBox_CMDr.Location = new Point(12, 170);
            checkBox_CMDr.Name = "checkBox_CMDr";
            checkBox_CMDr.Size = new Size(160, 19);
            checkBox_CMDr.TabIndex = 2;
            checkBox_CMDr.Text = "CMDr Namen übertragen";
            checkBox_CMDr.UseVisualStyleBackColor = true;
            // 
            // checkBox_FullList
            // 
            checkBox_FullList.AutoSize = true;
            checkBox_FullList.Location = new Point(12, 195);
            checkBox_FullList.Name = "checkBox_FullList";
            checkBox_FullList.Size = new Size(136, 19);
            checkBox_FullList.TabIndex = 3;
            checkBox_FullList.Text = "Gesamte Liste zeigen";
            checkBox_FullList.UseVisualStyleBackColor = true;
            // 
            // checkBox_OnlyBGS
            // 
            checkBox_OnlyBGS.AutoSize = true;
            checkBox_OnlyBGS.Location = new Point(12, 220);
            checkBox_OnlyBGS.Name = "checkBox_OnlyBGS";
            checkBox_OnlyBGS.Size = new Size(158, 19);
            checkBox_OnlyBGS.TabIndex = 4;
            checkBox_OnlyBGS.Text = "Nur BGS relevante zeigen";
            checkBox_OnlyBGS.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoUpdate
            // 
            checkBox_AutoUpdate.AutoSize = true;
            checkBox_AutoUpdate.Location = new Point(12, 245);
            checkBox_AutoUpdate.Name = "checkBox_AutoUpdate";
            checkBox_AutoUpdate.Size = new Size(93, 19);
            checkBox_AutoUpdate.TabIndex = 5;
            checkBox_AutoUpdate.Text = "Auto Update";
            checkBox_AutoUpdate.UseVisualStyleBackColor = true;
            // 
            // textBox_Send
            // 
            textBox_Send.Location = new Point(75, 32);
            textBox_Send.Name = "textBox_Send";
            textBox_Send.Size = new Size(180, 23);
            textBox_Send.TabIndex = 6;
            // 
            // textBox_State
            // 
            textBox_State.Location = new Point(75, 61);
            textBox_State.Name = "textBox_State";
            textBox_State.Size = new Size(180, 23);
            textBox_State.TabIndex = 7;
            // 
            // textBox_Token
            // 
            textBox_Token.Location = new Point(75, 90);
            textBox_Token.Name = "textBox_Token";
            textBox_Token.Size = new Size(180, 23);
            textBox_Token.TabIndex = 8;
            // 
            // checkBox_SlowState
            // 
            checkBox_SlowState.AutoSize = true;
            checkBox_SlowState.Location = new Point(12, 270);
            checkBox_SlowState.Name = "checkBox_SlowState";
            checkBox_SlowState.Size = new Size(80, 19);
            checkBox_SlowState.TabIndex = 9;
            checkBox_SlowState.Text = "Slow State";
            checkBox_SlowState.UseVisualStyleBackColor = true;
            // 
            // checkBox_Debug
            // 
            checkBox_Debug.AutoSize = true;
            checkBox_Debug.Location = new Point(12, 295);
            checkBox_Debug.Name = "checkBox_Debug";
            checkBox_Debug.Size = new Size(61, 19);
            checkBox_Debug.TabIndex = 10;
            checkBox_Debug.Text = "Debug";
            checkBox_Debug.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 35);
            label1.Name = "label1";
            label1.Size = new Size(57, 15);
            label1.TabIndex = 11;
            label1.Text = "Send URL";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 64);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 12;
            label2.Text = "State URL";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 93);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 13;
            label3.Text = "Token";
            // 
            // Konfiguration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(292, 375);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(checkBox_Debug);
            Controls.Add(checkBox_SlowState);
            Controls.Add(textBox_Token);
            Controls.Add(textBox_State);
            Controls.Add(textBox_Send);
            Controls.Add(checkBox_AutoUpdate);
            Controls.Add(checkBox_OnlyBGS);
            Controls.Add(checkBox_FullList);
            Controls.Add(checkBox_CMDr);
            Controls.Add(button2);
            Controls.Add(button_Save);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "Konfiguration";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Konfiguration";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_Save;
        private Button button2;
        private CheckBox checkBox_CMDr;
        private CheckBox checkBox_FullList;
        private CheckBox checkBox_OnlyBGS;
        private CheckBox checkBox_AutoUpdate;
        private TextBox textBox_Send;
        private TextBox textBox_State;
        private TextBox textBox_Token;
        private CheckBox checkBox_SlowState;
        private CheckBox checkBox_Debug;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}