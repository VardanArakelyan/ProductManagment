module ProductManagement {
    var appModule = angular.module("productApp",[])
        .controller("ProductController", ProductController)
        .service("DataAccessService", DataAccessService)
        .component("productItem", new ProductItemComponent());
}