$(document).ready(function() {
    $("#saveButton").click(saveButtonClick);
});

function saveButtonClick() {
    $('#saveButton').val('Saving Changes...');
    $('#saveButton').disable(true);

    // Prevent button from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
}
