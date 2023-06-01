
namespace BackMonoLegal.NotificationAdapter.EmailNotification
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
