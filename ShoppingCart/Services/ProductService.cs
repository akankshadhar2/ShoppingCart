using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models.Domain;
using ShoppingCart.Models.DTO;
using ShoppingCart.ServiceContracts;

namespace ShoppingCart.Services
{
    public class ProductService : IProductService
    {
        private readonly ShoppingCartDbContext _context;

        public ProductService(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return null;
            }

            return new ProductDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity 
            };
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity 
            }).ToList();
        }

        public async Task<List<ProductDTO>> SearchProductsByNameAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<ProductDTO>();
            }

            var products = await _context.Products
              .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()))
              .ToListAsync();

            return products.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity 
            }).ToList();
        }

        public async Task<bool> CreateProductAsync(ProductDTO productDto)
        {
            try
            {
                var product = new Product
                {
                    ProductId = productDto.ProductId,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Quantity = productDto.Quantity 
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(Guid id, ProductDTO productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Quantity = productDto.Quantity; 

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
