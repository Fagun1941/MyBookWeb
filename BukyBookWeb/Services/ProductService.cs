using BukyBookWeb.Models;
using BukyBookWeb.Repositories;

namespace BukyBookWeb.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly string _imageFolder;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
            _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            if (!Directory.Exists(_imageFolder))
                Directory.CreateDirectory(_imageFolder);
        }

        public IEnumerable<Product> GetAllProduct(string search, int page , int pageSize)
        {
             var products = _repository.GetAllProduct(search, page, pageSize);
            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products
                    .Where(c => c.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            return products;
        }

        public Product GetByIdProduct(int id)
        {
            return _repository.GetByIdProduct(id);
        }

        public void AddProduct(Product product, IFormFile? file)
        {
            HandleFileUpload(product, file);
            _repository.AddProduct(product);
        }

        public void UpdateProduct(Product product, IFormFile? file)
        {
            HandleFileUpload(product, file);
            _repository.UpdateProduct(product);
        }

        public void DeleteProduct(int id)
        {
            _repository.DeleteProduct(id);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _repository.GetCategories();
        }

        private void HandleFileUpload(Product product, IFormFile? file)
        {
            if (file == null || file.Length == 0) return;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_imageFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            product.ImageUrl = "/images/products/" + fileName;
        }

      

        public int GetTotalCountProduct(string search)
        {
            var productCount = _repository.GetTotalProductCount(search);

            return productCount;
        }
    }
}
