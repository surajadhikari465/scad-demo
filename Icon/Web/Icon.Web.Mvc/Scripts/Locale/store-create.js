$(function () {
    $('#create-button').click(function () {
        $('#create-button').val('Creating Locale...');
        $('#create-button').disable(true);
    });

    // Prevent disabled buttons from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
});
