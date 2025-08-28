using Microsoft.AspNetCore.Mvc;
using mta.DTOs;
using mta.Models;
using mta.Services.ProductService;

namespace mta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService; // inject the products service
        }

        // Get list of products
        [HttpGet("get")]
        public IActionResult Get()
        {
            var list = _productService.GetAllProducts();
            return Ok(list);
        }

        // Create a new product
        [HttpPost("create")]
        public ActionResult<Product> Post(CreateProductRequest request)
        {
            var result = _productService.CreateProduct(request);
            return result;
        }

        // Delete a product by id
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _productService.DeleteProduct(id);
            return Ok(result);
        }

    }
}
