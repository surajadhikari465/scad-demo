/// <reference path="../typings/jquery/jquery.d.ts" />
class PriceManager {
    public editButton: Object
    constructor(editButtonId: string) {
        this.editButton = $("#" + editButtonId);
    }
}

var pm = new PriceManager("blah");
pm.editButton