using System.Threading.Tasks;

namespace Service.Contract
{
    /// <summary>
    /// provide method for send sms
    /// </summary>
    public interface ISmsSender
    {
        /// <summary>
        /// Send a sms with the given number and message
        /// </summary>
        /// <param name="number">value of the number</param>
        /// <param name="message">value of the message</param>
        /// <returns></returns>
        Task SendSmsAsync(string number, string message);
    }
}
