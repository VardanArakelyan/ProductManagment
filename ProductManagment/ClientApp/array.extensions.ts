interface Array<T> {
    removeById(id: number): Array<T>;
    findIndexById(id: number): number;
}

Array.prototype.findIndexById = function (id: number) {
    for (let i = 0; i < this.length; i++) {
        if (this[i].Id == id) {
            return i;
        }
    }
    return -1;
}

Array.prototype.removeById = function (id: number) {
    const index = this.findIndexById(id);
    if (index !== -1) {
        this.splice(index, 1);
    }
    return this;
}