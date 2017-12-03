var ProductManagement;
(function (ProductManagement) {
    var ProductController = (function () {
        function ProductController(dataAccessService, $q) {
            this.dataAccessService = dataAccessService;
            this.$q = $q;
            this.currentProduct = new ProductManagement.Product();
            this.pageItemsCount = 10;
            this.nameFilter = "";
        }
        ProductController.prototype.$onInit = function () {
            this.curentPage = 1;
            this.getDataByPage(1);
        };
        ProductController.prototype.calculatePagesCount = function () {
            this.pagesCount = Math.ceil(this.allProductsCount / this.pageItemsCount);
        };
        ProductController.prototype.getDataByPage = function (page) {
            var _this = this;
            this.dataAccessService.getProducts(page, this.pageItemsCount, this.nameFilter)
                .then(function (productsPage) {
                _this.products = productsPage.Products;
                _this.allProductsCount = productsPage.AllItemsCount;
                _this.calculatePagesCount();
            });
        };
        ProductController.prototype.previusPage = function () {
            if (this.curentPage > 1) {
                this.curentPage--;
                this.getDataByPage(this.curentPage);
            }
        };
        ProductController.prototype.nextPage = function () {
            if (this.curentPage < this.pagesCount) {
                this.curentPage++;
                this.getDataByPage(this.curentPage);
            }
        };
        ProductController.prototype.addProduct = function () {
            var _this = this;
            this.dataAccessService.saveProduct(this.currentProduct)
                .then(function (id) {
                _this.currentProduct.Id = id;
                _this.products.push(_this.currentProduct);
                _this.currentProduct = new ProductManagement.Product;
                _this.allProductsCount++;
                _this.calculatePagesCount();
            }, function (reason) {
                var exMsg = "Add product faild. ";
                if (reason.data && reason.data.Message)
                    exMsg += reason.data.Message;
                alert(exMsg);
            });
        };
        ProductController.prototype.updateProduct = function (productForUpdate) {
            this.dataAccessService.saveProduct(productForUpdate)
                .then(function () {
            }, function (reason) {
                var exMsg = "Save product faild. ";
                if (reason.data && reason.data.Message)
                    exMsg += reason.data.Message;
                alert(exMsg);
            });
        };
        ProductController.prototype.deleteProduct = function (productForDelete) {
            var _this = this;
            this.dataAccessService.deleteProduct(productForDelete)
                .then(function () {
                _this.products.removeById(productForDelete.Id);
                _this.allProductsCount--;
                _this.calculatePagesCount();
            });
        };
        ProductController.prototype.searchByName = function () {
            this.getDataByPage(this.curentPage);
        };
        ProductController.prototype.generateRandomProducts = function () {
            var _this = this;
            this.dataAccessService.generateRandomProducts()
                .then(function () {
                _this.getDataByPage(1);
            });
        };
        return ProductController;
    }());
    ProductController.$inject = ["DataAccessService", "$q"];
    ProductManagement.ProductController = ProductController;
})(ProductManagement || (ProductManagement = {}));
//# sourceMappingURL=product.controller.js.map