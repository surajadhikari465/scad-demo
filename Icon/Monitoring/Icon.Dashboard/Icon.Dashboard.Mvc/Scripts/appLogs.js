
// following variables must be set from server-side Razor code
//intervalMilliseconds
//urlForGetRecentErrorsAction 
//arrayOfFunctions
//urlForTableRefresh
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

function reloadLogTableSucceeded(responseHtml) {
    if (typeof console !== "undefined") console.log("reloadLogTableSucceeded ajax call succeeded");
    $("#pagingDataTable").html(responseHtml);
    redrawPaging(responseHtml);
}

function reloadLogTableFailed(e) {
    if (typeof console !== "undefined") {
        console.log("reloadLogTableFailed ajax call error");
        console.log(e);
    }
} 

function errorLevelDropdownChanged() {
    if (typeof console !== "undefined") console.log("errorLevelDropdownChanged called");
    var appName = $("#appName").val();
    var page = $("#page").val();
    var pageSize = $("#pageSize").val();
    var errorLevel = $(this).find("option:selected").text();
    $.ajax({
        type: "GET",
        url: urlForTableRefresh,
        data: { appName: appName, page: page, pageSize: pageSize, errorLevel: errorLevel },
        success: reloadLogTableSucceeded,
        error: reloadLogTableFailed
    });
}

$(document).ready(function () {
    $("#ErrorLevel").change(errorLevelDropdownChanged);
    buildArrayOfFunctions();
    window.setInterval(launchRequests, intervalMilliseconds);
});