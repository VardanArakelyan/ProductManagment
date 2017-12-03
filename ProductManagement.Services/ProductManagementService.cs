using ProductManagement.Dal;
using System.Diagnostics;
using ProductManagement.Contracts.Entities;
using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private IProductManagementDal _productManagementDal;

        public ProductManagementService()
        {
            // Dependency injection avoided for simplification reasons
            _productManagementDal = new ProductManagementDal();
        }

        public async Task<int> GetCount(string filterName)
        {
            return await _productManagementDal.GetCount(filterName);
        }

        public async Task<List<Product>> GetProducts(int pageIndex, int count, string nameFilter = null)
        {
            if (count < 1)
            {
                throw new Exception($"count {count} cannot be less then 1");
            }
            int limitStart = (pageIndex - 1) * count;
            var products = await _productManagementDal.GetProducts(limitStart, count, nameFilter);
            return products;
        }

        public async Task Delete(Product product)
        {
            if (product.Id == null)
            {
                throw new Exception($"product id {product.Id} should not be null");
            }
            else if (product.Id < 0)
            {
                throw new Exception($"product id {product.Id} is negative");
            }
            await _productManagementDal.DeleteById(product.Id.Value);
        }

        public async Task<int> Save(Product product)
        {
            List<int> matchingIds = await _productManagementDal.GetSimilarProductIds(product);
            int savedProductId;
            ValidateProduct(product);
            // Update
            if (product.Id != null)
            {
                if (matchingIds == null)
                    throw new Exception("Product doesn't exist");
                if (matchingIds.Count != 1 || !matchingIds.Contains(product.Id.Value))
                {
                    throw new DuplicateElementException("Similar product already exists");
                }
                savedProductId = await _productManagementDal.Update(product);
            }
            // Insert
            else
            {
                if (matchingIds?.Count != 0)
                {
                    throw new DuplicateElementException("Similar product already exists");
                }
                savedProductId = await _productManagementDal.Insert(product);
            }
            return savedProductId;
        }

        private void ValidateProduct(Product productToValidate)
        {
            if (string.IsNullOrEmpty(productToValidate.Name))
            {
                throw new Exception("Product name cannot be null");
            }
        }

        public async Task GenerateRandomItems(int count)
        {
            await _productManagementDal.GenerateRandomProducts(count);
        }
    }
}
