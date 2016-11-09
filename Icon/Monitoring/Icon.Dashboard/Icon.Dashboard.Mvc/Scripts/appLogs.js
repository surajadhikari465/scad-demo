
// following variables must be set from server-side Razor code
//intervalMilliseconds
//urlForGetRecentErrorsAction 
//arrayOfFunctions 
var currentFunctionIndex = 0;

function getRecentErrorsSucceeded(responseHtml) {
    if (typeof console !== "undefined") console.log("getRecentErrors ajax call succeeded");
    var xmlDoc = $.parseXML(responseHtml);
    var idForContent = xmlDoc.firstChild.id;
    var idForTarget = idForContent.replace('Content', '');
    $('#' + idForTarget).html(responseHtml);
}

function getRecentErrorsFailed(e) {
    if (typeof console !== "undefined") {
        console.log("getRecentErrors ajax call error");
        console.log(e);
    }
}

function getRecentErrors(appID, hours) {
    $.ajax({
        type: "GET",
        url: urlForGetRecentErrorsAction,
        data: { appID: appID, hours: hours },
        success: getRecentErrorsSucceeded,
        error: getRecentErrorsFailed
    });
}

function launchRequests() {
    if (arrayOfFunctions.length > currentFunctionIndex) {
        arrayOfFunctions[currentFunctionIndex].call();
        currentFunctionIndex++;
    } else {
        currentFunctionIndex = 0;
    }
}

$(document).ready(function () {
    buildArrayOfFunctions();
    window.setInterval(launchRequests, intervalMilliseconds);
});