function editType(contactTypeId, archived) {
    let name = contactTypeId <= 0 ? '' : $('#contactGrid').igGrid('getCellValue', contactTypeId, 'ContactTypeName');
    $("#dlgMsg").text('');

    $("#dialogContactType").dialog({
        resizable: false,
        height: 'auto',
        width: 450,
        modal: true,
        closeOnEscape: true,
        show: { effect: "slide", duration: 300 },
        hide: { effect: "fade", duration: 300 },
        open: function () {
            $("#contactTypeName").val(name);
            $("#contactTypeName").prop('disabled', false);
            $("#contactTypeName").focus();
            
            $("#cbArchive").prop('checked', archived);
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").text("Contact Type");
            $(".ui-dialog-buttonpane").show();
            $(".ui-dialog-buttonpane").children().show();
            $(".ui-dialog-titlebar").css("background-color", "gainsboro");
        },
        buttons: {
            'Update':
            {
                id: 'dlgSave',
                text: 'Save',
                click: function () {
                    let refDialog = this; //Ref to dialog so we can close it on success
                    let isArchived = $("#cbArchive").is(":checked");
                    $("#dlgMsg").text('');
                    let name = $("#contactTypeName").val().trim().replace(/  +/g, ' ');
                    if (name.length == 0) return;

                    $("#contactTypeName").val(name);
                    $("#contactTypeName").prop('disabled', true);
                    $(".ui-dialog-buttonset").hide();
                    $(".ui-dialog-titlebar-close").hide();
                    $(".ui-dialog-buttonset").children().hide();
                    $(".ui-dialog-title").text("Processing Request...");

                    var request = $.ajax({
                        url: '/Contact/AddUpdateContactType',
                        type: 'POST',
                        data: "{ 'contactTypeName': '" + name + "', 'contactTypeId': " + contactTypeId + ", 'isArchived': " + isArchived + "}",
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
                            $(refDialog).dialog("close");
                            var grd = $('#contactGrid');
                            grd.igGrid('dataBind');
                        },
                        error:
                            function (xhr, status, error) {
                                $("#contactTypeName").prop('disabled', false);
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgSave').show();
                                $('#dlgCancel').show();
                                $("#dlgTypeMsg").text(JSON.parse(xhr.responseText));
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

function deleteType(contactTypeId) {
    $("#dlgType").html('Contact Type: <span style="color:black; font-weight:bolder">' + $('#contactGrid').igGrid('getCellValue', contactTypeId, 'ContactTypeName') + '</span>');
    $("#dlgTypeMsg").text('All Contacts associated with this Contact Type will be deleted permanently. Are you sure you would like to proceed?');

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

                    let contactUrl = '/Contact/DeleteContactType';
                    var request = $.ajax({
                        url: contactUrl,
                        type: 'POST',
                        data: "{ 'contactTypeId' : '" + contactTypeId + "' }",
                        cache: false,
                        contentType: 'application/json; charset=utf-8',

                        success: function (response) {
                            /* //Keep the code below if we decide to show confirmation
                            $(".ui-dialog-titlebar").css("background-color", response.success ? "lightGreen" : "red");
                            $(".ui-dialog-title").text("System Information");
                            $(".ui-dialog-buttonset").show();
                            $('#dlgCancel').text('Close').show();
                            $("#dlgTypeMsg").text(response.responseText);*/

                            /*Full grid update with POST if full refresh needed
                            var grd = $('#contactGrid');
                            grd.igGrid('dataBind');*/

                            var grd = $('#contactGrid').data('igGrid'); //remove deleted record from UI without POST
                            grd.dataSource.deleteRow(contactTypeId);
                            grd.commit();
                            $(refDialog).dialog("close");
                        },
                        error:
                            function (xhr, status, error) {
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgCancel').show();
                                $("#dlgTypeMsg").text(JSON.parse(xhr.responseText));
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

function archiveType(contactTypeId, archived) {
    let name = $('#contactGrid').igGrid('getCellValue', contactTypeId, 'ContactTypeName');
    $("#dlgType").html('Contact Type: <span style="color:black; font-weight:bolder">' + $('#contactGrid').igGrid('getCellValue', contactTypeId, 'ContactTypeName') + '</span>');
    if (archived) {
        $("#dlgTypeMsg").text('Are you sure you would like to enable Contact Type?');
    }
    else {
        $("#dlgTypeMsg").text('Contact Type will be disabled and will not be available when creating new contacts. Are you sure you would like to proceed?');
    }

    $("#dlgConfirm").dialog({
        resizable: false,
        height: 'auto',
        width: 450,
        modal: true,
        closeOnEscape: true,
        open: function () {
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").text("User Confirmation Required");

            $(".ui-dialog-buttonpane").show();
            $(".ui-dialog-buttonpane").children().show();
            $(".ui-dialog-titlebar").css("background-color", "gainsboro");
        },
        buttons: {
            'Archive':
            {
                id: 'dlgArchive',
                text: archived ? 'Activate' : 'Disable',
                click: function () {
                    let refDialog = this; //Ref to dialog so we can close it on success
                    $(".ui-dialog-buttonset").hide();
                    $(".ui-dialog-titlebar-close").hide();
                    $(".ui-dialog-buttonset").children().hide();
                    $(".ui-dialog-title").text("Processing Request...");
                    $("#dlgTypeMsg").html('Processing Contact Type... &nbsp<span class="spinner-border float-right ml-auto text-primary" role="status" aria-hidden="true" style="color: red" />');

                    let contactUrl = '/Contact/AddUpdateContactType';
                    var request = $.ajax({
                        url: contactUrl,
                        type: 'POST',
                        data: "{ 'contactTypeName': '" + name + "', 'contactTypeId': " + contactTypeId + ", 'isArchived': " + !archived + "}",
                        cache: false,
                        contentType: 'application/json; charset=utf-8',

                        success: function (response) {
                            /* //Keep the code below if we decide to show confirmation
                            $(".ui-dialog-titlebar").css("background-color", response.success ? "lightGreen" : "red");
                            $(".ui-dialog-title").text("System Information");
                            $(".ui-dialog-buttonset").show();
                            $('#dlgCancel').text('Close').show();
                            $("#dlgTypeMsg").text(response.responseText);*/

                            $(refDialog).dialog("close");
                            var grd = $('#contactGrid');
                            grd.igGrid('dataBind');
                        },
                        error:
                            function (xhr, status, error) {
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgCancel').show();
                                $('#dlgCancel').show();
                                $("#dlgTypeMsg").text(JSON.parse(xhr.responseText));
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