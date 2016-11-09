var alreadyRunTaxLookup = false;
var alreadyRunMerchLookup = false;
var alreadyRunNationalLookup = false;
var brandsLookup = {};
var taxNameLookup = {}, merchNameLookup = {}, nationalLookup = {};

function displayModelValidation(result) {

    // Display the search options with included validation errors.
    $('#searchOptions').html(result.responseText); 
}

function displayResults(result) {

    // Clear any validation messages that may have previously been shown in the search options section.
    $('.field-validation-error')
        .removeClass('field-validation-error')
        .addClass('field-validation-valid');

    $('.input-validation-error')
        .removeClass('input-validation-error')
        .addClass('valid');

    $('.validation-summary-errors')
        .removeClass('validation-summary-errors')
        .addClass('validation-summary-valid');
    
    $('#igGrid').on('iggridrendered', setColumnHeaderTooltips);
 
    rowEditSaveChanges();
    enableSearchButton();

    $('#export').click(exportItems);
}

function enableSearchButton() {

    $('#search-button').val('Search');
    $('#search-button').disable(false);
}

function rowEditSaveChanges() {
    $("#igGrid").on("iggridupdatingeditrowended", function (evt, ui) {
        if (ui.update) {
            $("#igGrid").igGrid("saveChanges",
                function saveSuccess(data) {
                    if (data.Success === true) {
                        displayAlertMessage('Successfully updated item.', false);
                    }
                    else {
                        displayAlertMessage(data.Error, true);
                        rollbackPendingGridChanges();
                    }
                },
                function saveFailure() {
                    displayAlertMessage('There was an unexpected error updating the item.', true);
                    rollbackPendingGridChanges();
                });
        }        
    });
}


function rollbackPendingGridChanges() {
    var grid = $('#igGrid');
    var updates = $.extend({}, grid.data('igGrid').pendingTransactions());
    $.each(updates, function (index, transaction) {
        grid.igGrid("rollback", transaction.rowId, true);
    });
}

function boolFormatter(value) {
    var formattedBool;

    if (value === true) {
        formattedBool = "Y";
    } else if (value === false) {
        formattedBool = "N";
    } else {
        formattedBool = '';
    }

    return formattedBool;
}

function setColumnHeaderTooltips() {
    $('#igGrid_Region').attr("title", "Region");
    $('#igGrid_Identifier').attr("title", "Identifier");
    $('#igGrid_BrandName').attr("title", "Brand Name");
    $('#igGrid_ItemDescription').attr("title", "Item Description");
    $('#igGrid_PosDescription').attr("title", "POS Description");
    $('#igGrid_PackageUnit').attr("title", "Item Pack");
    $('#igGrid_RetailSize').attr("title", "Retail Size");
    $('#igGrid_RetailUom').attr("title", "Retail UOM");
    $('#igGrid_FoodStamp').attr("title", "Food Stamp Eligible");
    $('#igGrid_PosScaleTare').attr("title", "POS Scale Tare");
    $('#igGrid_MerchandiseHierarchyClassId').attr("title", "Merchandise Hierarchy");
    $('#igGrid_TaxHierarchyClassId').attr("title", "Tax Class");
}

//Populate Tax Lookup Array for displaying Tax Names instead of IDs
function fillTaxLookup() {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];
        if (setting.columnKey === 'TaxHierarchyClassId') {
            var comboDataSource = setting.editorOptions.dataSource;
            var textKey = setting.editorOptions.textKey;
            var valueKey = setting.editorOptions.valueKey;
            var item;
            for (var j = 0; j < comboDataSource.length; j++) {
                item = comboDataSource[j];
                taxNameLookup[item[valueKey]] = item[textKey];
            }
        }
    }

    alreadyRunTaxLookup = true;
}

