using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Exceptions;
using ProductAPI.Logic.Interfaces;
using ProductAPI.Models;
using Microsoft.Extensions.Logging;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductLogic _productLogic;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductLogic productLogic, ILogger<ProductController> logger)
        {
            this._productLogic = productLogic;
            _logger = logger;
        }

        // Endpoint GetAllProducts gets list of all products with short details about them
        // If User is not logged in & authorized, endpoint will return Notauthorized message
        // Example of GET request: http://localhost:5077/api/product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                _logger.LogWarning("Unauthorized access attempt to GetAllProducts");
                return Unauthorized("User not authorized");
            }

            _logger.LogInformation("User {Username} requested all products", user.username);

            var products = await _productLogic.GetProducts();
            return Ok(products);
        }

        // Endpoint GetProductById gets all details about one product that is seacrhed by product ID (integer)
        // Example of GET request: http://localhost:5077/api/product/2
        // If the product with the requested ID does not exist, we will receive
        // a response message that the product with the requested ID does not exist
        // If User is not logged in & authorized, endpoint will return Notauthorized message
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                _logger.LogWarning("Unauthorized access attempt to GetProductById");
                return Unauthorized("User not authorized");
            }

            _logger.LogInformation("User {Username} requested product with ID {ProductId}", user.username, id);
            var product = await _productLogic.GetProduct(id);
            if (product is null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound($"Could not find an product with ID = {id}");
            }
            else
            {
                return Ok(product);
            };
        }

        //Endpoint GetProductsByCategoryAndMaxPrice filters products by category (string) and maximum price (double).
        //Some of categories are: beauty, fragrances, furniture, groceries, ...
        //Example of GET request: http://localhost:5077/api/product/filter?category=beauty&maxPrice=20
        // If User is not logged in & authorized, endpoint will return Notauthorized message
        [HttpGet("filter")]
        public async Task<IActionResult> GetProductsByCategoryAndMaxPrice([FromQuery] string category, [FromQuery] double maxPrice)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                _logger.LogWarning("Unauthorized access attempt to GetProductsByCategoryAndMaxPrice");
                return Unauthorized("User not authorized");
            }

            _logger.LogInformation("User {Username} requested products by category {Category} and max price {MaxPrice}", user.username, category, maxPrice);

            try
            {
                var products = await _productLogic.GetProductsByCategoryAndPrice(category, maxPrice);
                return Ok(products);
            }
            catch (UserErrorMessage ex)
            {
                _logger.LogError(ex, "Error occurred while getting products by category {Category} and max price {MaxPrice}", category, maxPrice);
                return BadRequest(ex.Message);
            }
        }

        //Endpoint GetProductsByKeyword searches and gets products by keyword (string).
        //For example word "laptop" will retreive all products related to laptops
        //Example of GET request: http://localhost:5077/api/product/search?keyword=laptop
        // If User is not logged in & authorized, endpoint will return Notauthorized message
        [HttpGet("search")]
        public async Task<IActionResult> GetProductsByKeyword([FromQuery] string keyword)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                _logger.LogWarning("Unauthorized access attempt to GetProductsByKeyword");
                return Unauthorized("User not authorized");
            }

            _logger.LogInformation("User {Username} requested products by keyword {Keyword}", user.username, keyword);

            try
            {
                var products = await _productLogic.GetProductsByWord(keyword);
                return Ok(products);
            }
            catch (UserErrorMessage ex)
            {
                _logger.LogError(ex, "Error occurred while getting products by keyword {Keyword}", keyword);
                return BadRequest(ex.Message);
            }
        }
    }
}
