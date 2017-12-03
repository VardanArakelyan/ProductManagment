using ProductManagement.Contracts.Entities;
using ProductManagement.Models;
using ProductManagement.Services;
using ProductManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductManagement.Controllers.Api
{
    [RoutePrefix("api/productdata")]
    public class ProductDataController : ApiController
    {
        private IProductManagementService _productManagementService;
        public ProductDataController()
        {
            _productManagementService = new ProductManagementService();
        }

        [HttpGet]
        [Route("products/{pageIndex}/{itemsCountPerPage}/{filterName?}")]
        public async Task<IHttpActionResult> GetProducts(int pageIndex, int itemsCountPerPage, string filterName = null)
        {
            try
            {
                var productModels = (await _productManagementService.GetProducts(pageIndex, itemsCountPerPage, filterName))
                    .Select(p => p.ToProductModel()).ToList();
                var productCount = await _productManagementService.GetCount(filterName);
                var productsPage = new ProductPageModel(productCount, productModels);
                return Ok(productsPage);
            }
            catch(Exception ex)
            {
                // Log exception
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("product/delete")]
        public async Task<IHttpActionResult> Delete(ProductModel productModel)
        {
            try
            {
                Product product = productModel.ToProduct();
                await _productManagementService.Delete(product);

                return Ok();
            }
            catch (Exception ex)
            {
                // Log exception
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("products/save")]
        public async Task<IHttpActionResult> Save(ProductModel newProductModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Model not valid.");
            }
            try
            {
                var newProduct = newProductModel.ToProduct();
                var id = await _productManagementService.Save(newProduct);

                return Ok(id);
            }
            catch(DuplicateElementException ex)
            {
                return BadRequest("Dublicated item");
            }
            catch (Exception ex)
            {
                // Log exception
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("products/generaterandomproducts")]
        public async Task<IHttpActionResult> GenerateRandomProducts()
        {
            try
            {
                await _productManagementService.GenerateRandomItems(50000);

                return Ok();
            }
            catch (Exception ex)
            {
                // Log exception
                return InternalServerError();
            }
        }
    }
}