using System.Collections.Generic;

namespace SolarCoffee.Services.Product
{
    public interface IProductService
    {
        public List<Data.Models.Product> GetAllProducts();
        public Data.Models.Product GetProductById(int id);
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product);
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id);
    }
}