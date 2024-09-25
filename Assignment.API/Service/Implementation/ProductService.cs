using Assignment.API.Data;
using Assignment.API.Entity;
using Assignment.API.Model.Dto.Request;
using Assignment.API.Model.Dto.Response;
using Assignment.API.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Assignment.API.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponseDto<ProductResponseDto>> GetAllProductsAsync(string searchQuery, int pageNumber, int pageSize)
        {
            // Start by querying all products
            var query = _context.Products.AsQueryable();

            // Apply filtering (searchQuery)
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchQuery) || p.Description.ToLower().Contains(searchQuery));
            }

            // Get total number of items before pagination
            var totalItems = await query.CountAsync();

            // Apply pagination (Skip and Take)
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to DTOs
            var productDtos = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
            }).ToList();

            // Create a PagedResponseDto
            return new PagedResponseDto<ProductResponseDto>
            {
                Items = productDtos,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ProductResponseDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                
            };
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductRequestDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                
            };
        }

        public async Task<ProductResponseDto> UpdateProductAsync(int id, ProductRequestDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
           

            await _context.SaveChangesAsync();

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
