using Assignment.API.Model.Dto.Request;
using Assignment.API.Model.Dto.Response;
using Assignment.API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.API.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] string searchQuery = "", // Optional search query
            [FromQuery] int pageNumber = 1,      // Default page number is 1
            [FromQuery] int pageSize = 10        // Default page size is 10
        )
        {
            var pagedProducts = await _productService.GetAllProductsAsync(searchQuery, pageNumber, pageSize);

            return Ok(new ResponseDto<PagedResponseDto<ProductResponseDto>>
            {
                Result = pagedProducts,
                IsSuccess = true,
                Message = "Products retrieved successfully"
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new ResponseDto<ProductResponseDto>
                {
                    IsSuccess = false,
                    Message = "Product not found"
                });
            }
            return Ok(new ResponseDto<ProductResponseDto>
            {
                Result = product,
                IsSuccess = true,
                Message = "Product retrieved successfully"
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequestDto productDto)
        {
            var createdProduct = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id },
                new ResponseDto<ProductResponseDto>
                {
                    Result = createdProduct,
                    IsSuccess = true,
                    Message = "Product created successfully"
                });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequestDto productDto)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
            if (updatedProduct == null)
            {
                return NotFound(new ResponseDto<ProductResponseDto>
                {
                    IsSuccess = false,
                    Message = "Product not found"
                });
            }

            return Ok(new ResponseDto<ProductResponseDto>
            {
                Result = updatedProduct,
                IsSuccess = true,
                Message = "Product updated successfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    Message = "Product not found"
                });
            }

            return Ok(new ResponseDto<bool>
            {
                Result = success,
                IsSuccess = true,
                Message = "Product deleted successfully"
            });
        }
    }
}
