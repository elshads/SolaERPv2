namespace SolaERPv2.Server.AppData;

public class AppState
{
    ISnackbar Snackbar { get; set; }
    public AppState()
    {

    }
    public AppState(ISnackbar snackbar)
    {
        Snackbar = snackbar;
        SetDefaults();
    }

    public void SetDefaults()
    {
        BackButtonVisible = false;
        BackButtonEnabled = true;
        NextButtonVisible = false;
        NextButtonEnabled = true;
        AddButtonVisible = false;
        AddButtonEnabled = true;
        DeleteButtonVisible = false;
        DeleteButtonEnabled = true;
        SaveButtonVisible = false;
        SaveButtonEnabled = true;
        ReloadButtonVisible = false;
        ReloadButtonEnabled = true;
        ApproveButtonVisible = false;
        ApproveButtonEnabled = true;
        RejectButtonVisible = false;
        RejectButtonEnabled = true;

        ReportButtonVisible = false;
        ReportButtonEnabled = true;
        AttachmentButtonVisible = false;
        AttachmentButtonEnabled = true;

        CustomButton01Visible = false;
        CustomButton01Enabled = true;
        CustomButton02Visible = false;
        CustomButton02Enabled = true;
        CustomButton03Visible = false;
        CustomButton03Enabled = true;
        CustomButton04Visible = false;
        CustomButton04Enabled = true;
        CustomButton05Visible = false;
        CustomButton05Enabled = true;
        CustomButton06Visible = false;
        CustomButton06Enabled = true;
        CustomButton07Visible = false;
        CustomButton07Enabled = true;
        CustomButton08Visible = false;
        CustomButton08Enabled = true;
        CustomButton09Visible = false;
        CustomButton09Enabled = true;
        CustomButton10Visible = false;
        CustomButton10Enabled = true;
        CustomButton11Visible = false;
        CustomButton11Enabled = true;
        CustomButton12Visible = false;
        CustomButton12Enabled = true;
        CustomButton13Visible = false;
        CustomButton13Enabled = true;
        CustomButton14Visible = false;
        CustomButton14Enabled = true;

        CustomButton01Title = "CustomButton01";
        CustomButton02Title = "CustomButton02";
        CustomButton03Title = "CustomButton03";
        CustomButton04Title = "CustomButton04";
        CustomButton05Title = "CustomButton05";
        CustomButton06Title = "CustomButton06";
        CustomButton07Title = "CustomButton07";
        CustomButton08Title = "CustomButton08";
        CustomButton09Title = "CustomButton09";
        CustomButton10Title = "CustomButton10";
        CustomButton11Title = "CustomButton11";
        CustomButton12Title = "CustomButton12";
        CustomButton13Title = "CustomButton13";
        CustomButton14Title = "CustomButton14";

        CustomButton01Icon = "";
        CustomButton02Icon = "";
        CustomButton03Icon = "";
        CustomButton04Icon = "";
        CustomButton05Icon = "";
        CustomButton06Icon = "";
        CustomButton07Icon = "";
        CustomButton08Icon = "";
        CustomButton09Icon = "";
        CustomButton10Icon = "";
        CustomButton11Icon = "";
        CustomButton12Icon = "";
        CustomButton13Icon = "";
        CustomButton14Icon = "";
    }

    public void ShowAlert(string message, MudBlazor.Severity severity, Action? onClick = null)
    {
        Snackbar.Add(message, severity, config =>
        {
            config.Onclick = snackbar =>
            {
                config.RequireInteraction = (onClick != null);
                onClick?.Invoke();
                return Task.CompletedTask;
            };
        });
    }

    bool loading = false;
    bool openDrawer;
    bool mobileView;


    public string? MudBreakpoint { get; set; } = "";
    public bool MobileView { get { return mobileView; } set { mobileView = value; OnMobileViewChanged?.Invoke(mobileView); Refresh(); } }
    public bool OpenDrawer { get { return openDrawer; } set { openDrawer = value; Refresh(); } }
    public bool Loading { get { return loading; } set { loading = value; Refresh(); } }
    public bool IsDarkMode { get; set; } = false;
    public bool BackButtonVisible { get; set; }
    public bool BackButtonEnabled { get; set; }
    public bool NextButtonVisible { get; set; }
    public bool NextButtonEnabled { get; set; }
    public bool AddButtonVisible { get; set; }
    public bool AddButtonEnabled { get; set; }
    public bool DeleteButtonVisible { get; set; }
    public bool DeleteButtonEnabled { get; set; }
    public bool SaveButtonVisible { get; set; }
    public bool SaveButtonEnabled { get; set; }
    public bool ReloadButtonVisible { get; set; }
    public bool ReloadButtonEnabled { get; set; }
    public bool ApproveButtonVisible { get; set; }
    public bool ApproveButtonEnabled { get; set; }
    public bool RejectButtonVisible { get; set; }
    public bool RejectButtonEnabled { get; set; }

    public bool ReportButtonVisible { get; set; }
    public bool ReportButtonEnabled { get; set; }
    public bool AttachmentButtonVisible { get; set; }
    public bool AttachmentButtonEnabled { get; set; }

