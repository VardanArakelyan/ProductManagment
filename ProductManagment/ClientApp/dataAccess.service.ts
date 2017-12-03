module ProductManagement {
    export class DataAccessService {
        private readonly $http: ng.IHttpService;
        private readonly $q: ng.IQService;

        static $inject = ["$http", "$q"];

        constructor($http: ng.IHttpService, $q: ng.IQService) {
            this.$http = $http;
            this.$q = $q;
        }

        private handleError(data, status) {
            //TODO: should be handled by error page
            console.log(`status: ${status}, data: ${data} `)
        }

        getProducts(pageIndex: number, itemsCountPerPage: number, filterName : string = null ) {
            var defered = this.$q.defer();
            var url = `/api/productdata/products/${pageIndex}/${itemsCountPerPage}/`;
            if (filterName)
                url += filterName;
           
            this.$http.get(url)
                .then(response => {
                    defered.resolve(response.data);
                })
                .catch(reason => {
                    this.handleError(reason.data, reason.status);
                    defered.reject(reason);
                })

            return defered.promise;
        }

        deleteProduct(productForDelete: Product) {
            var defered = this.$q.defer();
            this.$http.post(`/api/productdata/product/delete/`, productForDelete)
                .then(response => {
                    defered.resolve(response.data);
                })
                .catch(reason => {
                    this.handleError(reason.data, reason.status);
                    defered.reject(reason);
                })

            return defered.promise;
        }

        saveProduct(product: Product) {
            var defered = this.$q.defer();
            this.$http.post(`/api/productdata/products/save`, product)
                .then(response => {
                    defered.resolve(response.data);
                })
                .catch(reason => {
                    this.handleError(reason.data, reason.status);
                    defered.reject(reason);
                })

            return defered.promise;
        }

        generateRandomProducts() {
            var defered = this.$q.defer();
            this.$http.post(`/api/productdata/products/generaterandomproducts`, null)
                .then(response => {
                    defered.resolve(response.data);
                })
                .catch(reason => {
                    this.handleError(reason.data, reason.status);
                    defered.reject(reason);
                })

            return defered.promise;
        }
    }
}