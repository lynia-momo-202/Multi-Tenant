using mta.DTOs;
using mta.Models;

namespace mta.Services.ProductService;

public interface IProductService
{
    IEnumerable<Product> GetAllProducts();
    Product CreateProduct(CreateProductRequest request);
    bool DeleteProduct(int Id);
}
