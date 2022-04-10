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
        AddButtonVisible = false;
        AddButtonEnabled = true;
        DeleteButtonVisible = false;
        DeleteButtonEnabled = true;
        SaveButtonVisible = false;
        SaveButtonEnabled = true;
        ReloadButtonVisible = false;
        ReloadButtonEnabled = true;
        ReportButtonVisible = false;
        ReportButtonEnabled = true;
        ApproveButtonVisible = false;
        ApproveButtonEnabled = true;
        RejectButtonVisible = false;
        RejectButtonEnabled = true;


        CustomButton1Visible = false;
        CustomButton1Enabled = true;
        CustomButton2Visible = false;
        CustomButton2Enabled = true;
        CustomButton3Visible = false;
        CustomButton3Enabled = true;
        CustomButton4Visible = false;
        CustomButton4Enabled = true;
        CustomButton5Visible = false;
        CustomButton5Enabled = true;
        CustomButton6Visible = false;
        CustomButton6Enabled = true;
        CustomButton7Visible = false;
        CustomButton7Enabled = true;
        CustomButton8Visible = false;
        CustomButton8Enabled = true;
        CustomButton9Visible = false;
        CustomButton9Enabled = true;

        CustomButton1Title = "CustomButton1";
        CustomButton2Title = "CustomButton2";
        CustomButton3Title = "CustomButton3";
        CustomButton4Title = "CustomButton4";
        CustomButton5Title = "CustomButton5";
        CustomButton6Title = "CustomButton6";
        CustomButton7Title = "CustomButton7";
        CustomButton8Title = "CustomButton8";
        CustomButton9Title = "CustomButton9";

        CustomButton1Icon = "";
        CustomButton2Icon = "";
        CustomButton3Icon = "";
        CustomButton4Icon = "";
        CustomButton5Icon = "";
        CustomButton6Icon = "";
        CustomButton7Icon = "";
        CustomButton8Icon = "";
        CustomButton9Icon = "";
    }

    public void ShowAlert(string message, Severity severity, Action? onClick = null)
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


    public bool MobileView { get { return mobileView; } set { mobileView = value; OnMobileViewChanged?.Invoke(mobileView); Refresh(); } }
    public bool OpenDrawer { get { return openDrawer; } set { openDrawer = value; Refresh(); } }
    public bool Loading { get { return loading; } set { loading = value; Refresh(); } }
    public bool AddButtonVisible { get; set; }
    public bool AddButtonEnabled { get; set; }
    public bool DeleteButtonVisible { get; set; }
    public bool DeleteButtonEnabled { get; set; }
    public bool SaveButtonVisible { get; set; }
    public bool SaveButtonEnabled { get; set; }
    public bool ReloadButtonVisible { get; set; }
    public bool ReloadButtonEnabled { get; set; }
    public bool ReportButtonVisible { get; set; }
    public bool ReportButtonEnabled { get; set; }
    public bool ApproveButtonVisible { get; set; }
    public bool ApproveButtonEnabled { get; set; }
    public bool RejectButtonVisible { get; set; }
    public bool RejectButtonEnabled { get; set; }


    public bool CustomButton1Visible { get; set; }
    public bool CustomButton1Enabled { get; set; }
    public bool CustomButton2Visible { get; set; }
    public bool CustomButton2Enabled { get; set; }
    public bool CustomButton3Visible { get; set; }
    public bool CustomButton3Enabled { get; set; }
    public bool CustomButton4Visible { get; set; }
    public bool CustomButton4Enabled { get; set; }
    public bool CustomButton5Visible { get; set; }
    public bool CustomButton5Enabled { get; set; }
    public bool CustomButton6Visible { get; set; }
    public bool CustomButton6Enabled { get; set; }
    public bool CustomButton7Visible { get; set; }
    public bool CustomButton7Enabled { get; set; }
    public bool CustomButton8Visible { get; set; }
    public bool CustomButton8Enabled { get; set; }
    public bool CustomButton9Visible { get; set; }
    public bool CustomButton9Enabled { get; set; }

    public string? CustomButton1Title { get; set; }
    public string? CustomButton2Title { get; set; }
    public string? CustomButton3Title { get; set; }
    public string? CustomButton4Title { get; set; }
    public string? CustomButton5Title { get; set; }
    public string? CustomButton6Title { get; set; }
    public string? CustomButton7Title { get; set; }
    public string? CustomButton8Title { get; set; }
    public string? CustomButton9Title { get; set; }

    public string? CustomButton1Icon { get; set; }
    public string? CustomButton2Icon { get; set; }
    public string? CustomButton3Icon { get; set; }
    public string? CustomButton4Icon { get; set; }
    public string? CustomButton5Icon { get; set; }
    public string? CustomButton6Icon { get; set; }
    public string? CustomButton7Icon { get; set; }
    public string? CustomButton8Icon { get; set; }
    public string? CustomButton9Icon { get; set; }


    public event Action<bool>? OnMobileViewChanged;

    public event Action? OnRefreshClick;
    public event Action? OnAddClick;
    public event Action? OnDeleteClick;
    public event Action? OnSaveClick;
    public event Action? OnReloadClick;
    public event Action? OnReportClick;
    public event Action? OnApproveClick;
    public event Action? OnRejectClick;

    public event Action? OnCustomButton1Click;
    public event Action? OnCustomButton2Click;
    public event Action? OnCustomButton3Click;
    public event Action? OnCustomButton4Click;
    public event Action? OnCustomButton5Click;
    public event Action? OnCustomButton6Click;
    public event Action? OnCustomButton7Click;
    public event Action? OnCustomButton8Click;
    public event Action? OnCustomButton9Click;

    public void Refresh()
    {
        OnRefreshClick?.Invoke();
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

    public void ReportClick()
    {
        OnReportClick?.Invoke();
    }

    public void ApproveClick()
    {
        OnApproveClick?.Invoke();
    }

    public void RejectClick()
    {
        OnRejectClick?.Invoke();
    }


    public void CustomButton1Click()
    {
        OnCustomButton1Click?.Invoke();
    }

    public void CustomButton2Click()
    {
        OnCustomButton2Click?.Invoke();
    }

    public void CustomButton3Click()
    {
        OnCustomButton3Click?.Invoke();
    }

    public void CustomButton4Click()
    {
        OnCustomButton4Click?.Invoke();
    }

    public void CustomButton5Click()
    {
        OnCustomButton5Click?.Invoke();
    }

    public void CustomButton6Click()
    {
        OnCustomButton6Click?.Invoke();
    }

    public void CustomButton7Click()
    {
        OnCustomButton7Click?.Invoke();
    }

    public void CustomButton8Click()
    {
        OnCustomButton8Click?.Invoke();
    }

    public void CustomButton9Click()
    {
        OnCustomButton9Click?.Invoke();
    }
}
