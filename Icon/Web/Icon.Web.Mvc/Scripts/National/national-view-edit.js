$(function () {
    $("#saveButton").bind({
        click: function (e) {
            $("#nationalClassGrid").igHierarchicalGrid("saveChanges");
        }
    });

    $('#saveButton').click(function () {
        $('#saveButton').val('Saving...');
        $('#saveButton').disable(true);
    });

    // Prevent disabled buttons from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
});

function onSuccess(data) {
    var message;
    var isError;

    if (data.Success) {
        message = "Updates were applied successfully.";
        isError = false;
        displayAlertMessage(message, isError);
    } else {
        message = "Unable to apply updates.  Please refresh the grid and check for duplicate national class names or class codes.";
        isError = true;
        displayAlertMessage(message, isError);
    }

    enableSaveButton();
}

function onError() {
    var message = "Unable to apply updates.  Please refresh the grid and check for duplicate national class names or class codes.";
    var isError = true;

    displayAlertMessage(message, isError);

    enableSaveButton();
}

function enableSaveButton() {
    $('#saveButton').val('Save Changes');
    $('#saveButton').disable(false);
}