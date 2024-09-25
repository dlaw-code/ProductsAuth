using Assignment.API.Model;

namespace Assignment.API.Service.Interface
{
    public interface IEmailService
    {
        Task<bool> Send(Message message, string attachment = "");
    }
}
