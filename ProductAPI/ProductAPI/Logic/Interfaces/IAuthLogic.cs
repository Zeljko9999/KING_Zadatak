using ProductAPI.Models;

namespace ProductAPI.Logic.Interfaces
{
    public interface IAuthLogic
    {
        Task<AuthResponse> Login(Login loginRequest);
        Task<User> GetUser(string token);
    }
}