//Populate Merchandise Lookup Array for displaying Merchandise HierarchyClass Names instead of IDs
function fillMerchLookup() {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];

        if (setting.columnKey === 'MerchandiseHierarchyClassId') {
            var comboDataSource = setting.editorOptions.dataSource;
            var textKey = setting.editorOptions.textKey;
            var valueKey = setting.editorOptions.valueKey;
            var item;
            for (var j = 0; j < comboDataSource.length; j++) {
                item = comboDataSource[j];
                merchNameLookup[item[valueKey]] = item[textKey];
            }
        }
    }

    alreadyRunMerchLookup = true;
}

// These Formatter Functions display the HierarchyClassName instead of the HierarchyClassId and are called on the asp.net view
// FormatterFunction for TaxHierarchyClassId column in igGrid
function lookupTaxName(taxId) {
    if (!alreadyRunTaxLookup) {
        fillTaxLookup();
    }

    if (taxId === null) {
        return "";
    }
    return taxNameLookup[taxId];
}

function lookupNational(nationalId) {
    if (!alreadyRunNationalLookup) {
        fillNationalLookup();
    }

    if (nationalId === null) {
        return "";
    }
    return nationalLookup[nationalId];
}

function fillNationalLookup() {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];
        if (setting.columnKey === 'NationalHierarchyClassId') {
            var comboDataSource = setting.editorOptions.dataSource;
            var textKey = setting.editorOptions.textKey;
            var valueKey = setting.editorOptions.valueKey;
            var item;
            for (var j = 0; j < comboDataSource.length; j++) {
                item = comboDataSource[j];
                nationalLookup[item[valueKey]] = item[textKey];
            }
        }
    }

    alreadyRunNationalLookup = true;
}

// FormatterFunction for MerchandiseHierarchyClassId column in igGrid
function lookupMerchName(merchId) {
    if (!alreadyRunMerchLookup) {
        fillMerchLookup();
    }
    if (merchId === null) {
        return "";
    }
    return merchNameLookup[merchId];
}

function exportItems() {
    var selectedID = $('#SelectedPluCategoryId').val()
    var maxPlus = $('#MaxPlus').val()
    $.fileDownload('/PluAssignment/Export', {
        httpMethod: "POST",
        data: {
            "SelectedPluCategoryId": selectedID, "MaxPlus": maxPlus
        }
    });    
}

function searchButtonClick() {

    $('#search-button').val('Searching...');
    $('#search-button').disable(true);

    // Prevent the search button from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
    // Clear any create item alert messages if shown
    $('#createItemAlert').remove();
}



// Helper function to allow toggling of the 'disabled' CSS class.
jQuery.fn.extend({
    disable: function (state) {
        return this.each(function () {
            var $this = $(this);
            $this.toggleClass('disabled', state);
        });
    }
});

function getSelectedRows() {
    var selectedRows = $("#igGrid").igGridSelection("selectedRows");
    var rowDataObject = new Array();

    for (var i = 0; i < selectedRows.length; i++) {
        rowDataObject[i] = $("#igGrid").igGrid("findRecordByKey", selectedRows[i].element.data().id);
    }

    return rowDataObject;
}

function displayAlertMessage(message, isError) {
    var alertClass = null;
    if (isError) {
        alertClass = "alert-danger";
    } else {
        alertClass = "alert-success";
    }
    var alertMessage = '<div class="alert ' + alertClass + ' alert-dismissable page-subsection">' +
                            '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                            '<strong class="alertMessage">' + message + '</strong>' +
                        '</div>';
    $('.search-alert').html(alertMessage);
}

function alertMessage(alertId, message, isError) {
    var alertClass = null;
    if (isError) {
        alertClass = "alert-danger";
    } else {
        alertClass = "alert-success";
    }
    var alertMessage = '<div class="alert ' + alertClass + ' alert-dismissable page-subsection">' +
                            '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                            '<strong class="alertMessage">' + message + '</strong>' +
                        '</div>';
    $("#" + alertId).html(alertMessage);
}

function isNullOrEmpty(str) {
    return (str === '' || str === null);
}
