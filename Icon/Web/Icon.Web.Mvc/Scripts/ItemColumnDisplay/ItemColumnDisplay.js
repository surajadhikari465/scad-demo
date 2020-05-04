var listElement;
var list;


$(function () {
    listElement = document.getElementById("item-detail-column-list");
    CheckForMissingColumns();
});

function scrollToTop() {
    $("html, body").animate({ scrollTop: 0 }, "fast");
}

function scrollToBottom() {
    $("html, body").animate({ scrollTop: $(document).height() }, "fast");
}

function moveListItem(listElement, direction) {
    listElement.hide();
    listElement.toggleClass("blue-background-class");

    switch (direction) {
        case "top":
            $(listElement).parent().prepend(listElement);
            break;
        case "bottom":
            $(listElement).parent().append(listElement);
            break;
    }

    listElement.slideDown("fast", function () { listElement.toggleClass("blue-background-class") });
    enableSaveButton();

};

function CheckForMissingColumns() {
    $.ajax({
        url: "/ItemColumnDisplay/AddMissingColumnsToOrderTable",
        type: "POST",
        data: {},
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            RefreshList();
        },
        error:
            function (xhr, status, error) {
                console.log("error", error);
                return null;
            }
    });
}


function RefreshList() {
    $.ajax({
        url: "/ItemColumnDisplay/GetDisplayOrder",
        type: "GET",
        data: {},
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            $("#loadingSpan").hide();
            DrawList(response);
        },
        error:
            function (xhr, status, error) {
                console.log("error", error);
                return null;
            }
    });
};

function returnDataId(d) {
    return d.ColumnType + ":" + d.ReferenceId;
}

function sortAlphabetically(listElement) {
    listElement.children("li").sort(sort_li) // sort elements
        .appendTo(listElement); // append again to the list

    enableSaveButton();
}

function saveChanges() {

    const data = list.toArray();
    $.ajax({
        url: "/ItemColumnDisplay/SetDisplayOrder",
        type: "POST",
        data: parseDisplayOrderData(data),
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function () {
            disableSaveButton();
            showMessage("#successDiv", 3000);
        },
        error:
            function (xhr, status, error) {
                console.log(error);
                showMessage("#errorDiv", 6000);
                return null;
            }
    });
}

function showMessage(div, duration) {
    let msg = $(div);
    msg.slideDown("fast");
    setTimeout(function () { msg.slideUp("fast"); }, duration);
}

function parseDisplayOrderData(data) {
    let returnvalue = data.map((d, index) => {
        let parts = d.split(":");
        return { ColumnType: parts[0], ReferenceId: Number(parts[1]), ReferenceName: "", DisplayOrder: index }
    });
    return JSON.stringify(returnvalue);
}

function DrawList(data) {
    const controlsTemplate = document.getElementsByTagName("template")[0].innerHTML;
    if (list !== undefined) { list.destroy(); }
    if (data !== null) {
        listElement.innerHTML = ""; // clear items.
        if (!data) return;
        data.forEach(d => {
            let icon = d.ColumnType === "Hierarchy" ? "fa-sitemap" : "fa-cube";
            $(listElement).append(`<li class="list-group-item" data-name="${d.ReferenceName}" data-id="${returnDataId(d)}"><span><i class="fa ${icon} control-item" title="${d.ColumnType}"></i></span> ${d.ReferenceName}${controlsTemplate}</li>`);
        });

        list = new window.Sortable(listElement,
            {
                animation: 300,
                ghostClass: "blue-background-class",
                handle: ".drag-handle",
                onSort: function () {
                    enableSaveButton();
                }
            });
        disableSaveButton();
    }
}

// sort function callback that sorts on data-name
function sort_li(a, b) {
    return ($(b).data("name")) < ($(a).data("name")) ? 1 : -1;
}

function enableSaveButton() {
    $("#saveButton").prop("disabled", false).animate();
}
function disableSaveButton() {
    $("#saveButton").prop("disabled", true);
}