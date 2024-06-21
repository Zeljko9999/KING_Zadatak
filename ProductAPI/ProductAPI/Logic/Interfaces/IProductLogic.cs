using ProductAPI.Controllers.DTO;
using ProductAPI.Models;

namespace ProductAPI.Logic.Interfaces
{
    public interface IProductLogic
    {
        Task<List<ProductDTO>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<List<ProductDTO>> GetProductsByCategoryAndPrice(string category, double maxPrice);
        Task<List<ProductDTO>> GetProductsByWord(string word);

    }
}
