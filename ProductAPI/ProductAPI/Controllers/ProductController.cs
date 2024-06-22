﻿using Microsoft.AspNetCore.Http;
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

        // Endpoint GetAllProducts gets list of all products with short details about them
        // Example of GET request: http://localhost:5077/api/product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productLogic.GetProducts();
            return Ok(products);
        }

        // Endpoint GetProductById gets all details about one product that is seacrhed by product ID (integer)
        // Example of GET request: http://localhost:5077/api/product/2
        // If the product with the requested ID does not exist, we will receive
        // a response message that the product with the requested ID does not exist
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

        //Endpoint GetProductsByCategoryAndMaxPrice filters products by category (string) and maximum price (double).
        //Some of categories are: beauty, fragrances, furniture, groceries, ...
        //Example of GET request: http://localhost:5077/api/product/filter?category=beauty&maxPrice=20
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

        //Endpoint GetProductsByKeyword searches and gets products by keyword (string).
        //For example word "laptop" will retreive all products related to laptops
        //Example of GET request: http://localhost:5077/api/product/search?keyword=laptop
        [HttpGet("search")]
        public async Task<IActionResult> GetProductsByKeyword([FromQuery] string keyword)
        {
            try
            {
                var products = await _productLogic.GetProductsByWord(keyword);
                return Ok(products);
            }
            catch (UserErrorMessage ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
