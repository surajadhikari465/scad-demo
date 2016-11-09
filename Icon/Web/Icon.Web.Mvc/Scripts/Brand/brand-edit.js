$(function () {

    $('#saveButton').click(function () {
        $('#saveButton').disable(true);
    });

    $('#brandName').on('input propertychange paste', function () {
        $('#saveButton').disable(false);
    });

    $('#brandAbbreviation').on('input propertychange paste', function () {
        $('#saveButton').disable(false);
    });

    // Prevent disabled buttons from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });

});