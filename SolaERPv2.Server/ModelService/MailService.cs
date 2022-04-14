using System.Net;
using System.Net.Mail;

namespace SolaERPv2.Server.ModelService;

public class MailService
{
    MailSettings _mailSettings;
    public MailService(MailSettings mailSettings)
    {
        _mailSettings = mailSettings;
    }

    public async Task<bool> SendMail(IEnumerable<string> toAddresses, string subject, string htmlBody)
    {
        try
        {
            using var message = new MailMessage();
            var smtp = new SmtpClient();

            string body = @"
                            <html lang=""en"">
                                <head>    
                                    <meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"">
                                    <title>Sola ERP</title>
                                    <link rel=""preconnect"" href=""https://fonts.googleapis.com"">
                                    <link rel = ""preconnect"" href = ""https://fonts.gstatic.com"" crossorigin >
                                    <link href = ""https://fonts.googleapis.com/css2?family=Nunito+Sans:ital,wght@0,300;0,400;0,600;0,700;1,300;1,400;1,600;1,700&display=swap"" rel = ""stylesheet"" >
                                    <style type=""text/css"">
                                        .body-content{font-family: 'Nunito Sans', sans-serif;font-size: 1rem;}
                                    </style>
                                </head>
                                <body>
                                    <div class=""body-content"">" + htmlBody + @"</div>
                                </body>
                            </html>
                            ";

            message.IsBodyHtml = true;
            message.From = new MailAddress(_mailSettings?.FromEmail);
            foreach (var item in toAddresses)
            {
                message.To.Add(item);
            }
            message.Subject = subject;
            message.Body = body;
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
