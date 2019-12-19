$(document).ready(function () {
    $(":submit").removeAttr("disabled");

    $('form').submit(function () {
        $(this).validate();

        if ($(this).valid()) {
            $(':submit', this).attr('disabled', true).val('Processing...');
            $('#divBlock').show();
        }
    });
});