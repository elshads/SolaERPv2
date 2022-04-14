namespace SolaERPv2.Server.Models;

public class MailSettings
{
    public MailSettings() { }
    public MailSettings(MailSettings mailSettings)
    {
        Host = mailSettings.Host;
        Port = mailSettings.Port;
        EnableSsl = mailSettings.EnableSsl;
        Username = mailSettings.Username;
        Password = mailSettings.Password;
        FromEmail = mailSettings.FromEmail;
    }
    public string? Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? FromEmail { get; set; }
}
