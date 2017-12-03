var ProductManagement;
(function (ProductManagement) {
    var ProductItemComponent = (function () {
        function ProductItemComponent() {
            this.bindings = {
                product: "="
            };
            this.require = { main: "^ngController" };
            this.templateUrl = "../../ClientApp/templates/ProductItem.html";
        }
        return ProductItemComponent;
    }());
    ProductManagement.ProductItemComponent = ProductItemComponent;
})(ProductManagement || (ProductManagement = {}));
//# sourceMappingURL=productItem.component.js.map