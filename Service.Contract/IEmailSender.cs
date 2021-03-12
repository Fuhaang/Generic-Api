using System.Threading.Tasks;

namespace Service.Contract
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
