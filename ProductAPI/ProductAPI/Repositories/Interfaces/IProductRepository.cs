using ProductAPI.Models;
using System.ComponentModel;
using ProductAPI.Controllers.DTO;

namespace ProductAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductDTO>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<List<ProductDTO>> GetProductsByCategoryAndMaxPrice(string category, double maxPrice);
        Task<List<ProductDTO>> GetProductsByKeyword(string word);
    }
}