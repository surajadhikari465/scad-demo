
var priceAssociationLineTable = null;
var selectedItem = null;
$(document).ready(function () {
    priceAssociationLineTable = $('#tblPriceLineAssociation').DataTable({
        "ajax": {
            "url": "/PriceLineAssociation/GetAllRelatedItems?priceLineId=" + priceLineId,
            "type": 'POST'
        },
        columns: [
            { data: "ScanCode", name: "Scan Code", autoWidth: false },
            {
                data: "IsPrimary", name: "Price Line Primary", autoWidth: true,
                render: function (data, type, row) {
                    if (row.IsPrimary == true) {
                        return '<input type = "checkbox" name = "PriceLinePrimary[' + row.ItemId + ']" checked disabled  />';
                    }
                    else {
                        return '<input type = "checkbox" name = "PriceLinePrimary[' + row.ItemId + ']" onchange="togglePrimary(this, ' + row.ItemId + ')" />';
                    }
                }
            },
            { data: "CustomerFriendlyDescription", name: "Customer Friendly Description", autoWidth: true },
            { data: "ProductDescription", name: "Product Description", autoWidth: true },
            { data: "ItemPack", name: "ItemPack", autoWidth: true },
            { data: "RetailSize", name: "Retail Size", autoWidth: true, searchable: false },
            { data: "UOM", name: "UOM", autoWidth: true, searchable: false }
        ],
        lengthMenu: [[20, 50, 100, -1], [20, 50, 100, 'All']],
        loadingRecords: "Loading price line association data... Please wait..."
    });
});

function togglePrimary(checkbox, itemId) {
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

            $('#dlgCancel').hide();
            $("#dlgMsg").html('<p>Primary Item is being changed, do you want to proceed?</p>');
        },
        buttons: {
            'Confirm':
            {
                id: 'dlgConfirm',
                text: 'Yes',
                click: function () {
                    let refDialog = this; //Ref to dialog so we can close it on success
                    $(".ui-dialog-buttonset").hide();
                    $(".ui-dialog-titlebar-close").hide();
                    $(".ui-dialog-buttonset").children().hide();
                    $(".ui-dialog-title").text("Processing Request...");
                    $("#dlgMsg").html('Changing Primary Item... &nbsp<span class="spinner-border float-right ml-auto text-primary" role="status" aria-hidden="true" style="color: red" />');

                    var request = $.ajax({
                        url: '/PriceLineAssociation/ChangePrimaryItem',
                        type: 'POST',
                        data: JSON.stringify({ itemId: itemId, priceLineId: priceLineId }),
                        cache: false,
                        contentType: 'application/json; charset=utf-8',

                        success: function (response) {
                            priceAssociationLineTable.ajax.reload();
                            $(refDialog).dialog("close");
                        },
                        error:
                            function (xhr, status, error) {
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgCancel').show();
                                $("#dlgMsg").html('');

                                try {
                                    $("#dlgMsg").text(JSON.parse(xhr.responseText));
                                }
                                catch (err) {
                                    $("#dlgMsg").text('Request failed: ' + error);
                                }
                            }
                    });
                }
            },
            'no':
            {
                id: 'dlgNo',
                text: 'No',
                click: function () {
                    checkbox.checked = false;
                    $(this).dialog("close");
                },
            },

            'Cancel':
            {
                id: 'dlgCancel',
                text: 'Cancel',
                click: function () {
                    checkbox.checked = false;
                    $(this).dialog("close");
                },
            }
        }
    });
}


function clearAddItemDialogBox() {
    selectedItem = null;
    $('#addButton').attr("disabled", true);
    $("#itemToAdd").text('');
    $("#previousPriceLine").text('');
    $("#previousPriceLinePanel").hide();
    $("#addItemMessagePanel").hide();
    $("#addItemMessage").text('');
    //$("#dlgAddItem").dialog("option", "height", 260);
}

