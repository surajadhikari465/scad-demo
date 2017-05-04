/* functions for use in the ESB Environment editor template,
*  when dynamically adding & removing rows for the child collection of Applications */

function removeRow(e) {
    $(e).parent().parent().parent().parent().parent().remove();

    var rowContainerDiv = $("#applicationsPanelBody");

    //renumber mvc fields for binding
    $(rowContainerDiv).find(".applicationRow").each(function (i) {
        //rename some stuff so the mvc binding will work
        $(this).find("input").each(function (j) {
            var existingNameVal = $(this).attr("name").slice($(this).attr("name").indexOf(".") - $(this).attr("name").length + 1);
            var existingIdVal = $(this).attr("id").slice($(this).attr("id").indexOf("__") - $(this).attr("id").length + 2);

            $(this).attr("name", "Applications[" + i + "]." + existingNameVal);
            $(this).attr("id", "Applications_" + i + "__" + existingIdVal);
        });
        $(this).find("span").each(function (i) {
            var existingVal = $(this).attr("data-valmsg-for");
            $(this).attr("data-valmsg-for", "Applications[" + i + "]." + existingVal);
        });
    });
}

function addRow(e) {
    //variable should have been set in view
    var url = urlToAddEsbEnvironmentApp;
  
    //call action to add html for adding a row          
    $.ajax({
        type: "GET",
        url: url,
        success: addApplicationSucceeded,
        error: addApplicationFailed
    });
}

function addApplicationSucceeded(html) {

    $("#rowForAddApplicationButton").before(html);
    var justAddedDiv = $("#rowForAddApplicationButton").prev();

    //get count of rows/applications
    var index = $(".applicationRow").length - 1;
    // loop through dropdown--set the attributes 
    $(justAddedDiv).find("select").each(function (i) {
        var existingNameVal = $(this).attr('name');
        var existingIdVal = $(this).attr('id');
        $(this).attr("name", "Applications[" + index + "]." + existingNameVal);
        $(this).attr("id", "Applications_" + index + "__" + existingIdVal);
    });
    //rename some stuff in the added row so the mvc binding will work
    $(justAddedDiv).find("input").each(function (i) {
        var existingNameVal = $(this).attr('name');
        var existingIdVal = $(this).attr('id');
      
        $(this).attr("name", "Applications[" + index + "]." + existingNameVal);
        $(this).attr("id", "Applications_" + index + "__" + existingIdVal);
    });


    $(justAddedDiv).find("span").each(function (i) {
        var existingVal = $(this).attr('data-valmsg-for');
        $(this).attr("data-valmsg-for", "Applications[" + index + "]." + existingVal);
    });
}

function addApplicationFailed(e) {
    if (typeof console !== "undefined") console.log("addApplicationFailed ajax call failed");
}