using ShoppingCart.Models.DTO;

using System.Threading.Tasks;

namespace ShoppingCart.ServiceContracts
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductByIdAsync(Guid productId);
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<List<ProductDTO>> SearchProductsByNameAsync(string searchTerm);
        Task<bool> CreateProductAsync(ProductDTO productDto);
        Task<bool> UpdateProductAsync(Guid id, ProductDTO productDto);
        Task<bool> DeleteProductAsync(Guid id);
    }
}

