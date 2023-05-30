import { validateNumberOfDecimals } from './shared.js'

//Infragistics requires custom validators to be in the 
//global namespace so adding this to the window object unfortunately.
window['validateNumberOfDecimals'] = validateNumberOfDecimals;

$(document).ready(function () {
    $(":submit").removeAttr("disabled");

    $('form').submit(function () {
        let isValid = true;

        if ($('[name="ItemViewModel.ItemAttributes[IMPSynchronized]"]').val() == 'Yes') {
            $('#impSyncedAlert').show();
            return false;
        } else {
            $('#impSyncedAlert').hide();
        }

        if ($(':focus') != null) {
            $(':focus').blur(); //Remove focuse to force saving changes
        }

        $("[class*=ui-igvalidator-target").each(function () {
            try { isValid = $(this).igValidator("isValid"); }
            catch (err) { console.log('Infragistics Validation Error: ' + err.errorMessage); }

            if (!isValid) return false;
        });

        if (isValid) {
            $(this).validate();
            if ($(this).valid()) {
                $(':submit', this).attr('disabled', true).val('Processing...');
                $('#divBlock').show();
            }
        }
    });

    $(window).keydown(function (e) {
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
});
