Array.prototype.findIndexById = function (id) {
    for (var i = 0; i < this.length; i++) {
        if (this[i].Id == id) {
            return i;
        }
    }
    return -1;
};
Array.prototype.removeById = function (id) {
    var index = this.findIndexById(id);
    if (index !== -1) {
        this.splice(index, 1);
    }
    return this;
};
//# sourceMappingURL=array.extensions.js.map