using System.Threading.Tasks;

namespace Service.Contract
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
