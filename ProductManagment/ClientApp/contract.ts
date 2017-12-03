module ProductManagement {
    export class Product {
        Id: number;
        Code: number;
        Name: string;
        Price: number;
        Barcode: string;

        constructor() {
            this.Id = null;
        }
    }  

    export class ProductPage {
        AllItemsCount: number;
        Products: Product[];
    }
}