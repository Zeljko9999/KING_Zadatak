using Microsoft.AspNetCore.Identity.Data;
using ProductAPI.Models;
using ProductAPI.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using ProductAPI.Exceptions;
using ProductAPI.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ProductAPI.Controllers.DTO;
using System;
using Azure.Core;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ProductAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;

        public AuthRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponse> Login(Login loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/auth/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponse>();
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }
        }

        public async Task<User> GetUser([FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                // Create HttpClient instance
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await httpClient.GetAsync("https://dummyjson.com/auth/me");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        User user = JsonSerializer.Deserialize<User>(responseBody);

                        if (user == null)
                        {
                            throw new UserErrorMessage("Failed to deserialize user object from API response.");
                        }

                        return user;
                    }
                    else
                    {
                        throw new UserErrorMessage($"Failed to get user. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new UserErrorMessage($"HTTP request error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new UserErrorMessage($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new UserErrorMessage($"An error occurred: {ex.Message}");
            }
        }
    }
}
