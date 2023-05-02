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
            components = new System.ComponentModel.Container();
            button_Save = new Button();
            button_Design = new Button();
            toolTip_Konfig = new ToolTip(components);
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            checkBox_CloseMini = new CheckBox();
            numericUpDown_ListCount = new NumericUpDown();
            checkBoxy_AutoStart = new CheckBox();
            label_Token = new Label();
            label_StateUrl = new Label();
            label_SendUrl = new Label();
            checkBox_Debug = new CheckBox();
            checkBox_SlowState = new CheckBox();
            textBox_Token = new TextBox();
            textBox_State = new TextBox();
            textBox_Send = new TextBox();
            checkBox_AutoUpdate = new CheckBox();
            checkBox_OnlyBGS = new CheckBox();
            checkBox_FullList = new CheckBox();
            checkBox_CMDr = new CheckBox();
            tabPage2 = new TabPage();
            label_Disclaimer = new Label();
            groupBox_Overlay = new GroupBox();
            ColorPick_Overlay_Tick_Light = new PictureBox();
            ColorPick_Overlay_Systeme_Light = new PictureBox();
            label5 = new Label();
            label6 = new Label();
            label8 = new Label();
            ColorPick_Overlay_Systeme_Dark = new PictureBox();
            ColorPick_Overlay_Tick_Dark = new PictureBox();
            ColorPick_Overlay_Background = new PictureBox();
            groupBox_MainFrame = new GroupBox();
            button_ResetToDark = new Button();
            button_ResetToLight = new Button();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            ColorPick_MainFrame_Systeme = new PictureBox();
            ColorPick_MainFrame_Tick = new PictureBox();
            ColorPick_MainFrame_Infos = new PictureBox();
            ColorPick_MainFrame_Background = new PictureBox();
            checkBox_Override = new CheckBox();
            radioButton_Custom = new RadioButton();
            radioButton_Dark = new RadioButton();
            radioButton_Light = new RadioButton();
            checkBox_AlwaysTop = new CheckBox();
            colorDialog1 = new ColorDialog();
            checkBox_EDDN = new CheckBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_ListCount).BeginInit();
            tabPage2.SuspendLayout();
            groupBox_Overlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Tick_Light).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Systeme_Light).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Systeme_Dark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Tick_Dark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Background).BeginInit();
            groupBox_MainFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ColorPick_MainFrame_Systeme).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_MainFrame_Tick).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_MainFrame_Infos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_MainFrame_Background).BeginInit();
            SuspendLayout();
            // 
            // button_Save
            // 
            button_Save.Location = new Point(118, 381);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(88, 23);
            button_Save.TabIndex = 0;
            button_Save.Text = "Übernehmen";
            button_Save.UseVisualStyleBackColor = true;
            // 
            // button_Design
            // 
            button_Design.Location = new Point(9, 317);
            button_Design.Name = "button_Design";
            button_Design.Size = new Size(98, 23);
            button_Design.TabIndex = 1;
            button_Design.Text = "Legacy Design";
            button_Design.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(0, 1);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(340, 374);
            tabControl1.SizeMode = TabSizeMode.FillToRight;
            tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(checkBox_EDDN);
            tabPage1.Controls.Add(checkBox_CloseMini);
            tabPage1.Controls.Add(numericUpDown_ListCount);
            tabPage1.Controls.Add(checkBoxy_AutoStart);
            tabPage1.Controls.Add(label_Token);
            tabPage1.Controls.Add(label_StateUrl);
            tabPage1.Controls.Add(label_SendUrl);
            tabPage1.Controls.Add(checkBox_Debug);
            tabPage1.Controls.Add(checkBox_SlowState);
            tabPage1.Controls.Add(textBox_Token);
            tabPage1.Controls.Add(textBox_State);
            tabPage1.Controls.Add(textBox_Send);
            tabPage1.Controls.Add(checkBox_AutoUpdate);
            tabPage1.Controls.Add(checkBox_OnlyBGS);
            tabPage1.Controls.Add(checkBox_FullList);
            tabPage1.Controls.Add(checkBox_CMDr);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(332, 346);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Main";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox_CloseMini
            // 
            checkBox_CloseMini.AutoSize = true;
            checkBox_CloseMini.Location = new Point(7, 279);
            checkBox_CloseMini.Name = "checkBox_CloseMini";
            checkBox_CloseMini.Size = new Size(153, 19);
            checkBox_CloseMini.TabIndex = 32;
            checkBox_CloseMini.Text = "in Infobereich Schließen";
            checkBox_CloseMini.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_ListCount
            // 
            numericUpDown_ListCount.Location = new Point(163, 152);
            numericUpDown_ListCount.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_ListCount.Name = "numericUpDown_ListCount";
            numericUpDown_ListCount.Size = new Size(36, 23);
            numericUpDown_ListCount.TabIndex = 31;
            numericUpDown_ListCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // checkBoxy_AutoStart
            // 
            checkBoxy_AutoStart.AutoSize = true;
            checkBoxy_AutoStart.Location = new Point(7, 253);
            checkBoxy_AutoStart.Name = "checkBoxy_AutoStart";
            checkBoxy_AutoStart.Size = new Size(136, 19);
            checkBoxy_AutoStart.TabIndex = 30;
            checkBoxy_AutoStart.Text = "mit Windows Starten";
            checkBoxy_AutoStart.UseVisualStyleBackColor = true;
            // 
            // label_Token
            // 
            label_Token.AutoSize = true;
            label_Token.Location = new Point(7, 67);
            label_Token.Name = "label_Token";
            label_Token.Size = new Size(38, 15);
            label_Token.TabIndex = 29;
            label_Token.Text = "Token";
            // 
            // label_StateUrl
            // 
            label_StateUrl.AutoSize = true;
            label_StateUrl.Location = new Point(7, 38);
            label_StateUrl.Name = "label_StateUrl";
            label_StateUrl.Size = new Size(57, 15);
            label_StateUrl.TabIndex = 28;
            label_StateUrl.Text = "State URL";
            // 
            // label_SendUrl
            // 
            label_SendUrl.AutoSize = true;
            label_SendUrl.Location = new Point(7, 9);
            label_SendUrl.Name = "label_SendUrl";
            label_SendUrl.Size = new Size(57, 15);
            label_SendUrl.TabIndex = 27;
            label_SendUrl.Text = "Send URL";
            // 
            // checkBox_Debug
            // 
            checkBox_Debug.AutoSize = true;
            checkBox_Debug.Location = new Point(7, 304);
            checkBox_Debug.Name = "checkBox_Debug";
            checkBox_Debug.Size = new Size(61, 19);
            checkBox_Debug.TabIndex = 26;
            checkBox_Debug.Text = "Debug";
            checkBox_Debug.UseVisualStyleBackColor = true;
            // 
            // checkBox_SlowState
            // 
            checkBox_SlowState.AutoSize = true;
            checkBox_SlowState.Location = new Point(7, 228);
            checkBox_SlowState.Name = "checkBox_SlowState";
            checkBox_SlowState.Size = new Size(80, 19);
            checkBox_SlowState.TabIndex = 25;
            checkBox_SlowState.Text = "Slow State";
            checkBox_SlowState.UseVisualStyleBackColor = true;
            // 
            // textBox_Token
            // 
            textBox_Token.Location = new Point(70, 64);
            textBox_Token.Name = "textBox_Token";
            textBox_Token.Size = new Size(180, 23);
            textBox_Token.TabIndex = 24;
            // 
            // textBox_State
            // 
            textBox_State.Location = new Point(70, 35);
            textBox_State.Name = "textBox_State";
            textBox_State.Size = new Size(180, 23);
            textBox_State.TabIndex = 23;
            // 
            // textBox_Send
            // 
            textBox_Send.Location = new Point(70, 6);
            textBox_Send.Name = "textBox_Send";
            textBox_Send.Size = new Size(180, 23);
            textBox_Send.TabIndex = 22;
            // 
            // checkBox_AutoUpdate
            // 
            checkBox_AutoUpdate.AutoSize = true;
            checkBox_AutoUpdate.Location = new Point(7, 203);
            checkBox_AutoUpdate.Name = "checkBox_AutoUpdate";
            checkBox_AutoUpdate.Size = new Size(93, 19);
            checkBox_AutoUpdate.TabIndex = 21;
            checkBox_AutoUpdate.Text = "Auto Update";
            checkBox_AutoUpdate.UseVisualStyleBackColor = true;
            // 
            // checkBox_OnlyBGS
            // 
            checkBox_OnlyBGS.AutoSize = true;
            checkBox_OnlyBGS.Location = new Point(7, 178);
            checkBox_OnlyBGS.Name = "checkBox_OnlyBGS";
            checkBox_OnlyBGS.Size = new Size(158, 19);
            checkBox_OnlyBGS.TabIndex = 20;
            checkBox_OnlyBGS.Text = "Nur BGS relevante zeigen";
            checkBox_OnlyBGS.UseVisualStyleBackColor = true;
            // 
            // checkBox_FullList
            // 
            checkBox_FullList.AutoSize = true;
            checkBox_FullList.Location = new Point(7, 153);
            checkBox_FullList.Name = "checkBox_FullList";
            checkBox_FullList.Size = new Size(136, 19);
            checkBox_FullList.TabIndex = 19;
            checkBox_FullList.Text = "Gesamte Liste zeigen";
            checkBox_FullList.UseVisualStyleBackColor = true;
            // 
            // checkBox_CMDr
            // 
            checkBox_CMDr.AutoSize = true;
            checkBox_CMDr.Location = new Point(7, 128);
            checkBox_CMDr.Name = "checkBox_CMDr";
            checkBox_CMDr.Size = new Size(160, 19);
            checkBox_CMDr.TabIndex = 18;
            checkBox_CMDr.Text = "CMDr Namen übertragen";
            checkBox_CMDr.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label_Disclaimer);
            tabPage2.Controls.Add(groupBox_Overlay);
            tabPage2.Controls.Add(groupBox_MainFrame);
            tabPage2.Controls.Add(checkBox_Override);
            tabPage2.Controls.Add(radioButton_Custom);
            tabPage2.Controls.Add(radioButton_Dark);
            tabPage2.Controls.Add(radioButton_Light);
            tabPage2.Controls.Add(checkBox_AlwaysTop);
            tabPage2.Controls.Add(button_Design);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(332, 346);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Design";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label_Disclaimer
            // 
            label_Disclaimer.AutoSize = true;
            label_Disclaimer.Location = new Point(8, 128);
            label_Disclaimer.Name = "label_Disclaimer";
            label_Disclaimer.Size = new Size(112, 135);
            label_Disclaimer.TabIndex = 41;
            label_Disclaimer.Text = "Overlay Transparenz\r\ndurch Choma-Key\r\ndes Overlay\r\nhintergrundes!\r\n\r\nAlle Elemente mit\r\nidentischer Farbe\r\nim Overlay\r\nwerden transparent!";
            label_Disclaimer.Visible = false;
            // 
            // groupBox_Overlay
            // 
            groupBox_Overlay.Controls.Add(ColorPick_Overlay_Tick_Light);
            groupBox_Overlay.Controls.Add(ColorPick_Overlay_Systeme_Light);
            groupBox_Overlay.Controls.Add(label5);
            groupBox_Overlay.Controls.Add(label6);
            groupBox_Overlay.Controls.Add(label8);
            groupBox_Overlay.Controls.Add(ColorPick_Overlay_Systeme_Dark);
            groupBox_Overlay.Controls.Add(ColorPick_Overlay_Tick_Dark);
            groupBox_Overlay.Controls.Add(ColorPick_Overlay_Background);
            groupBox_Overlay.Location = new Point(128, 157);
            groupBox_Overlay.Name = "groupBox_Overlay";
            groupBox_Overlay.Size = new Size(195, 103);
            groupBox_Overlay.TabIndex = 39;
            groupBox_Overlay.TabStop = false;
            groupBox_Overlay.Text = "Overlay";
            // 
            // ColorPick_Overlay_Tick_Light
            // 
            ColorPick_Overlay_Tick_Light.BorderStyle = BorderStyle.FixedSingle;
            ColorPick_Overlay_Tick_Light.Location = new Point(121, 53);
            ColorPick_Overlay_Tick_Light.Name = "ColorPick_Overlay_Tick_Light";
            ColorPick_Overlay_Tick_Light.Size = new Size(15, 15);
            ColorPick_Overlay_Tick_Light.TabIndex = 13;
            ColorPick_Overlay_Tick_Light.TabStop = false;
            // 
            // ColorPick_Overlay_Systeme_Light
            // 
            ColorPick_Overlay_Systeme_Light.BorderStyle = BorderStyle.FixedSingle;
            ColorPick_Overlay_Systeme_Light.Location = new Point(121, 74);
            ColorPick_Overlay_Systeme_Light.Name = "ColorPick_Overlay_Systeme_Light";
            ColorPick_Overlay_Systeme_Light.Size = new Size(15, 15);
            ColorPick_Overlay_Systeme_Light.TabIndex = 12;
            ColorPick_Overlay_Systeme_Light.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 74);
            label5.Name = "label5";
            label5.Size = new Size(51, 15);
            label5.TabIndex = 11;
            label5.Text = "Systeme";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 53);
            label6.Name = "label6";
            label6.Size = new Size(28, 15);
            label6.TabIndex = 10;
            label6.Text = "Tick";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 32);
            label8.Name = "label8";
            label8.Size = new Size(72, 15);
            label8.TabIndex = 8;
            label8.Text = "Hintergrund";
            // 
            // ColorPick_Overlay_Systeme_Dark
            // 
            ColorPick_Overlay_Systeme_Dark.BorderStyle = BorderStyle.FixedSingle;
            ColorPick_Overlay_Systeme_Dark.Location = new Point(84, 74);
            ColorPick_Overlay_Systeme_Dark.Name = "ColorPick_Overlay_Systeme_Dark";
            ColorPick_Overlay_Systeme_Dark.Size = new Size(15, 15);
            ColorPick_Overlay_Systeme_Dark.TabIndex = 4;
            ColorPick_Overlay_Systeme_Dark.TabStop = false;
            // 
            // ColorPick_Overlay_Tick_Dark
            // 
            ColorPick_Overlay_Tick_Dark.BorderStyle = BorderStyle.FixedSingle;
            ColorPick_Overlay_Tick_Dark.Location = new Point(84, 53);
            ColorPick_Overlay_Tick_Dark.Name = "ColorPick_Overlay_Tick_Dark";
            ColorPick_Overlay_Tick_Dark.Size = new Size(15, 15);
            ColorPick_Overlay_Tick_Dark.TabIndex = 3;
            ColorPick_Overlay_Tick_Dark.TabStop = false;
            // 
            // ColorPick_Overlay_Background
            // 
            ColorPick_Overlay_Background.BorderStyle = BorderStyle.FixedSingle;
            ColorPick_Overlay_Background.Location = new Point(84, 32);
            ColorPick_Overlay_Background.Name = "ColorPick_Overlay_Background";
            ColorPick_Overlay_Background.Size = new Size(15, 15);
            ColorPick_Overlay_Background.TabIndex = 1;
            ColorPick_Overlay_Background.TabStop = false;
            // 
            // groupBox_MainFrame
            // 
            groupBox_MainFrame.Controls.Add(button_ResetToDark);
            groupBox_MainFrame.Controls.Add(button_ResetToLight);
            groupBox_MainFrame.Controls.Add(label4);
            groupBox_MainFrame.Controls.Add(label3);
            groupBox_MainFrame.Controls.Add(label2);
            groupBox_MainFrame.Controls.Add(label1);
            groupBox_MainFrame.Controls.Add(ColorPick_MainFrame_Systeme);
            groupBox_MainFrame.Controls.Add(ColorPick_MainFrame_Tick);
            groupBox_MainFrame.Controls.Add(ColorPick_MainFrame_Infos);
            groupBox_MainFrame.Controls.Add(ColorPick_MainFrame_Background);
            groupBox_MainFrame.Location = new Point(128, 31);
            groupBox_MainFrame.Name = "groupBox_MainFrame";
            groupBox_MainFrame.Size = new Size(195, 120);
            groupBox_MainFrame.TabIndex = 38;
            groupBox_MainFrame.TabStop = false;
            groupBox_MainFrame.Text = "Fenster";
            // 
            // button_ResetToDark
            // 
            button_ResetToDark.Location = new Point(114, 54);
            button_ResetToDark.Name = "button_ResetToDark";
            button_ResetToDark.Size = new Size(75, 23);
            button_ResetToDark.TabIndex = 9;
            button_ResetToDark.Text = "Reset Dark";
            button_ResetToDark.UseVisualStyleBackColor = true;
            button_ResetToDark.Visible = false;
            // 
            // button_ResetToLight
            // 
            button_ResetToLight.Location = new Point(114, 25);
            button_ResetToLight.Name = "button_ResetToLight";
            button_ResetToLight.Size = new Size(75, 23);
            button_ResetToLight.TabIndex = 8;
            button_ResetToLight.Text = "Reset Light";
            button_ResetToLight.UseVisualStyleBackColor = true;
            button_ResetToLight.Visible = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 95);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 7;
            label4.Text = "Systeme";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 74);
            label3.Name = "label3";
            label3.Size = new Size(28, 15);
            label3.TabIndex = 6;
            label3.Text = "Tick";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 53);
            label2.Name = "label2";
            label2.Size = new Size(33, 15);
            label2.TabIndex = 5;
            label2.Text = "Infos";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 32);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 4;
            label1.Text = "Hintergrund";
            // 
            // ColorPick_MainFrame_Systeme
            // 
            ColorPick_MainFrame_Systeme.BorderStyle = BorderStyle.FixedSingle;
            ColorPick_MainFrame_Systeme.Location = new Point(84, 95);
            ColorPick_MainFrame_Systeme.Name = "ColorPick_MainFrame_Systeme";
            ColorPick_MainFrame_Systeme.Size = new Size(15, 15);
            ColorPick_MainFrame_Systeme.TabIndex = 3;
            ColorPick_MainFrame_Systeme.TabStop = false;
            // 
            // ColorPick_MainFrame_Tick
            // 
            ColorPick_MainFrame_Tick.BorderStyle = BorderStyle.FixedSingle;
            ColorPick_MainFrame_Tick.Location = new Point(84, 75);
            ColorPick_MainFrame_Tick.Name = "ColorPick_MainFrame_Tick";
            ColorPick_MainFrame_Tick.Size = new Size(15, 15);
            ColorPick_MainFrame_Tick.TabIndex = 2;
            ColorPick_MainFrame_Tick.TabStop = false;
            // 
            // ColorPick_MainFrame_Infos
            // 
            ColorPick_MainFrame_Infos.BorderStyle = BorderStyle.FixedSingle;
            ColorPick_MainFrame_Infos.Location = new Point(84, 54);
            ColorPick_MainFrame_Infos.Name = "ColorPick_MainFrame_Infos";
            ColorPick_MainFrame_Infos.Size = new Size(15, 15);
            ColorPick_MainFrame_Infos.TabIndex = 1;
            ColorPick_MainFrame_Infos.TabStop = false;
            // 
            // ColorPick_MainFrame_Background
            // 
            ColorPick_MainFrame_Background.BorderStyle = BorderStyle.FixedSingle;
            ColorPick_MainFrame_Background.Location = new Point(84, 32);
            ColorPick_MainFrame_Background.Name = "ColorPick_MainFrame_Background";
            ColorPick_MainFrame_Background.Size = new Size(15, 15);
            ColorPick_MainFrame_Background.TabIndex = 0;
            ColorPick_MainFrame_Background.TabStop = false;
            // 
            // checkBox_Override
            // 
            checkBox_Override.AutoSize = true;
            checkBox_Override.Location = new Point(8, 106);
            checkBox_Override.Name = "checkBox_Override";
            checkBox_Override.Size = new Size(114, 19);
            checkBox_Override.TabIndex = 40;
            checkBox_Override.Text = "Overlay Override";
            checkBox_Override.UseVisualStyleBackColor = true;
            // 
            // radioButton_Custom
            // 
            radioButton_Custom.AutoSize = true;
            radioButton_Custom.Location = new Point(9, 81);
            radioButton_Custom.Name = "radioButton_Custom";
            radioButton_Custom.Size = new Size(67, 19);
            radioButton_Custom.TabIndex = 37;
            radioButton_Custom.TabStop = true;
            radioButton_Custom.Text = "Custom";
            radioButton_Custom.UseVisualStyleBackColor = true;
            // 
            // radioButton_Dark
            // 
            radioButton_Dark.AutoSize = true;
            radioButton_Dark.Location = new Point(9, 56);
            radioButton_Dark.Name = "radioButton_Dark";
            radioButton_Dark.Size = new Size(49, 19);
            radioButton_Dark.TabIndex = 36;
            radioButton_Dark.TabStop = true;
            radioButton_Dark.Text = "Dark";
            radioButton_Dark.UseVisualStyleBackColor = true;
            // 
            // radioButton_Light
            // 
            radioButton_Light.AutoSize = true;
            radioButton_Light.Location = new Point(9, 31);
            radioButton_Light.Name = "radioButton_Light";
            radioButton_Light.Size = new Size(52, 19);
            radioButton_Light.TabIndex = 35;
            radioButton_Light.TabStop = true;
            radioButton_Light.Text = "Light";
            radioButton_Light.UseVisualStyleBackColor = true;
            // 
            // checkBox_AlwaysTop
            // 
            checkBox_AlwaysTop.AutoSize = true;
            checkBox_AlwaysTop.Location = new Point(9, 6);
            checkBox_AlwaysTop.Name = "checkBox_AlwaysTop";
            checkBox_AlwaysTop.Size = new Size(147, 19);
            checkBox_AlwaysTop.TabIndex = 34;
            checkBox_AlwaysTop.Text = "immer im Vordergrund";
            checkBox_AlwaysTop.UseVisualStyleBackColor = true;
            // 
            // colorDialog1
            // 
            colorDialog1.SolidColorOnly = true;
            // 
            // checkBox_EDDN
            // 
            checkBox_EDDN.AutoSize = true;
            checkBox_EDDN.Location = new Point(7, 103);
            checkBox_EDDN.Name = "checkBox_EDDN";
            checkBox_EDDN.Size = new Size(114, 19);
            checkBox_EDDN.TabIndex = 33;
            checkBox_EDDN.Text = "an EDDN senden";
            checkBox_EDDN.UseVisualStyleBackColor = true;
            // 
            // Konfiguration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(340, 416);
            Controls.Add(tabControl1);
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
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_ListCount).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            groupBox_Overlay.ResumeLayout(false);
            groupBox_Overlay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Tick_Light).EndInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Systeme_Light).EndInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Systeme_Dark).EndInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Tick_Dark).EndInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_Overlay_Background).EndInit();
            groupBox_MainFrame.ResumeLayout(false);
            groupBox_MainFrame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ColorPick_MainFrame_Systeme).EndInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_MainFrame_Tick).EndInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_MainFrame_Infos).EndInit();
            ((System.ComponentModel.ISupportInitialize)ColorPick_MainFrame_Background).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button_Save;
        private Button button_Design;
        private ToolTip toolTip_Konfig;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private CheckBox checkBox_CloseMini;
        private NumericUpDown numericUpDown_ListCount;
        private CheckBox checkBoxy_AutoStart;
        private Label label_Token;
        private Label label_StateUrl;
        private Label label_SendUrl;
        private CheckBox checkBox_Debug;
        private CheckBox checkBox_SlowState;
        private TextBox textBox_Token;
        private TextBox textBox_State;
        private TextBox textBox_Send;
        private CheckBox checkBox_AutoUpdate;
        private CheckBox checkBox_OnlyBGS;
        private CheckBox checkBox_FullList;
        private CheckBox checkBox_CMDr;
        private TabPage tabPage2;
        private CheckBox checkBox_AlwaysTop;
        private Label label_Disclaimer;
        private GroupBox groupBox_Overlay;
        private PictureBox ColorPick_Overlay_Tick_Light;
        private PictureBox ColorPick_Overlay_Systeme_Light;
        private Label label5;
        private Label label6;
        private Label label8;
        private PictureBox ColorPick_Overlay_Systeme_Dark;
        private PictureBox ColorPick_Overlay_Tick_Dark;
        private PictureBox ColorPick_Overlay_Background;
        private GroupBox groupBox_MainFrame;
        private Button button_ResetToDark;
        private Button button_ResetToLight;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private PictureBox ColorPick_MainFrame_Systeme;
        private PictureBox ColorPick_MainFrame_Tick;
        private PictureBox ColorPick_MainFrame_Infos;
        private PictureBox ColorPick_MainFrame_Background;
        private CheckBox checkBox_Override;
        private RadioButton radioButton_Custom;
        private RadioButton radioButton_Dark;
        private RadioButton radioButton_Light;
        private ColorDialog colorDialog1;
        private CheckBox checkBox_EDDN;
    }
}