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
            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = htmlBody;
            message.From = new MailAddress(_mailSettings.FromEmail);


            foreach (var address in toAddresses)
            {
                message.To.Add(address);
            }
            
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(_mailSettings.FromEmail, _mailSettings.Password);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtp.SendMailAsync(message);
                return true;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }


    }
}