    public bool CustomButton01Visible { get; set; }
    public bool CustomButton01Enabled { get; set; }
    public bool CustomButton02Visible { get; set; }
    public bool CustomButton02Enabled { get; set; }
    public bool CustomButton03Visible { get; set; }
    public bool CustomButton03Enabled { get; set; }
    public bool CustomButton04Visible { get; set; }
    public bool CustomButton04Enabled { get; set; }
    public bool CustomButton05Visible { get; set; }
    public bool CustomButton05Enabled { get; set; }
    public bool CustomButton06Visible { get; set; }
    public bool CustomButton06Enabled { get; set; }
    public bool CustomButton07Visible { get; set; }
    public bool CustomButton07Enabled { get; set; }
    public bool CustomButton08Visible { get; set; }
    public bool CustomButton08Enabled { get; set; }
    public bool CustomButton09Visible { get; set; }
    public bool CustomButton09Enabled { get; set; }
    public bool CustomButton10Visible { get; set; }
    public bool CustomButton10Enabled { get; set; }
    public bool CustomButton11Visible { get; set; }
    public bool CustomButton11Enabled { get; set; }
    public bool CustomButton12Visible { get; set; }
    public bool CustomButton12Enabled { get; set; }
    public bool CustomButton13Visible { get; set; }
    public bool CustomButton13Enabled { get; set; }
    public bool CustomButton14Visible { get; set; }
    public bool CustomButton14Enabled { get; set; }

    public string? CustomButton01Title { get; set; }
    public string? CustomButton02Title { get; set; }
    public string? CustomButton03Title { get; set; }
    public string? CustomButton04Title { get; set; }
    public string? CustomButton05Title { get; set; }
    public string? CustomButton06Title { get; set; }
    public string? CustomButton07Title { get; set; }
    public string? CustomButton08Title { get; set; }
    public string? CustomButton09Title { get; set; }
    public string? CustomButton10Title { get; set; }
    public string? CustomButton11Title { get; set; }
    public string? CustomButton12Title { get; set; }
    public string? CustomButton13Title { get; set; }
    public string? CustomButton14Title { get; set; }

    public string? CustomButton01Icon { get; set; }
    public string? CustomButton02Icon { get; set; }
    public string? CustomButton03Icon { get; set; }
    public string? CustomButton04Icon { get; set; }
    public string? CustomButton05Icon { get; set; }
    public string? CustomButton06Icon { get; set; }
    public string? CustomButton07Icon { get; set; }
    public string? CustomButton08Icon { get; set; }
    public string? CustomButton09Icon { get; set; }
    public string? CustomButton10Icon { get; set; }
    public string? CustomButton11Icon { get; set; }
    public string? CustomButton12Icon { get; set; }
    public string? CustomButton13Icon { get; set; }
    public string? CustomButton14Icon { get; set; }


    public event Action<bool>? OnMobileViewChanged;

    public event Action? OnRefreshClick;
    public event Action? OnChangeThemeClick;
    public event Action? OnBackClick;
    public event Action? OnNextClick;
    public event Action? OnAddClick;
    public event Action? OnDeleteClick;
    public event Action? OnSaveClick;
    public event Action? OnReloadClick;
    public event Action? OnApproveClick;
    public event Action? OnRejectClick;

    public event Action? OnReportClick;
    public event Action? OnAttachmentClick;

    public event Action? OnCustomButton01Click;
    public event Action? OnCustomButton02Click;
    public event Action? OnCustomButton03Click;
    public event Action? OnCustomButton04Click;
    public event Action? OnCustomButton05Click;
    public event Action? OnCustomButton06Click;
    public event Action? OnCustomButton07Click;
    public event Action? OnCustomButton08Click;
    public event Action? OnCustomButton09Click;
    public event Action? OnCustomButton10Click;
    public event Action? OnCustomButton11Click;
    public event Action? OnCustomButton12Click;
    public event Action? OnCustomButton13Click;
    public event Action? OnCustomButton14Click;

    public void Refresh()
    {
        OnRefreshClick?.Invoke();
    }

    public void ChangeTheme()
    {
        OnChangeThemeClick?.Invoke();
    }

    public void BackClick()
    {
        OnBackClick?.Invoke();
    }

    public void NextClick()
    {
        OnNextClick?.Invoke();
    }

    public void AddClick()
    {
        OnAddClick?.Invoke();
    }

    public void DeleteClick()
    {
        OnDeleteClick?.Invoke();
    }

    public void SaveClick()
    {
        OnSaveClick?.Invoke();
    }

    public void ReloadClick()
    {
        OnReloadClick?.Invoke();
    }

    public void ApproveClick()
    {
        OnApproveClick?.Invoke();
    }

    public void RejectClick()
    {
        OnRejectClick?.Invoke();
    }

    public void ReportClick()
    {
        OnReportClick?.Invoke();
    }

    public void AttachmentClick()
    {
        OnAttachmentClick?.Invoke();
    }


    public void CustomButton01Click()
    {
        OnCustomButton01Click?.Invoke();
    }

    public void CustomButton02Click()
    {
        OnCustomButton02Click?.Invoke();
    }

    public void CustomButton03Click()
    {
        OnCustomButton03Click?.Invoke();
    }

    public void CustomButton04Click()
    {
        OnCustomButton04Click?.Invoke();
    }

    public void CustomButton05Click()
    {
        OnCustomButton05Click?.Invoke();
    }

    public void CustomButton06Click()
    {
        OnCustomButton06Click?.Invoke();
    }

    public void CustomButton07Click()
    {
        OnCustomButton07Click?.Invoke();
    }

    public void CustomButton08Click()
    {
        OnCustomButton08Click?.Invoke();
    }

    public void CustomButton09Click()
    {
        OnCustomButton09Click?.Invoke();
    }

    public void CustomButton10Click()
    {
        OnCustomButton10Click?.Invoke();
    }

    public void CustomButton11Click()
    {
        OnCustomButton11Click?.Invoke();
    }

    public void CustomButton12Click()
    {
        OnCustomButton12Click?.Invoke();
    }

    public void CustomButton13Click()
    {
        OnCustomButton13Click?.Invoke();
    }

    public void CustomButton14Click()
    {
        OnCustomButton14Click?.Invoke();
    }
}
