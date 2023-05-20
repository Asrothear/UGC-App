

namespace UGC_App;

public partial class Design : Form
{
    private readonly Mainframe _parent;

    public Design(Mainframe pat)
    {
        InitializeComponent();
        TopMost = Config.Instance.AlwaysOnTop;
        _parent = pat;
        foreach (Control grp in Controls)
        {
            if (grp is not GroupBox) continue;
            foreach (Control control in grp.Controls)
            {
                if (control is PictureBox)
                {
                    control.Click += (_, _) => SelectColor(control);
                }
            }
        }
        GetSetting();
        radioButton_Light.Click += (_, _) => ChangeTheme(0);
        radioButton_Dark.Click += (_, _) => ChangeTheme(1);
        radioButton_Custom.Click += (_, _) => ChangeTheme(2);
        _parent.SetDesign();
        checkBox_Override.Click += (_, _) =>
        {
            groupBox_Overlay.Enabled = checkBox_Override.Checked;
            label_Disclaimer.Visible = checkBox_Override.Checked;
            ColorPick_Overlay_Background.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlayBackground : Config.Instance.ColorDefaultChroma;
            ColorPick_Overlay_Systeme_Light.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlaySystemeLight : Config.Instance.ColorDefaultLabelLight;
            ColorPick_Overlay_Systeme_Dark.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlaySystemeDark : Config.Instance.ColorDefaultLabelDark;
            ColorPick_Overlay_Tick_Light.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlayTickLight : Config.Instance.ColorDefaultLabelLight;
            ColorPick_Overlay_Tick_Dark.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlayTickDark : Config.Instance.ColorDefaultLabelDark;
            Config.Instance.ColorOverlayOverride = checkBox_Override.Checked;
            Config.Save();
            GetSetting();
            _parent.SetDesign();
        };
        button_ResetToLight.Click += (_, _) =>
        {
            ResetColors(true);
        };
        button_ResetToDark.Click += (_, _) =>
        {
            ResetColors(false);
        };
    }

    private void ResetColors(bool p0)
    {
        ColorPick_MainFrame_Background.BackColor = p0
            ? Config.Instance.ColorDefaultBackgroundLight
            : Config.Instance.ColorDefaultBackgroundDark;
        ColorPick_MainFrame_Infos.BackColor = p0
            ? Config.Instance.ColorDefaultLabelLight
            : Config.Instance.ColorDefaultLabelDark;
        ColorPick_MainFrame_Tick.BackColor = p0
            ? Config.Instance.ColorDefaultLabelLight
            : Config.Instance.ColorDefaultLabelDark;
        ColorPick_MainFrame_Systeme.BackColor = p0
            ? Config.Instance.ColorDefaultLabelLight
            : Config.Instance.ColorDefaultLabelDark;
        ColorPick_Overlay_Background.BackColor = Config.Instance.ColorDefaultChroma;
        ColorPick_Overlay_Systeme_Light.BackColor = Config.Instance.ColorDefaultLabelLight;
        ColorPick_Overlay_Systeme_Dark.BackColor = Config.Instance.ColorDefaultLabelDark;
        ColorPick_Overlay_Tick_Light.BackColor = Config.Instance.ColorDefaultLabelLight;
        ColorPick_Overlay_Tick_Dark.BackColor = Config.Instance.ColorDefaultLabelDark;
        Config.Instance.ColorMainBackground = ColorPick_MainFrame_Background.BackColor;
        Config.Instance.ColorMainInfo = ColorPick_MainFrame_Infos.BackColor;
        Config.Instance.ColorMainTick = ColorPick_MainFrame_Tick.BackColor;
        Config.Instance.ColorMainSysteme = ColorPick_MainFrame_Systeme.BackColor;
        if (checkBox_Override.Checked)
        {

            Config.Instance.ColorOverlayBackground = ColorPick_Overlay_Background.BackColor;
            Config.Instance.ColorOverlaySystemeLight = ColorPick_Overlay_Systeme_Light.BackColor;
            Config.Instance.ColorOverlaySystemeDark = ColorPick_Overlay_Systeme_Dark.BackColor;
            Config.Instance.ColorOverlayTickLight = ColorPick_Overlay_Tick_Light.BackColor;
            Config.Instance.ColorOverlayTickDark = ColorPick_Overlay_Tick_Dark.BackColor;
            Config.Instance.ColorOverlayOverride = checkBox_Override.Checked;
        }

        Config.Save();
        _parent.SetDesign();
    }

