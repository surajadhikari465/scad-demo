$(function () {
    $('#addButton').click(function () {
        $('#addButton').disable(true);
        $('#addButton').val('Adding...');
        $('#removeButton').disable(true);

        var message = "Please wait... transmitting new mapping to R10.";
        displayWarning(message);
    });

    $('#removeButton').click(function () {
        $('#removeButton').disable(true);
        $('#removeButton').val('Removing...');
        $('#addButton').disable(true);

        var message = "Please wait... transmitting mapping removal to R10.";
        displayWarning(message);
    });
});
