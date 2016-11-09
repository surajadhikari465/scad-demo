$(function () {

    $('#createButton').click(function () {
        $('#createButton').disable(true);
    });

    $('#agenctName').on('input propertychange paste', function () {
        $('#createButton').disable(false);
    });

    // Prevent disabled buttons from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });

});