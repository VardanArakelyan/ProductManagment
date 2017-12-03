module ProductManagement {
    export class ProductItemComponent implements ng.IComponentOptions {
        public require;
        public bindings;
        public templateUrl: string;

        constructor() {
            this.bindings = {
                product: "="
            };
            this.require = { main: "^ngController" };
            this.templateUrl = "../../ClientApp/templates/ProductItem.html";
        }
    }
}