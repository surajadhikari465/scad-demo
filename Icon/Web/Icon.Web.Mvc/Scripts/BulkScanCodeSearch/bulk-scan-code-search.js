$(function () {
    
    $('#search-button').click(function () {
        $('#search-button').val('Searching...');
        $('#search-button').disable(true);
        $('#upload-button').disable(true);
    });

    $('#upload-button').click(function () {
        $('#upload-button').val('Uploading...');
        $('#upload-button').disable(true);
        $('#search-button').disable(true);
    });

    // Prevent disabled buttons from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });

    var grid = $('#igGrid');

    // If the grid is present, perform additional setup.
    if (grid[0]) {
        $('[name="validate-button"]').disable(true);
        $('[name="export-button"]').click(exportItems);
        $('#igGrid').on('iggridrendered', setColumnHeaderTooltips);

        applyEditableGrid();
        setupValidateButtonOnClickEvent();
    }
});

function validateRows() {
    $('[name="validate-button"]').disable(true);
    $('[name="export-button"]').disable(true);

    var rowDataObject = getSelectedRows();
    var request = $.ajax({
        url: '/Item/ValidateSelected',
        type: 'POST',
        data: JSON.stringify(rowDataObject),
        contentType: 'application/json; charset=utf-8'
    });

    request.done(function (data) {
        if (data.Success == true) {
            displayAlertMessage(data.Message, false);
            $('#search-button').click();
        } else {
            displayAlertMessage(data.Message, true);
        }

        $('[name="export-button"]').disable(false);
    });
}
