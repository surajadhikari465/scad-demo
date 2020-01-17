function deleteHC(hcId) {
    $("#dlgInfo").html('Manufacturer: <span style="color:black; font-weight:bolder">' + $('#manufacturerGrid').igGrid('getCellValue', hcId, 'ManufacturerName') + '</span>');
    $("#dlgMsg").text('Are you sure you would like to delete manufacturer?');

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
                    $("#dlgMsg").html('Deleting Manufacturer...');
                    $("#msgSpinner").show();

                    var request = $.ajax({
                        url: '/Manufacturer/Delete',
                        type: 'POST',
                        data: "{ 'hcId' : '" + hcId + "' }",
                        cache: false,
                        contentType: 'application/json; charset=utf-8',

                        success: function (response) {
                            $(refDialog).dialog("close"); //Comment this line if confirmation is shown
                            let grd = $('#manufacturerGrid').data('igGrid'); //Remove deleted record from UI without POST
                            grd.dataSource.deleteRow(hcId);
                            grd.commit();
                        },
                        error:
                            function (xhr, status, error) {
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgCancel').show();
                                $("#msgSpinner").hide();
                                try {
                                    $("#dlgMsg").html(JSON.parse(xhr.responseText));
                                }
                                catch (err) {
                                    if (err != null) {
                                        $("#dlgMsg").html(error);
                                    }
                                    else { $("#dlgMsg").html('Failed to delete'); }
                                }
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