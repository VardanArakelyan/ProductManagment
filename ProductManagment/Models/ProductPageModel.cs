using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagement.Models
{
    public class ProductPageModel
    {
        public int AllItemsCount { get; set; }
        public IList<ProductModel> Products { get; set; }
        public ProductPageModel(int count, IList<ProductModel> products)
        {
            AllItemsCount = count;
            Products = products;
        }
    }
}