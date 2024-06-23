using Microsoft.Extensions.Options;
using ProductAPI.Models;
using ProductAPI.Exceptions;
using ProductAPI.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ProductAPI.Logic.Interfaces;
using ProductAPI.Controllers.DTO;
using System;
using ProductAPI.Repositories.Interfaces;

namespace ProductAPI.Logic
{
    public class ProductLogic : IProductLogic
    {
        private readonly IProductRepository _productRepository;

        public ProductLogic(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDTO>> GetProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return products;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            return product;
        }

        public async Task<List<ProductDTO>> GetProductsByCategoryAndPrice(string category, double maxPrice)
        {
            var products = await _productRepository.GetProductsByCategoryAndMaxPrice(category, maxPrice);
            return products;
        }

        public async Task<List<ProductDTO>> GetProductsByWord(string word)
        {
            var products = await _productRepository.GetProductsByKeyword(word);
            return products;
        }
    }
}
