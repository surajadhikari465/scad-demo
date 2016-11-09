
$(function () {
    rowEditSaveChanges();
});

function rowEditSaveChanges() {
    $("#pluCategoryGrid").on("iggridupdatingeditrowended", function (evt, ui) {
        if (ui.update) {
            $("#pluCategoryGrid").igGrid("saveChanges",
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
    var grid = $('#pluCategoryGrid');
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
        var plucategoryList = { plucategoryList: $('#pluCategoryGrid').igGrid('option', 'dataSource').Records };
        $.fileDownload('Excel/PluCategoryExport', {
            httpMethod: "POST",
            data: plucategoryList
        });
    });
 });
