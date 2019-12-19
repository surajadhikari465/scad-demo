$(function () {
    $('#manufacturerName').on('input propertychange paste', function () {
        $('#createButton').disable(false);
    });

    $('#zipCode').on('input propertychange paste', function () {
        $('#createButton').disable(false);
    });

    $('#arCustomerId').on('input propertychange paste', function () {
        $('#createButton').disable(false);
    });

    // Prevent disabled buttons from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
});

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