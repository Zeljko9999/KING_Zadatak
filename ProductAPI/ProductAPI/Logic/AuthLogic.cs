using ProductAPI.Logic.Interfaces;
using ProductAPI.Models;
using Microsoft.Extensions.Options;
using ProductAPI.Exceptions;
using ProductAPI.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ProductAPI.Controllers.DTO;
using System;
using ProductAPI.Repositories.Interfaces;

namespace ProductAPI.Logic
{
    public class AuthLogic : IAuthLogic
    {
        private readonly IAuthRepository _authRepository;

        public AuthLogic(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<AuthResponse> Login(Login loginRequest)
        {
            var login = await _authRepository.Login(loginRequest);
            return login;
        }

        public async Task<User> GetUser(string token)
        {
            var user = await _authRepository.GetUser(token);
            return user;
        }
    }
}
