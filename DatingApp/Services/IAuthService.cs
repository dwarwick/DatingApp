// IAuthService interface for authentication
using System.Threading.Tasks;

namespace DatingApp.Services
{
    public interface IAuthService
    {
        Task<bool> Login(string username, string password);
        Task<(bool Success, string ErrorMessage)> Register(string username, string password);
    }
}
