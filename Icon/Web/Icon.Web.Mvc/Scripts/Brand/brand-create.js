$(function () {

    $('#createButton').click(function () {
        $('#createButton').disable(true);
    });

    $('#brandName').on('input propertychange paste', function () {
        $('#createButton').disable(false);
    });

    $('#brandAbbreviation').on('input propertychange paste', function () {
        $('#createButton').disable(false);
    });

    // Prevent disabled buttons from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
    
});