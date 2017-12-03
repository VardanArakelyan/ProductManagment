var ProductManagement;
(function (ProductManagement) {
    var appModule = angular.module("productApp", [])
        .controller("ProductController", ProductManagement.ProductController)
        .service("DataAccessService", ProductManagement.DataAccessService)
        .component("productItem", new ProductManagement.ProductItemComponent());
})(ProductManagement || (ProductManagement = {}));
//# sourceMappingURL=product.app.js.map