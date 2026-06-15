using System.Net;
using System.Net.Mail;

namespace InternalAuditSystem.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string toName, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private string _ip = "";
        private int _port = 587;
        private string _password = "";
        private string _userName = "";
        private const string _fromName = "Internal Audit System";

        public EmailService(IWebHostEnvironment env)
        {
            string fileName = Path.Combine(env.ContentRootPath, "server.txt");

            if (!File.Exists(fileName))
            {
                Console.WriteLine("ERROR: server.txt not found!");
                return;
            }

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] splitLine = line.Split('=');
                    if (splitLine[0].Trim() == "ip") _ip = splitLine[1].Trim();
                    if (splitLine[0].Trim() == "port") _port = Convert.ToInt32(splitLine[1].Trim());
                    if (splitLine[0].Trim() == "username") _userName = splitLine[1].Trim();
                    if (splitLine[0].Trim() == "password") _password = splitLine[1].Trim();
                }
            }
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string body)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient(_ip, _port);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_userName, _password.TrimEnd());
                SmtpServer.EnableSsl = true;

                MailMessage Mailmsg = new MailMessage();
                Mailmsg.From = new MailAddress(_userName, _fromName);
                Mailmsg.To.Add(new MailAddress(toEmail, toName));
                Mailmsg.Subject = subject;
                Mailmsg.Body = body;
                Mailmsg.IsBodyHtml = true;

                await SmtpServer.SendMailAsync(Mailmsg);

                Console.WriteLine($"Email sent successfully to: {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMTP Error: {ex.Message}");
                Console.WriteLine($"Inner: {ex.InnerException?.Message}");
                throw; 
            }
        }
    }
}