    private void ChangeTheme(int p0)
    {
        Config.Instance.DesignSel = p0;
        Config.Save();
        _parent.SetDesign();
        GetSetting();
    }

    private void GetSetting()
    {
        switch (Config.Instance.DesignSel)
        {
            case 0:
            case 1:
                radioButton_Light.Checked = true;
                radioButton_Dark.Checked = false;
                radioButton_Custom.Checked = false;
                if (Config.Instance.DesignSel == 1)
                {
                    radioButton_Light.Checked = false;
                    radioButton_Dark.Checked = true;
                }

                ColorPick_MainFrame_Background.BackColor = radioButton_Light.Checked
                    ? Config.Instance.ColorDefaultBackgroundLight
                    : Config.Instance.ColorDefaultBackgroundDark;
                ColorPick_MainFrame_Infos.BackColor = radioButton_Light.Checked
                    ? Config.Instance.ColorDefaultLabelLight
                    : Config.Instance.ColorDefaultLabelDark;
                ColorPick_MainFrame_Tick.BackColor = radioButton_Light.Checked
                    ? Config.Instance.ColorDefaultLabelLight
                    : Config.Instance.ColorDefaultLabelDark;
                ColorPick_MainFrame_Systeme.BackColor = radioButton_Light.Checked
                    ? Config.Instance.ColorDefaultLabelLight
                    : Config.Instance.ColorDefaultLabelDark;
                ColorPick_Overlay_Background.BackColor = Config.Instance.ColorDefaultChroma;
                ColorPick_Overlay_Systeme_Light.BackColor = Config.Instance.ColorDefaultLabelLight;
                ColorPick_Overlay_Systeme_Dark.BackColor = Config.Instance.ColorDefaultLabelDark;
                ColorPick_Overlay_Tick_Light.BackColor = Config.Instance.ColorDefaultLabelLight;
                ColorPick_Overlay_Tick_Dark.BackColor = Config.Instance.ColorDefaultLabelDark;
                break;
            case 2:
                radioButton_Light.Checked = false;
                radioButton_Dark.Checked = false;
                radioButton_Custom.Checked = true;
                ColorPick_MainFrame_Background.BackColor = Config.Instance.ColorMainBackground;
                ColorPick_MainFrame_Infos.BackColor = Config.Instance.ColorMainInfo;
                ColorPick_MainFrame_Tick.BackColor = Config.Instance.ColorMainTick;
                ColorPick_MainFrame_Systeme.BackColor = Config.Instance.ColorMainSysteme;
                ColorPick_Overlay_Background.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlayBackground : Config.Instance.ColorDefaultChroma;
                ColorPick_Overlay_Systeme_Light.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlaySystemeLight : Config.Instance.ColorDefaultLabelLight;
                ColorPick_Overlay_Systeme_Dark.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlaySystemeDark : Config.Instance.ColorDefaultLabelDark;
                ColorPick_Overlay_Tick_Light.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlayTickLight : Config.Instance.ColorDefaultLabelLight;
                ColorPick_Overlay_Tick_Dark.BackColor = checkBox_Override.Checked ? Config.Instance.ColorOverlayTickDark : Config.Instance.ColorDefaultLabelDark;
                break;
        }
        checkBox_Override.Checked = Config.Instance.ColorOverlayOverride;
        ColorPick_MainFrame_Background.Enabled = radioButton_Custom.Checked;
        ColorPick_MainFrame_Infos.Enabled = radioButton_Custom.Checked;
        ColorPick_MainFrame_Tick.Enabled = radioButton_Custom.Checked;
        ColorPick_MainFrame_Systeme.Enabled = radioButton_Custom.Checked;
        groupBox_Overlay.Enabled = checkBox_Override.Checked;
        label_Disclaimer.Visible = checkBox_Override.Checked;
        checkBox_Override.Visible = radioButton_Custom.Checked;
        button_ResetToLight.Visible = radioButton_Custom.Checked;
        button_ResetToDark.Visible = radioButton_Custom.Checked;
        if (radioButton_Custom.Checked) return;
        groupBox_Overlay.Enabled = radioButton_Custom.Checked;
        label_Disclaimer.Visible = radioButton_Custom.Checked;
        checkBox_Override.Visible = radioButton_Custom.Checked;
    }

