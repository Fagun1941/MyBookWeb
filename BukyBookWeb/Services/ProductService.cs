using BukyBookWeb.Models;
using BukyBookWeb.Repositories;

namespace BukyBookWeb.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;
        private readonly string _imageFolder;

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
            _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            if (!Directory.Exists(_imageFolder))
                Directory.CreateDirectory(_imageFolder);
        }

        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public Product GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Product product, IFormFile? file)
        {
            HandleFileUpload(product, file);
            _repository.Add(product);
        }

        public void Update(Product product, IFormFile? file)
        {
            HandleFileUpload(product, file);
            _repository.Update(product);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _repository.GetCategories();
        }

        // Private helper to handle image upload
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
    }
}
