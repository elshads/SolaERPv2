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
            //var smtp = new SmtpClient();
            message.IsBodyHtml = true;
          
            //message.From = new MailAddress(_mailSettings.FromEmail, fromSubject);
            message.From = new MailAddress("salimovkhayal@gmail.com");


            foreach (var address in toAddresses)
            {
                message.To.Add(address);
            }
         

            message.Subject = subject;
            message.Body = htmlBody;


            
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new System.Net.NetworkCredential("salimovkhayal@gmail.com", "iqepqgykafupydyf");
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtp.SendMailAsync(message);
                return true;
            }




            //Send
            //smtp.Host = _mailSettings.Host;
            //smtp.Port = _mailSettings.Port;
            //smtp.EnableSsl = _mailSettings.EnableSsl;

            //smtp.Credentials = new NetworkCredential(_mailSettings.Username, _mailSettings.Password);
            //smtp.Credentials = new NetworkCredential("salimovkhayal@gmail.com", "iqepqgykafupydyf");

            //smtp.UseDefaultCredentials = false;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //await smtp.SendMailAsync(message);
            //return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}
