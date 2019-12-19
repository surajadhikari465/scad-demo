$(document).ready(function() {
    $(":submit").removeAttr("disabled");

    $('form').submit(function () {
        $(this).validate();
        if ($(this).valid()) {
            $(':submit', this).attr('disabled', true).val('Processing...');
            $('#divBlock').show();
        }

        // Prevent button from being clicked like a link.
        $('body').on('click', 'a.disabled', function (event) {
            event.preventDefault();
        });
    });
});
