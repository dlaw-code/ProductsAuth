using Assignment.API.Model.Dto.Request;
using Assignment.API.Model.Dto.Response;

namespace Assignment.API.Service.Interface
{
    public interface IProductService
    {
        
            Task<PagedResponseDto<ProductResponseDto>> GetAllProductsAsync(string searchQuery, int pageNumber, int pageSize);
            Task<ProductResponseDto> GetProductByIdAsync(int id);
            Task<ProductResponseDto> CreateProductAsync(ProductRequestDto productDto);
            Task<ProductResponseDto> UpdateProductAsync(int id, ProductRequestDto productDto);
            Task<bool> DeleteProductAsync(int id);
        


    }
}