function addItemToPriceLine() {
    var addEnablementInterval= null;
    $("#dlgAddItem").dialog({
        resizable: false,
        //height: 260,
        height: 'auto',
        width: '40%',
        modal: true,
        closeOnEscape: true,
        show: { effect: "drop", duration: 300 },
        hide: { effect: "fade", duration: 300 },
        open: function () {
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").text("Add Item To Price Line");

            $(".ui-dialog-buttonpane").show();
            $(".ui-dialog-buttonpane").children().show();
            $(".ui-dialog-titlebar").css("background-color", "gainsboro");

            $('#addButton').attr("disabled",true);
            clearAddItemDialogBox();
            $("#scanCodeToSearch").val('');

            addEnablementInterval = setInterval(function () {
                if (selectedItem != null && $("#scanCodeToSearch").val() != selectedItem.scanCode) {
                    clearAddItemDialogBox();
                }
            }, 250);

            $("#scanCodeToSearch").autocomplete({
                minLength: 3,
                select: function (event, ui) {
                    var scanCode = ui.item.value.scanCode;
                    $("#scanCodeToSearch").val(scanCode);
                    $("#itemToAdd").text(ui.item.value.description);                    
                    selectedItem = ui.item.value;
                    selectedItem.scanCode = scanCode

                    if (selectedItem.previousPriceLine != null) {
                        $("#dlgAddItem").dialog("option", "height", 380);

                        if (selectedItem.isPrimary && selectedItem.isLastInpreviousPriceLine == false) {
                            $('#addButton').attr("disabled", true);
                            $("#previousPriceLine").text(
                                'This scan code cannot be moved from: "'
                                + selectedItem.previousPriceLine
                                + '" because it is the primary item, and it is not the last item.');
                            $("#previousPriceLinePanel").show();
                            selectedItem = null;
                        }
                        else {
                            $('#addButton').attr("disabled", false);
                            $("#previousPriceLine").text(
                                'This scan code will move from:"'
                                + selectedItem.previousPriceLine
                                + '" to: "'
                                + priceModelDescription
                                + '".');
                            $("#previousPriceLinePanel").show();
                        }
                    } else {
                        $("#dlgAddItem").dialog("option", "height", 260);;
                        $("#previousPriceLinePanel").hide();
                        $("#previousPriceLine").text(''); 
                    }
                                       
                    previousPriceLine
                    return false;
                },
                source: "PriceLineAssociation/SearchItemByPrefix?priceLineId=" + priceLineId,
                focus: function (event, ui) {
                    event.preventDefault();
                }
            });
        },
        buttons: {
            'Add':
            {
                id: 'addButton',
                text: 'Add',
                disabled: true,
                click: function () {
                    let refDialog = this; //Ref to dialog so we can close it on success
                    $(".ui-dialog-buttonset").hide();
                    $(".ui-dialog-titlebar-close").hide();
                    $(".ui-dialog-buttonset").children().hide();
                    $(".ui-dialog-title").text("Processing Request...");
                    $("#previousPriceLinePanel").hide();
                    //$("#dlgAddItem").dialog("option", "height", 380);
                    $("#addItemMessagePanel").show();
                    $("#addItemMessage").html('Adding Item... &nbsp<span class="spinner-border float-right ml-auto text-primary" role="status" aria-hidden="true" style="color: red" />');

                    if (addEnablementInterval != null) {
                        clearInterval(addEnablementInterval);
                    }                    

                    var request = $.ajax({
                        url: '/PriceLineAssociation/AddItemToPriceLine',
                        type: 'POST',
                        data: JSON.stringify({ itemId: selectedItem.itemId, priceLineId: priceLineId }),
                        cache: false,
                        contentType: 'application/json; charset=utf-8',

                        success: function (response) {
                            priceAssociationLineTable.ajax.reload();
                            $("#dlgAddItem").dialog("close");
                        },
                        error:
                            function (xhr, status, error) {
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
                                try {
                                    $("#errorMessage").text(JSON.parse(xhr.responseText));
                                }
                                catch (err) {
                                    $("#errorMessage").text('Request failed: ' + error);
                                }
                            }
                    });
                }
            },
            'Cancel':
            {
                id: 'dlgCancelAddItem',
                text: 'Cancel',
                click: function () {
                    if (addEnablementInterval != null) {
                        clearInterval(addEnablementInterval);
                    }
                    $(this).dialog("close");
                },
            }
        }
    });
}