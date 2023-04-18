﻿namespace UGC_App
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
            components = new System.ComponentModel.Container();
            button_Save = new Button();
            button_Design = new Button();
            checkBox_CMDr = new CheckBox();
            checkBox_FullList = new CheckBox();
            checkBox_OnlyBGS = new CheckBox();
            checkBox_AutoUpdate = new CheckBox();
            textBox_Send = new TextBox();
            textBox_State = new TextBox();
            textBox_Token = new TextBox();
            checkBox_SlowState = new CheckBox();
            checkBox_Debug = new CheckBox();
            label_SendUrl = new Label();
            label_StateUrl = new Label();
            label_Token = new Label();
            checkBoxy_AutoStart = new CheckBox();
            numericUpDown_ListCount = new NumericUpDown();
            checkBox_CloseMini = new CheckBox();
            toolTip_Konfig = new ToolTip(components);
            checkBox_RichPresence = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_ListCount).BeginInit();
            SuspendLayout();
            // 
            // button_Save
            // 
            button_Save.Location = new Point(116, 381);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(88, 23);
            button_Save.TabIndex = 0;
            button_Save.Text = "Übernehmen";
            button_Save.UseVisualStyleBackColor = true;
            // 
            // button_Design
            // 
            button_Design.Location = new Point(12, 381);
            button_Design.Name = "button_Design";
            button_Design.Size = new Size(75, 23);
            button_Design.TabIndex = 1;
            button_Design.Text = "Design";
            button_Design.UseVisualStyleBackColor = true;
            // 
            // checkBox_CMDr
            // 
            checkBox_CMDr.AutoSize = true;
            checkBox_CMDr.Location = new Point(12, 129);
            checkBox_CMDr.Name = "checkBox_CMDr";
            checkBox_CMDr.Size = new Size(160, 19);
            checkBox_CMDr.TabIndex = 2;
            checkBox_CMDr.Text = "CMDr Namen übertragen";
            checkBox_CMDr.UseVisualStyleBackColor = true;
            // 
            // checkBox_FullList
            // 
            checkBox_FullList.AutoSize = true;
            checkBox_FullList.Location = new Point(12, 154);
            checkBox_FullList.Name = "checkBox_FullList";
            checkBox_FullList.Size = new Size(136, 19);
            checkBox_FullList.TabIndex = 3;
            checkBox_FullList.Text = "Gesamte Liste zeigen";
            checkBox_FullList.UseVisualStyleBackColor = true;
            // 
            // checkBox_OnlyBGS
            // 
            checkBox_OnlyBGS.AutoSize = true;
            checkBox_OnlyBGS.Location = new Point(12, 179);
            checkBox_OnlyBGS.Name = "checkBox_OnlyBGS";
            checkBox_OnlyBGS.Size = new Size(158, 19);
            checkBox_OnlyBGS.TabIndex = 4;
            checkBox_OnlyBGS.Text = "Nur BGS relevante zeigen";
            checkBox_OnlyBGS.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoUpdate
            // 
            checkBox_AutoUpdate.AutoSize = true;
            checkBox_AutoUpdate.Location = new Point(12, 204);
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
            checkBox_SlowState.Location = new Point(12, 229);
            checkBox_SlowState.Name = "checkBox_SlowState";
            checkBox_SlowState.Size = new Size(80, 19);
            checkBox_SlowState.TabIndex = 9;
            checkBox_SlowState.Text = "Slow State";
            checkBox_SlowState.UseVisualStyleBackColor = true;
            // 
            // checkBox_Debug
            // 
            checkBox_Debug.AutoSize = true;
            checkBox_Debug.Location = new Point(12, 305);
            checkBox_Debug.Name = "checkBox_Debug";
            checkBox_Debug.Size = new Size(61, 19);
            checkBox_Debug.TabIndex = 10;
            checkBox_Debug.Text = "Debug";
            checkBox_Debug.UseVisualStyleBackColor = true;
            // 
            // label_SendUrl
            // 
            label_SendUrl.AutoSize = true;
            label_SendUrl.Location = new Point(12, 35);
            label_SendUrl.Name = "label_SendUrl";
            label_SendUrl.Size = new Size(57, 15);
            label_SendUrl.TabIndex = 11;
            label_SendUrl.Text = "Send URL";
            // 
            // label_StateUrl
            // 
            label_StateUrl.AutoSize = true;
            label_StateUrl.Location = new Point(12, 64);
            label_StateUrl.Name = "label_StateUrl";
            label_StateUrl.Size = new Size(57, 15);
            label_StateUrl.TabIndex = 12;
            label_StateUrl.Text = "State URL";
            // 
            // label_Token
            // 
            label_Token.AutoSize = true;
            label_Token.Location = new Point(12, 93);
            label_Token.Name = "label_Token";
            label_Token.Size = new Size(38, 15);
            label_Token.TabIndex = 13;
            label_Token.Text = "Token";
            // 
            // checkBoxy_AutoStart
            // 
            checkBoxy_AutoStart.AutoSize = true;
            checkBoxy_AutoStart.Location = new Point(12, 254);
            checkBoxy_AutoStart.Name = "checkBoxy_AutoStart";
            checkBoxy_AutoStart.Size = new Size(136, 19);
            checkBoxy_AutoStart.TabIndex = 14;
            checkBoxy_AutoStart.Text = "mit Windows Starten";
            checkBoxy_AutoStart.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_ListCount
            // 
            numericUpDown_ListCount.Location = new Point(168, 153);
            numericUpDown_ListCount.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_ListCount.Name = "numericUpDown_ListCount";
            numericUpDown_ListCount.Size = new Size(36, 23);
            numericUpDown_ListCount.TabIndex = 15;
            numericUpDown_ListCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // checkBox_CloseMini
            // 
            checkBox_CloseMini.AutoSize = true;
            checkBox_CloseMini.Location = new Point(12, 280);
            checkBox_CloseMini.Name = "checkBox_CloseMini";
            checkBox_CloseMini.Size = new Size(153, 19);
            checkBox_CloseMini.TabIndex = 16;
            checkBox_CloseMini.Text = "in Infobereich Schließen";
            checkBox_CloseMini.UseVisualStyleBackColor = true;
            // 
            // checkBox_RichPresence
            // 
            checkBox_RichPresence.AutoSize = true;
            checkBox_RichPresence.Location = new Point(12, 330);
            checkBox_RichPresence.Name = "checkBox_RichPresence";
            checkBox_RichPresence.Size = new Size(96, 19);
            checkBox_RichPresence.TabIndex = 17;
            checkBox_RichPresence.Text = "RichPresence";
            checkBox_RichPresence.UseVisualStyleBackColor = true;
            // 
            // Konfiguration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(292, 416);
            Controls.Add(checkBox_RichPresence);
            Controls.Add(checkBox_CloseMini);
            Controls.Add(numericUpDown_ListCount);
            Controls.Add(checkBoxy_AutoStart);
            Controls.Add(label_Token);
            Controls.Add(label_StateUrl);
            Controls.Add(label_SendUrl);
            Controls.Add(checkBox_Debug);
            Controls.Add(checkBox_SlowState);
            Controls.Add(textBox_Token);
            Controls.Add(textBox_State);
            Controls.Add(textBox_Send);
            Controls.Add(checkBox_AutoUpdate);
            Controls.Add(checkBox_OnlyBGS);
            Controls.Add(checkBox_FullList);
            Controls.Add(checkBox_CMDr);
            Controls.Add(button_Design);
            Controls.Add(button_Save);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "Konfiguration";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Einstellungen";
            ((System.ComponentModel.ISupportInitialize)numericUpDown_ListCount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_Save;
        private Button button_Design;
        private CheckBox checkBox_CMDr;
        private CheckBox checkBox_FullList;
        private CheckBox checkBox_OnlyBGS;
        private CheckBox checkBox_AutoUpdate;
        private TextBox textBox_Send;
        private TextBox textBox_State;
        private TextBox textBox_Token;
        private CheckBox checkBox_SlowState;
        private CheckBox checkBox_Debug;
        private Label label_SendUrl;
        private Label label_StateUrl;
        private Label label_Token;
        private CheckBox checkBoxy_AutoStart;
        private NumericUpDown numericUpDown_ListCount;
        private CheckBox checkBox_CloseMini;
        private ToolTip toolTip_Konfig;
        private CheckBox checkBox_RichPresence;
    }
}