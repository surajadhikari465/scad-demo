//urlForRedrawPagingAction variable must be set by razor view!

function redrawPagingSucceeded(responseHtml) {
    if (typeof console !== "undefined") console.log("redrawPagingSucceeded ajax call succeeded");

    $("#pagingDiv").html(responseHtml);
}

function redrawPagingFailed(e) {
    if (typeof console !== "undefined") {
        console.log("redrawPagingFailed ajax call error");
        console.log(e);
    }
}

function redrawPaging(partialViewHtml) {
    if (typeof console !== "undefined") console.log("redrawPaging() called");
   
    if (partialViewHtml) {
        var xmlDoc = $.parseXML(partialViewHtml);
        var divWithData = xmlDoc.firstChild;
        if (divWithData) {
            var page = divWithData.getAttribute("data-page");
            var pageSize = divWithData.getAttribute("data-pageSize");
            var appName = divWithData.getAttribute("data-appName");
            var errorLevel = $("#ErrorLevel").find("option:selected").text();
            $.ajax({
                type: "GET",
                url: urlForRedrawPagingAction,
                data: { appName: appName, page: page, pageSize: pageSize, errorLevel: errorLevel },
                success: redrawPagingSucceeded,
                error: redrawPagingFailed
            });
        }
    }
}