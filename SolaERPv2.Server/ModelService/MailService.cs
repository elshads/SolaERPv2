using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using System.Net;
using System.Net.Mail;

namespace SolaERPv2.Server.ModelService;

public class MailService
{
    MailSettings? _mailSettings;
    AppSettings? _appSettings;
    public MailService(MailSettings mailSettings, AppSettings appSettings)
    {
        _mailSettings = mailSettings;
        _appSettings = appSettings;
    }

    public async Task<bool> SendHtmlMailAsync(IEnumerable<string> toAddresses, string subject, string htmlBody, string? fromSubject = null)
    {
        try
        {
            using var message = new MailMessage();
            var smtp = new SmtpClient();
            message.IsBodyHtml = true;
            message.From = new MailAddress(_mailSettings.FromEmail, fromSubject);
            foreach (var address in toAddresses)
            {
                message.To.Add(address);
            }
            message.Subject = subject;
            message.Body = htmlBody;
            smtp.Host = _mailSettings.Host;
            smtp.Port = _mailSettings.Port;
            smtp.EnableSsl = _mailSettings.EnableSsl;
            smtp.Credentials = new NetworkCredential(_mailSettings.Username, _mailSettings.Password);
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}
