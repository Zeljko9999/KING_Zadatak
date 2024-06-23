using ProductAPI.Models;
using System.ComponentModel;
using ProductAPI.Controllers.DTO;
using Microsoft.AspNetCore.Identity.Data;

namespace ProductAPI.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthResponse> Login(Login loginRequest);
        Task<User> GetUser(string token);
    }
}