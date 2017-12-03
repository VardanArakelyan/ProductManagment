module ProductManagement {
    export class ProductController {
        products: Product[];
        currentProduct: Product;
        allProductsCount: number;
        curentPage: number;
        pagesCount: number;
        nameFilter: string;
        readonly pageItemsCount: number;
        static $inject = ["DataAccessService", "$q"];

        constructor(private readonly dataAccessService: DataAccessService, private readonly $q: ng.IQService) {
            this.currentProduct = new Product();

            this.pageItemsCount = 10;
            this.nameFilter = "";
        }

        $onInit() {
            this.curentPage = 1;
            this.getDataByPage(1);

            

        }

        private calculatePagesCount() {
            this.pagesCount = Math.ceil(this.allProductsCount / this.pageItemsCount);
        }

        private getDataByPage(page: number) {
            this.dataAccessService.getProducts(page, this.pageItemsCount, this.nameFilter)
                .then((productsPage: ProductPage) => {
                    this.products = productsPage.Products;
                    this.allProductsCount = productsPage.AllItemsCount;
                    this.calculatePagesCount();
                }
                );
        }

        previusPage() {
            if (this.curentPage > 1) {
                this.curentPage--;
                this.getDataByPage(this.curentPage);
            }
        }

        nextPage() {
            if (this.curentPage < this.pagesCount) {
                this.curentPage++;
                this.getDataByPage(this.curentPage);
            }
        }

        addProduct() {
            this.dataAccessService.saveProduct(this.currentProduct)
                .then((id: number) => {
                    this.currentProduct.Id = id;
                    this.products.push(this.currentProduct);
                    this.currentProduct = new Product;

                    this.allProductsCount++;
                    this.calculatePagesCount();
                },
                (reason) => {
                    let exMsg = "Add product faild. ";
                    if (reason.data && reason.data.Message)
                        exMsg += reason.data.Message;
                    alert(exMsg);
                }
                );
        }

        updateProduct(productForUpdate: Product) {
            this.dataAccessService.saveProduct(productForUpdate)
                .then(() => {
                }
                ,
                (reason) => {
                    let exMsg = "Save product faild. ";
                    if (reason.data && reason.data.Message)
                        exMsg += reason.data.Message;
                    alert(exMsg);
                }
                );
        }

        deleteProduct(productForDelete: Product) {
            this.dataAccessService.deleteProduct(productForDelete)
                .then(() => {
                    this.products.removeById(productForDelete.Id)

                    this.allProductsCount--;
                    this.calculatePagesCount();
                });
        }

        searchByName() {
            this.getDataByPage(this.curentPage);
        }

        generateRandomProducts() {
            this.dataAccessService.generateRandomProducts()
                .then(() => {
                    this.getDataByPage(1);
                }
                );
        }
    }
}