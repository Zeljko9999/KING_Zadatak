using ProductAPI.Models;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Exceptions;
using ProductAPI.Configuration;
using System.Text.Json;
using ProductAPI.Controllers.DTO;
using System;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ProductAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(HttpClient httpClient, ILogger<AuthRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<AuthResponse> Login(Login loginRequest)
        {
            _logger.LogInformation("Login attempt for username: {Username}", loginRequest.Username);

            var response = await _httpClient.PostAsJsonAsync("/auth/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                _logger.LogInformation("Login successful for username: {Username}", loginRequest.Username);
                return authResponse;
            }
            else
            {
                _logger.LogWarning("Login failed for username: {Username}. Status code: {StatusCode}", loginRequest.Username, response.StatusCode);
                throw new UnauthorizedAccessException("Invalid username or password");
            }
        }

        public async Task<User> GetUser([FromHeader(Name = "Authorization")] string token)
        {
            _logger.LogInformation("Attempting to get user with provided token");

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.GetAsync("https://dummyjson.com/auth/me");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    User user = JsonSerializer.Deserialize<User>(responseBody);

                    if (user == null)
                    {
                        _logger.LogWarning("Failed to deserialize user object from API response.");
                        throw new UserErrorMessage("Failed to deserialize user object from API response.");
                    }

                    _logger.LogInformation("Successfully retrieved user data");
                    return user;
                }
                else
                {
                    _logger.LogWarning("Failed to get user. Status code: {StatusCode}", response.StatusCode);
                    throw new UserErrorMessage($"Failed to get user. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while getting user");
                throw new UserErrorMessage($"HTTP request error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error while getting user");
                throw new UserErrorMessage($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting user");
                throw new UserErrorMessage($"An error occurred: {ex.Message}");
            }
        }
    }
}