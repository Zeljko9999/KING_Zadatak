using Microsoft.Extensions.Options;
using ProductAPI.Models;
using ProductAPI.Exceptions;
using ProductAPI.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Controllers.DTO;
using System;
using Azure.Core;
using Microsoft.Identity.Client;

namespace ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _apiConnection;

        private readonly HttpClient _httpClient;

        // private readonly ProductAPIContext db;

        public ProductRepository(HttpClient httpClient /*, ProductAPIContext context*/)
        {
            _httpClient = httpClient;
            //db = context;
        }

        public async Task<List<ProductDTO>> GetAllProducts()
        {
            try
            {
                var response = await _httpClient.GetAsync("/products");

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(results))
                    {
                        throw new UserErrorMessage("Empty or invalid response received from API.");
                    }

                    var productResponse = JsonSerializer.Deserialize<ProductResponse>(results);

                    if (productResponse == null || productResponse.products == null)
                    {
                        throw new UserErrorMessage("No products found in the response.");
                    }

                    var productDTOs = productResponse.products.Select(p => new ProductDTO
                    {
                        Id = p.id,
                        Price = p.price,
                        Title = p.title,
                        Description = p.description,
                        Images = p.images

                    }).ToList();
                    return productDTOs;
                }
                else
                {
                    throw new UserErrorMessage("No response.");
                }
            }
            catch (Exception ex)
            {
                throw new UserErrorMessage($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/products/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(results))
                    {
                        throw new UserErrorMessage("Empty or invalid response received from API.");
                    }

                    var product= JsonSerializer.Deserialize<Product>(results);

                    if (product == null)
                    {
                        throw new UserErrorMessage("No product found in the response.");
                    }

                    return product;
                }
                else
                {
                    throw new UserErrorMessage($"No response for an product with Id = {id}.");
                }
            }
            catch (Exception ex)
            {
                throw new UserErrorMessage($"An error occurred: {ex.Message}");
            }
        }

        public async Task<List<ProductDTO>> GetProductsByCategoryAndMaxPrice(string category, double maxPrice)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/products/category/{category}");

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(results))
                    {
                        throw new UserErrorMessage("Empty or invalid response received from API.");
                    }

                    var productResponse = JsonSerializer.Deserialize<ProductResponse>(results);

                    if (productResponse?.products == null)
                    {
                        throw new UserErrorMessage("No products found in the response.");
                    }

                    var filteredProducts = productResponse.products
                        .Where(p => p.price <= maxPrice)
                        .Select(p => new ProductDTO
                        {
                            Id = p.id,
                            Price = p.price,
                            Title = p.title,
                            Description = p.description,
                            Images = p.images
                        })
                        .ToList();

                    return filteredProducts;
                }
                else
                {
                    throw new UserErrorMessage($"No response.");
                }
            }
            catch (Exception ex)
            {
                throw new UserErrorMessage($"An error occurred: {ex.Message}");
            }
        }

        public async Task<List<ProductDTO>> GetProductsByKeyword(string keyword)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/products/search?q={keyword}");

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(results))
                    {
                        throw new UserErrorMessage("Empty or invalid response received from API.");
                    }

                    var searchResponse = JsonSerializer.Deserialize<ProductResponse>(results);

                    if (searchResponse?.products == null)
                    {
                        throw new UserErrorMessage("No products found in the response.");
                    }

                    var productDTOs = searchResponse.products.Select(p => new ProductDTO
                    {
                        Id = p.id,
                        Price = p.price,
                        Title = p.title,
                        Description = p.description,
                        Images = p.images
                    }).ToList();

                    return productDTOs;
                }
                else
                {
                    throw new UserErrorMessage("No response.");
                }
            }
            catch (Exception ex)
            {
                throw new UserErrorMessage($"An error occurred: {ex.Message}");
            }
        }
    }
}
