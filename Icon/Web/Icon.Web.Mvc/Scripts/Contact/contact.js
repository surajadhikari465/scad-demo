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

                let isDisabled = $('#btnSubmit').is(':disabled');
                let isBlock = $('#msgBlock') == null ? false : $('#msgBlock').is(':visible');
                if (!isDisabled && !isBlock) {
                    $("#btnSubmit").click();
                }
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
        show: { effect: "slide", duration: 300 },
        hide: { effect: "fade", duration: 300 },
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

function deleteContact(contactId) {
    $("#dlgContact").html('Contact Type: <span style="color:black; font-weight:bolder">' + $('#contactGrid').igGrid('getCellValue', contactId, 'ContactName') + '</span>');
    $("#dlgMsg").text('Are you sure you would like to delete contact?');

    $("#dlgConfirm").dialog({
        resizable: false,
        height: 'auto',
        width: 450,
        modal: true,
        closeOnEscape: true,
        show: { effect: "drop", duration: 300 },
        hide: { effect: "fade", duration: 300 },
        open: function () {
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").text("User Confirmation Required");

            $(".ui-dialog-buttonpane").show();
            $(".ui-dialog-buttonpane").children().show();
            $(".ui-dialog-titlebar").css("background-color", "gainsboro");
        },
        buttons: {
            'Delete':
            {
                id: 'dlgDelete',
                text: 'Delete',
                click: function () {
                    let refDialog = this; //Ref to dialog so we can close it on success
                    $(".ui-dialog-buttonset").hide();
                    $(".ui-dialog-titlebar-close").hide();
                    $(".ui-dialog-buttonset").children().hide();
                    $(".ui-dialog-title").text("Processing Request...");
                    $("#dlgMsg").html('Deleting Contact Type... &nbsp<span class="spinner-border float-right ml-auto text-primary" role="status" aria-hidden="true" style="color: red" />');

                    let contactUrl = '/Contact/DeleteContact';
                    var request = $.ajax({
                        url: contactUrl,
                        type: 'POST',
                        data: "{ 'contactId' : '" + contactId + "' }",
                        cache: false,
                        contentType: 'application/json; charset=utf-8',

                        success: function (response) {
                            /* //Keep the code below if we decide to show confirmation
                            $(".ui-dialog-titlebar").css("background-color", response.success ? "lightGreen" : "red");
                            $(".ui-dialog-title").text("System Information");
                            $(".ui-dialog-buttonset").show();
                            $('#dlgCancel').text('Close').show();
                            $("#dlgMsg").text(response.responseText);*/

                            $(refDialog).dialog("close"); //Comment this line if confirmation is shown
                            /*Full grid update with POST if full refresh needed
                            let grd = $('#contactGrid');
                            grd.igGrid('dataBind'); */

                            let grd = $('#contactGrid').data('igGrid'); //remove deleted record from UI without POST
                            grd.dataSource.deleteRow(contactId);
                            grd.commit();
                        },
                        error:
                            function (xhr, status, error) {
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
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