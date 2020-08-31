import { validateNumberOfDecimals } from './shared.js'

//Infragistics requires custom validators to be in the 
//global namespace so adding this to the window object unfortunately.
window['validateNumberOfDecimals'] = validateNumberOfDecimals;

$(document).ready(function () {
    $(":submit").removeAttr("disabled");

    $('form').submit(function () {
        let isValid = true;

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

    const scanCodeTypeUpcId = "#scancodetype_upc";
    const scanCodeTypeUpcInput = scanCodeTypeUpcId + " input";
    const scanCodeTypePluId = "#scancodetype_plu";
    const scanCodeTypePluSelect = scanCodeTypePluId + " select";

    var processScanCodeTypeChange =()=> {
        var $elem = $('input[type=radio][name="ScanCodeType"]');
        if ($elem[0].checked) {
           
            $(scanCodeTypeUpcInput).removeAttr("disabled");

            $(scanCodeTypePluSelect).attr("disabled", "disabled");
            $(scanCodeTypePluId).hide();

            //Update the scan code text editor to be required
            let scanCode = $("#ScanCode").first();
            scanCode.igTextEditor("option", "validatorOptions", {
                required: {
                    errorMessage: "Scan Code is required when choosing UPC",
                }, pattern: {
                    expression: "^[1-9]\\d{0,12}$",
                    errorMessage: "Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long"
                }
            });

            //Update the PLU combobox to not be required
            let plu = $("#BarcodeTypes").first();
            plu.igCombo("option", "validatorOptions", {
                required: false
            });
        } else {
            $(scanCodeTypeUpcInput).attr("disabled", "disabled");

            $(scanCodeTypePluSelect).removeAttr("disabled");
            $(scanCodeTypePluId).show();

            //Update the scan code text editor to not be required
            let scanCode = $("#ScanCode").first();
            scanCode.igTextEditor("option", "validatorOptions", { required: false });

            //Update the PLU combobox to be required
            let plu = $("#BarcodeTypes").first();
            plu.igCombo("option", "validatorOptions", {
                required: true
            });
        }

        $("#SkuId").igCombo("option", "locale", { placeHolder: "Enter text to search" });
    };

    $('input[type="radio"][name="ScanCodeType"]').change(function () {
        processScanCodeTypeChange();
    });

    // init
    processScanCodeTypeChange();
});

$(function () {
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