    private void SelectColor(Control control)
    {
        colorDialog1.Color = control.BackColor;
        colorDialog1.ShowDialog(this);
        control.BackColor = colorDialog1.Color;
        Config.Instance.ColorMainBackground = ColorPick_MainFrame_Background.BackColor;
        Config.Instance.ColorMainInfo = ColorPick_MainFrame_Infos.BackColor;
        Config.Instance.ColorMainTick = ColorPick_MainFrame_Tick.BackColor;
        Config.Instance.ColorMainSysteme = ColorPick_MainFrame_Systeme.BackColor;
        Config.Instance.ColorOverlayBackground = ColorPick_Overlay_Background.BackColor;
        Config.Instance.ColorOverlaySystemeLight = ColorPick_Overlay_Systeme_Light.BackColor;
        Config.Instance.ColorOverlaySystemeDark = ColorPick_Overlay_Systeme_Dark.BackColor;
        Config.Instance.ColorOverlayTickLight = ColorPick_Overlay_Tick_Light.BackColor;
        Config.Instance.ColorOverlayTickDark = ColorPick_Overlay_Tick_Dark.BackColor;
        Config.Instance.ColorOverlayOverride = checkBox_Override.Checked;
        Config.Save();
        _parent.SetDesign();
    }

    public void SetDesign(int p0)
    {
        switch (p0)
        {
            case 0:
                BackColor = Config.Instance.ColorDefaultBackgroundLight;
                foreach (Control control in Controls)
                {
                    switch (control)
                    {
                        case Label:
                        case CheckBox:
                        case RadioButton:
                            control.ForeColor = Config.Instance.ColorDefaultLabelLight;
                            break;
                        case GroupBox:
                        {
                            control.ForeColor = Config.Instance.ColorDefaultLabelLight;
                            foreach (Control sub in control.Controls)
                            {
                                switch (sub)
                                {
                                    case Label:
                                    case CheckBox:
                                    case RadioButton:
                                    case GroupBox:
                                    case Button:
                                        sub.ForeColor = Config.Instance.ColorDefaultLabelLight;
                                        break;
                                }
                            }

                            break;
                        }
                    }
                }
                break;
            case 1:
                BackColor = Config.Instance.ColorDefaultBackgroundDark;
                foreach (Control control in Controls)
                {
                    switch (control)
                    {
                        case Label:
                        case CheckBox:
                        case RadioButton:
                            control.ForeColor = Config.Instance.ColorDefaultLabelDark;
                            break;
                        case GroupBox:
                        {
                            control.ForeColor = Config.Instance.ColorDefaultLabelDark;
                            foreach (Control sub in control.Controls)
                            {
                                switch (sub)
                                {
                                    case Label:
                                    case CheckBox:
                                    case RadioButton:
                                    case GroupBox:
                                        sub.ForeColor = Config.Instance.ColorDefaultLabelDark;
                                        break;
                                    case Button:
                                        sub.ForeColor = Config.Instance.ColorDefaultLabelLight;
                                        break;
                                }
                            }

                            break;
                        }
                    }
                }
                break;
            case 2:
                BackColor = Config.Instance.ColorMainBackground;
                foreach (Control control in Controls)
                {
                    switch (control)
                    {
                        case Label:
                        case CheckBox:
                        case RadioButton:
                            control.ForeColor = Config.Instance.ColorMainInfo;
                            break;
                        case GroupBox:
                        {
                            control.ForeColor = Config.Instance.ColorMainInfo;
                            foreach (Control sub in control.Controls)
                            {
                                switch (sub)
                                {
                                    case Label:
                                    case CheckBox:
                                    case RadioButton:
                                    case GroupBox:
                                        sub.ForeColor = Config.Instance.ColorMainInfo;
                                        break;
                                    case Button:
                                        sub.ForeColor = Config.Instance.ColorDefaultLabelLight;
                                        break;
                                }
                            }

                            break;
                        }
                    }
                }
                break;

        }
    }
}