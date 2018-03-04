using System.Threading.Tasks;

namespace FableFortuneCardList.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
