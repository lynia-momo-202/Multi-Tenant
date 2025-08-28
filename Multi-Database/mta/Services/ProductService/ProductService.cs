using mta.DTOs;
using mta.Models;

namespace mta.Services.ProductService;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        var products = _context.Products.ToList();
        return products;
    }

    public Product CreateProduct(CreateProductRequest request)
    {
        var product = new Product()
        {
            Name = request.Name,
            Description = request.Description,
        };

        _context.Products.Add(product);
        _context.SaveChanges();

        return product;
    }

    public bool DeleteProduct(int Id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == Id);

        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();

            return true;
        }

        return false;
    }

}
