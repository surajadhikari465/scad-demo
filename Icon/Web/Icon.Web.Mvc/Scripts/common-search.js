function displayModelValidation(result) {

    // Display the search options with included validation errors.
    $('#searchOptions').html(result.responseText);

    enableSearchButton();

    $("input[data-icon-autocomplete]").each(applyAutocomplete);
}

function searchButtonClick() {

    $('#search-button').val('Searching...');
    $('#search-button').disable(true);

    // Prevent the search button from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });

    // Clear any create item alert messages if shown
    $('#createItemAlert').remove();
}

function enableSearchButton() {

    $('#search-button').val('Search');
    $('#search-button').disable(false);
}

// Helper function to allow toggling of the 'disabled' CSS class.
jQuery.fn.extend({
    disable: function (state) {
        return this.each(function () {
            var $this = $(this);
            $this.toggleClass('disabled', state);
        });
    }
});

function getSelectedRows() {
    var selectedRows = $("#igGrid").igGridSelection("selectedRows");
    var rowDataObject = new Array();

    for (var i = 0; i < selectedRows.length; i++) {
        rowDataObject[i] = $("#igGrid").igGrid("findRecordByKey", selectedRows[i].element.data().id);
    }

    return rowDataObject;
}

function displayAlertMessage(message, isError) {
    var alertClass = null;
    if (isError) {
        alertClass = "alert-danger";
    } else {
        alertClass = "alert-success";
    }
    var alertMessage = '<div class="alert ' + alertClass + ' alert-dismissable page-subsection">' +
                            '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                            '<strong class="alertMessage">' + message + '</strong>' +
                        '</div>';
    $('.search-alert').html(alertMessage);
}

function alertMessage(alertId, message, isError) {
    var alertClass = null;
    if (isError) {
        alertClass = "alert-danger";
    } else {
        alertClass = "alert-success";
    }
    var alertMessage = '<div class="alert ' + alertClass + ' alert-dismissable page-subsection">' +
                            '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                            '<strong class="alertMessage">' + message + '</strong>' +
                        '</div>';
    $("#" + alertId).html(alertMessage);
}

function isNullOrEmpty(str) {
    return (str === '' || str === null);
}