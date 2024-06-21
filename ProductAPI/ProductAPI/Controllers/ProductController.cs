using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Exceptions;
using ProductAPI.Logic.Interfaces;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductLogic _productLogic;

        public ProductController(IProductLogic productLogic)
        {
            this._productLogic = productLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productLogic.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productLogic.GetProduct(id);
            if (product is null)
            {
                return NotFound($"Could not find an product with ID = {id}");
            }
            else
            {
                return Ok(product);
            };
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetProductsByCategoryAndMaxPrice([FromQuery] string category, [FromQuery] double maxPrice)
        {
            try
            {
                var products = await _productLogic.GetProductsByCategoryAndPrice(category, maxPrice);
                return Ok(products);
            }
            catch (UserErrorMessage ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetProductsByKeyword([FromQuery] string q)
        {
            try
            {
                var products = await _productLogic.GetProductsByWord(q);
                return Ok(products);
            }
            catch (UserErrorMessage ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
