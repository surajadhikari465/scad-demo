$(document).ready(function () {
    $(":submit").removeAttr("disabled");

    $('form').submit(function () {
        if ($('#msgBlock') == null && $('#divBlock') == null) {
            $(this).validate();

            if ($(this).valid()) {
                $(':submit', this).attr('disabled', true).val('Processing...');
                $('#divBlock').show();
            }
        }
    });
});

function closeDialog() {
    $('#msgBlock').remove();
}

$(window).keydown(function(e) {
    if (e.keyCode == 13) {
        e.preventDefault();
        return false;
    }

    if (e.ctrlKey || e.metaKey) {
        switch (String.fromCharCode(e.which).toLowerCase()) {
            case 's':
                e.preventDefault();
                var element = $(':focus'); //Focused control
                if (element != null) $(':focus').blur(); //Remove focuse to force saving changes
                $("#btnSubmit").click();
                break;
        }
    }
});