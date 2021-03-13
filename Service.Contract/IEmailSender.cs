using System.Threading.Tasks;

namespace Service.Contract
{
    /// <summary>
    /// provide method for send email
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Send a email with the given email, subject and message
        /// </summary>
        /// <param name="email">value of the email</param>
        /// <param name="subject">value of the subject</param>
        /// <param name="message">value of the message</param>
        /// <returns></returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}
