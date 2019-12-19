
$(function () {
    rowEditSaveChanges();
});

function rowEditSaveChanges() {
    $("#barcodeTypeGrid").on("iggridupdatingeditrowended", function (evt, ui) {
        if (ui.update) {
            $("#barcodeTypeGrid").igGrid("saveChanges",
                function saveSuccess(data) {
                    if (data.Success === true) {
                        displayAlertMessage('Successfully updated item.', false);
                    }
                    else {
                        displayAlertMessage(data.Error, true);                        
                        rollbackPendingGridChanges();
                    }
                },
                function saveFailure(data) {
                    displayAlertMessage(data.Error, true);                    
                    rollbackPendingGridChanges();
                });
        }
    });
}

function rollbackPendingGridChanges() {
    var grid = $('#barcodeTypeGrid');
    var updates = $.extend({}, grid.data('igGrid').pendingTransactions());
    $.each(updates, function (index, transaction) {
        grid.igGrid("rollback", transaction.rowId, true);
    });
}



function onSuccess(data) {
    var message;
    var isError;

    if (data.Success) {
        message = "Update was applied successfully.";
        isError = false;
        displayAlertMessage(message, isError);
    } else {        
        isError = true;
        displayAlertMessage(data.Message, isError);
    }
}

function onError(data) {
    displayAlertMessage(data.Message, true);
}

$(document).ready(function () {
    $('#export').click(function () {
        var barcodeTypeList = { barcodeTypeList: $('#barcodeTypeGrid').igGrid('option', 'dataSource').Records };
        $.fileDownload('Excel/BarcodeTypeExport', {
            httpMethod: "POST",
            data: barcodeTypeList
        });
    });
 });
