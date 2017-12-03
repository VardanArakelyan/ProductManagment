using ProductManagement.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Dal
{
    public interface IProductManagementDal
    {
        Task DeleteById(int productId);
        Task<int> Insert(Product product);
        Task<int> Update(Product product);
        Task<List<Product>> GetProducts(int limitStart, int rowsCount, string nameFilter = null);
        Task<List<int>> GetSimilarProductIds(Product productToMatch);
        Task<int> GetCount(string filterName);
        Task GenerateRandomProducts(int countToGenerate);
    }
}
