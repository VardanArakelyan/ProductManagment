using ProductManagement.Contracts.Entities;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ProductManagment.Models
{

    public static class ProductConverter
    {
        public static ProductModel ToProductModel(this Product product)
        {
            return new ProductModel()
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Price = product.Price,
                Barcode = product.Barcode
            };
        }

        public static Product ToProduct(this ProductModel productModel)
        {
            return new Product()
            {
                Id = productModel.Id,
                Code = productModel.Code,
                Name = productModel.Name,
                Price = productModel.Price,
                Barcode = productModel.Barcode
            };
        }
    }
}