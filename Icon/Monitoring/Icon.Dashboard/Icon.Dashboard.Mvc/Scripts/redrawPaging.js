//urlForRedrawPagingAction variable must be set by razor view!

function redrawPagingSucceeded(responseHtml) {
    if (typeof console !== "undefined") console.log("redrawPagingSucceeded ajax call succeeded");
    $("#pagingDiv").html(responseHtml);
}

function redrawPagingFailed(e) {
    console.log("redrawPagingFailed ajax call error");
    if (typeof console !== "undefined") { console.log(e); }
}

function redrawPaging(partialViewHtml) {
    if (typeof console !== "undefined") console.log("redrawPaging() called");
    if (partialViewHtml) {
        var xmlDoc = $.parseXML(partialViewHtml);
        var divWithData = xmlDoc.firstChild;
        if (divWithData) {
            var page = divWithData.getAttribute("data-page");
            var pageSize = divWithData.getAttribute("data-pageSize");
            var id = divWithData.getAttribute("data-route-parameter");
            $.ajax({
                type: "GET",
                url: urlForRedrawPagingAction,
                data: { routeParameter: id, page: page, pageSize: pageSize },
                success: redrawPagingSucceeded,
                error: redrawPagingFailed
            });
        }
    }
}