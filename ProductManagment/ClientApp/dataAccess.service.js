var ProductManagement;
(function (ProductManagement) {
    var DataAccessService = (function () {
        function DataAccessService($http, $q) {
            this.$http = $http;
            this.$q = $q;
        }
        DataAccessService.prototype.handleError = function (data, status) {
            //TODO: should be handled by error page
            console.log("status: " + status + ", data: " + data + " ");
        };
        DataAccessService.prototype.getProducts = function (pageIndex, itemsCountPerPage, filterName) {
            var _this = this;
            if (filterName === void 0) { filterName = null; }
            var defered = this.$q.defer();
            var url = "/api/productdata/products/" + pageIndex + "/" + itemsCountPerPage + "/";
            if (filterName)
                url += filterName;
            this.$http.get(url)
                .then(function (response) {
                defered.resolve(response.data);
            })
                .catch(function (reason) {
                _this.handleError(reason.data, reason.status);
                defered.reject(reason);
            });
            return defered.promise;
        };
        DataAccessService.prototype.deleteProduct = function (productForDelete) {
            var _this = this;
            var defered = this.$q.defer();
            this.$http.post("/api/productdata/product/delete/", productForDelete)
                .then(function (response) {
                defered.resolve(response.data);
            })
                .catch(function (reason) {
                _this.handleError(reason.data, reason.status);
                defered.reject(reason);
            });
            return defered.promise;
        };
        DataAccessService.prototype.saveProduct = function (product) {
            var _this = this;
            var defered = this.$q.defer();
            this.$http.post("/api/productdata/products/save", product)
                .then(function (response) {
                defered.resolve(response.data);
            })
                .catch(function (reason) {
                _this.handleError(reason.data, reason.status);
                defered.reject(reason);
            });
            return defered.promise;
        };
        DataAccessService.prototype.generateRandomProducts = function () {
            var _this = this;
            var defered = this.$q.defer();
            this.$http.post("/api/productdata/products/generaterandomproducts", null)
                .then(function (response) {
                defered.resolve(response.data);
            })
                .catch(function (reason) {
                _this.handleError(reason.data, reason.status);
                defered.reject(reason);
            });
            return defered.promise;
        };
        return DataAccessService;
    }());
    DataAccessService.$inject = ["$http", "$q"];
    ProductManagement.DataAccessService = DataAccessService;
})(ProductManagement || (ProductManagement = {}));
//# sourceMappingURL=dataAccess.service.js.map