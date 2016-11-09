function applyEditableGrid() {
    $("#igGrid").on("iggridupdatingeditrowended", function (evt, ui) {
        if (ui.update) {
            $("#igGrid").igGrid("saveChanges",
                function saveSuccess(data) {
                    if (data.Success === true) {
                        displayAlertMessage('Successfully updated item.', false);
                    }
                    else {
                        displayAlertMessage(data.Error, true);
                        $('#igGrid').igGrid('rollback');
                    }
                },
                function saveFailure() {
                    displayAlertMessage('There was an unexpected error updating the item.', true);
                    $('#igGrid').igGrid('rollback');
                });
        }
        refreshValidateButtonForSelection();
    });
}

