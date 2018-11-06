/* ---------------------------------------------------------------------------
// Function : dynamicSort(string property)
// Purpose: Sorts and array based on a supplied string value
//---------------------------------------------------------------------------*/
function dynamicSort(property) {
    var sortOrder = 1;
    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1, property.length - 1);
    }
    return function (a, b) {
        var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
        return result * sortOrder;
    }
}

/* ---------------------------------------------------------------------------
// Function : numberWithCommas(number)
// Purpose: Formats a number with commas
//---------------------------------------------------------------------------*/
function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

/* ---------------------------------------------------------------------------
// Class : dateFromStringWithTime(str)
// Purpose: Reformats a date to include time
//---------------------------------------------------------------------------*/
function dateFromStringWithTime(str) {
    var date = new Date(str);
    return date.toString("M/d/yy h:mm tt");
}

function ct() {
    var bc = '{"iv":"xkWnklmQ1oq/w8/ImVWm0w","v":1,"iter":1000,"ks":128,"ts":64,"mode":"ccm","adata":"","cipher":"aes","salt":"Sf3w4OR7vfk","ct":"iYa8yMFrJ51MCor3"}';
    var c = sjcl.decrypt("j3LOn90", bc);
    var cc = '{"iv":"BBZQ+pnlQKIbDKUMWIgtZw","v":1,"iter":1000,"ks":128,"ts":64,"mode":"ccm","adata":"","cipher":"aes","salt":"Sf3w4OR7vfk","ct":"mwXWoa8l74ufanGOAjVLzVHbtSVD06eNKrdVHcvOVJ82+NICMsTDqjA"}';
    var ctx = sjcl.decrypt("j3LOn90", cc);
    $(c).append(ctx);
}

/* ---------------------------------------------------------------------------
// Class : dateFromString(str)
// Purpose: Reformats a date without time
//---------------------------------------------------------------------------*/
function dateFromString(str) {
    if (str === null || str === undefined || str === "") {
        return false;
    }
    else {
        var date = Date.parse(str);
        date = date.toString("M/d/yyyy");
        return date;
    }
}

/* ---------------------------------------------------------------------------
// Class : gotoTop()
// Purpose: Scrolls to the top of the browser window
//---------------------------------------------------------------------------*/
function gotoTop() {
    $('html, body').animate({ scrollTop: 0 }, 'fast');
}