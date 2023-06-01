using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace BackMonoLegal.NotificationAdapter.EmailNotification
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendEmail(EmailDTO request)
        {
            string host = configuration.GetSection("EmailConfiguration:Host").Value;
            string userName = configuration.GetSection("EmailConfiguration:UserName").Value;
            string password = configuration.GetSection("EmailConfiguration:Password").Value;
            int port = configuration.GetValue<int>("EmailConfiguration:Port");


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(userName));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(host, port, SecureSocketOptions.StartTls);
            smtp.Authenticate(userName, password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }

}
