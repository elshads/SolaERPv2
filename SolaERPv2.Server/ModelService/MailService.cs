using System.Net;
using System.Net.Mail;

namespace SolaERPv2.Server.ModelService;

public class MailService
{
    MailSettings _mailSettings;
    AppSettings _appSettings;
    public MailService(MailSettings mailSettings, AppSettings appSettings)
    {
        _mailSettings = mailSettings;
        _appSettings = appSettings;
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
                                    <style type=""text/css""></style>
                                </head>
                                <body>
                                    <div style=""font-family: 'Nunito Sans', sans-serif; font-size: 0.875rem;"">" + htmlBody + @"</div>
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

    public async Task<bool> SendApprovalMail(IEnumerable<string> toAddresses, string subject, string fullName, string userName, string destination, string documentNumber)
    {
        var htmlBody = $"<h3 style=\"color: #0f62fe;\">Example Approve is Pending</h3>" +
            $"<div style=\"color: #9c9c9c;\">This is an automatically generated email - please do not reply</div><br/>" +
            $"<div>Dear <span style=\"font-weight: 700;\">{fullName}</span>,</div><br/>" +
            $"<div>Example action #<a style=\"color: #0f62fe;\" href=\"{_appSettings.AppUrl}/payments/approve?doc={documentNumber}\">{documentNumber}</a> is pending for your approvals</div>" +
            $"<div>Buyer: <span style=\"font-weight: 700;\">{userName}</span></div>" +
            $"<div>Requester: <span style=\"font-weight: 700;\">{userName}</span></div>" +
            $"<div>Destination: <span style=\"font-weight: 700;\">{destination}</span></div><br/>" +
            $"<div>Please login to <a style=\"color: #0f62fe;\" href=\"{_appSettings.AppUrl}\">Sola ERP</a> to see all awaiting approvals</div><br/>" +
            $"<div>The Apertech team.</div><br/><div style=\"font-size: 0.75rem\">&#169; 2022 Sola ERP SOCAR-STP All rights reserved.</div>";

        return await SendMail(toAddresses, subject, htmlBody);
    }
}
