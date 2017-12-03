using System.Collections.Generic;
using ProductManagement.Contracts.Entities;
using System.Threading.Tasks;

namespace ProductManagement.Services
{
    public interface IProductManagementService
    {
        Task Delete(Product product);
        Task<int> Save(Product product);
        Task<List<Product>> GetProducts(int pageIndex, int count, string nameFilter = null);
        Task<int> GetCount(string filterName);
        Task GenerateRandomItems(int count);
    }
}