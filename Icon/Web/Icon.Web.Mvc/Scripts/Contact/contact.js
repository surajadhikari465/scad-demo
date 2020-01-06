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

function openContactType() {
    $("#dialogContactType").dialog({
        resizable: false,
        height: 'auto',
        width: 450,
        modal: true,
        closeOnEscape: false,
        open: function () {
            $("#dlgMsg").text('');
            $("#contactTypeName").val('');
            $("#contactTypeName").prop('disabled', false);
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").text("Contact Type");
            $(".ui-dialog-buttonpane").show();
            $(".ui-dialog-buttonpane").children().show();
            $(".ui-dialog-titlebar").css("background-color", "gainsboro");
        },
        buttons: {
            'Create':
            {
                id: 'dlgCreate',
                text: 'Create',
                click: function () {
                    let refDialog = this; //Ref to dialog so we can close it on success
                    let isExist = false;
                    $("#dlgMsg").text('');
                    let name = $("#contactTypeName").val().trim().replace(/  +/g, ' '); // $("#contactTypeName").val().trim().toLowerCase();
                    if (name.length == 0) return;

                    $("#contactTypeName").val(name);
                    name = name.replace(/ /g, '').toLowerCase(); 
                    //Validate if ContactType exists before POST
                    $('#ddlType option').each(function () {
                        if (name == this.text.replace(/ /g, '').toLowerCase()) {
                            isExist = true;
                            return false;
                        }
                    });
                    
                    if (isExist) {
                        $("#dlgMsg").text('Contact Type already exists.');
                        return;
                    }

                    name = $("#contactTypeName").val();
                    $("#contactTypeName").prop('disabled', true);
                    $(".ui-dialog-buttonset").hide();
                    $(".ui-dialog-titlebar-close").hide();
                    $(".ui-dialog-buttonset").children().hide();
                    $(".ui-dialog-title").text("Processing Request...");

                    var request = $.ajax({
                        url: '/Contact/AddUpdateContactType',
                        type: 'POST',
                        data: "{ 'contactTypeName' : '" + name + "' }",
                        cache: false,
                        contentType: 'application/json; charset=utf-8',

                        success: function (response) {
                            /* //Keep the code below if we decide to show confirmation
                            $(".ui-dialog-titlebar").css("background-color", response.success ? "lightGreen" : "red");
                            $(".ui-dialog-title").text("System Information");
                            $(".ui-dialog-buttonset").show();
                            $('#dlgCancel').text('Close').show();
                            $("#dlgMsg").text(response.responseText);
                            */

                            var ar = [{ text: response.ContactTypeName, value: response.ContactTypeId }]; //Init array with new ContactType and add all existing ContactTypes 
                            $('#ddlType option').each(function () {
                                ar.push({ text: this.text, value: this.value });
                            });

                            $('#ddlType').empty(); //Reinitilize the list with sorted items
                            ar.sort(function (a, b) { return a.text.toLowerCase() > b.text.toLowerCase() });
                            $.each(ar, function (index, item) {
                                $('#ddlType').append($("<option />").val(item.value).text(item.text)); //Add item
                            });

                            $('#ddlType').val(response.ContactTypeId); //Select newly created ContactType
                            $(refDialog).dialog("close");
                        },
                        error:
                            function (xhr, status, error) {
                                $("#contactTypeName").prop('disabled', false);
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgCreate').show();
                                $('#dlgCancel').show();
                                $("#dlgMsg").text(JSON.parse(xhr.responseText));
                            }
                    });
                }
            },
            'Cancel':
            {
                id: 'dlgCancel',
                text: 'Cancel',
                click: function () {
                    $(this).dialog("close");
                },
            }
        }
    });
}