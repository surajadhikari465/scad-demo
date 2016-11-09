$(function () {
    $('#addButton').click(function () {
        $('#addButton').disable(true);
        $('#removeButton').disable(true);

        var message = "Please wait... transmitting new exclusion to R10.";
        displayWarning(message);
    });

    $('#removeButton').click(function () {
        $('#addButton').disable(true);
        $('#removeButton').disable(true);

        var message = "Please wait... transmitting exclusion removal to R10.";
        displayWarning(message);
    });

    $('#newExclusion').on('input propertychange paste', function () {
        $('#addButton').disable(false);
        $('#removeButton').disable(false);
    });
});