// 'global' variables needed for combo datasource formatter functions
var taxNameLookup = {}, merchNameLookup = {}, nationalClassLookup = {};
var $loadRowsBtn;
var $validateRowsBtn;
var $deleteRowsBtn;

// These are used to fix the bug in IE10 where the 'undefined' showed up in tax, merch, and uom lookups.
var alreadyRunTaxLookup = false;
var alreadyRunMerchLookup = false;
var alreadyRunNationalClassLookup = false;

// Tracks when Load or Validation is happening to help with button can be disabled/enabled
var isLoadingOrValidating = false;

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

    $loadRowsBtn = $('#loadRows');
    $loadRowsBtn.disable(true);
    $validateRowsBtn = $('#validateRows');
    $validateRowsBtn.disable(true);
    $deleteRowsBtn = $('#deleteRows');
    $deleteRowsBtn.disable(true);

    $(document).on('iggridrendered', '#igGrid', function (evt, ui) {
        setColumnHeaderTooltips();
    });

    enableSearchButton();
    disableLoadAndValidateButtons();
    rowEditSaveChanges();
    setupButtonClickEvents();

    // Make sure Load and Validate buttons are disabled/enabled also when
    // a user select a row even when they haven't edited anything.
    $('#igGrid').on('iggridselectionactiverowchanged', function () {
        gridSelectionChanged();
    });

    // Disable loading and validating items when a user is editing a row
    $('#igGrid').on('iggridupdatingeditrowstarted', function () {
        $loadRowsBtn.disable(true);
        $validateRowsBtn.disable(true);
        $deleteRowsBtn.disable(true);
    });

    $('#igGrid').on('iggridupdatingeditrowended', function () {
        $loadRowsBtn.disable(false);
    });

    $('#search-button').on('click', function () {
        $loadRowsBtn.disable(true);
        $validateRowsBtn.disable(true);
        $deleteRowsBtn.disable(true);
    })

    $('#export').click(exportItems);  

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
                    }
                },
                function saveFailure() {
                    displayAlertMessage('There was an unexpected error updating the item.', true);
                });
        }
        gridSelectionChanged();
    });
}

function disableLoadAndValidateButtons() {
    $loadRowsBtn.disable(true);
    $validateRowsBtn.disable(true);
    $deleteRowsBtn.disable(true);
}

function setupButtonClickEvents() {
    $loadRowsBtn.on("click", loadRowsToIcon);
    $validateRowsBtn.on("click", validateNewItemRows);
    $deleteRowsBtn.on("click", deleteNewItemRows);
}

function loadRowsToIcon() {
    isLoadingOrValidating = true;
    disableLoadAndValidateButtons();
    $loadRowsBtn.val("Loading...");

    var rowDataObject = getSelectedRows();
    var request = $.ajax({
        url: 'IrmaItem/LoadSelected',
        type: 'POST',
        data: JSON.stringify(rowDataObject),
        contentType: 'application/json; charset=utf-8',
    })
    .always(function (data) {
        if (data.Success === true) {
            // Rerun Search with search parameters in search form
            displayAlertMessage(data.Message, false);
            runNewItemSearch();
        } else {
            displayAlertMessage(data.Message, true);            
        }

        $loadRowsBtn.val("Load to Icon");
        isLoadingOrValidating = false;
    });
}

function validateNewItemRows() {
    isLoadingOrValidating = true;
    disableLoadAndValidateButtons();
    $validateRowsBtn.val("Validating...");

    var rowDataObject = getSelectedRows();
    var request = $.ajax({
        url: 'IrmaItem/ValidateSelected',
        type: 'POST',
        data: JSON.stringify(rowDataObject),
        contentType: 'application/json; charset=utf-8'
    });

    request.done(function (data) {
        if (data.Success == true) {
            // Rerun Search with search parameters in search form
            displayAlertMessage(data.Message, false);
            runNewItemSearch();
        } else {
            displayAlertMessage(data.Message, true);
        }
    });

    request.always(function () {
        $validateRowsBtn.val("Validate");
        isLoadingOrValidating = false;
    });
}

function deleteNewItemRows() {
    var confirmed = false;
    if (confirm("Are you sure you want to delete ?")) {
        confirmed = true;
    } else {
        confirmed =  false;
    }
    if (!confirmed)
        return;

    isLoadingOrValidating = true;
    disableLoadAndValidateButtons();
    $deleteRowsBtn.val("Deleting...");

    var rowDataObject = getSelectedRows();
    var request = $.ajax({
        url: 'IrmaItem/DeleteSelected',
        type: 'POST',
        data: JSON.stringify(rowDataObject),
        contentType: 'application/json; charset=utf-8'
    });

    request.done(function (data) {
        if (data.Success == true) {
            // Re-run search with search parameters in search form
            displayAlertMessage(data.Message, false);
            runNewItemSearch();
        } else {
            displayAlertMessage(data.Message, true);
        }
    });

    request.always(function () {
        $deleteRowsBtn.val("Delete");
        isLoadingOrValidating = false;
    });
}

function runNewItemSearch() {
    disableLoadAndValidateButtons();

    var viewModel = new Object();
    viewModel.BrandName = $("#BrandName").val();
    viewModel.Description = $("#Description").val();
    viewModel.Identifier = $("#Identifier").val();

    searchButtonClick();
    var request = $.ajax({
        url: 'IrmaItem/Search',
        type: 'GET',
        data: viewModel,
    });

    request.done(function (data) {
        $('#searchResults').html(data);
        displayResults(data);
    });

    request.fail(function (data) {
        displayModelValidation(data);
    });

    request.always(function () {
        isLoadingOrValidating = false;
    });
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

//Populate National Class Lookup Array for displaying National HierarchyClass Names instead of IDs
function fillNationalClassLookup() {
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
                nationalClassLookup[item[valueKey]] = item[textKey];
            }
        }
    }

    alreadyRunNationalClassLookup = true;
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

// FormatterFunction for NationalHierarchyClassId column in igGrid
function lookupNationalClassName(nationalClassId) {
    if (!alreadyRunNationalClassLookup) {
        fillNationalClassLookup();
    }
    if (nationalClassId === null) {
        return "";
    }
    return nationalClassLookup[nationalClassId];
}

function gridSelectionChanged() {
    if (isLoadingOrValidating) {
        return;
    }

    var selectedRows = $('#igGrid').igGrid('selectedRows');
    if (selectedRows.length > 0) {
        for (var i = 0; i < selectedRows.length; i++) {
            if (!(isRowValidatable(selectedRows[i]))) {
                $validateRowsBtn.disable(true);
                $loadRowsBtn.disable(false);
                $deleteRowsBtn.disable(false);
                return;
            }
        }
        $validateRowsBtn.disable(false);
        $loadRowsBtn.disable(false);
        $deleteRowsBtn.disable(false);
    }
    else {
        $validateRowsBtn.disable(true);
        $loadRowsBtn.disable(true);
        $deleteRowsBtn.disable(true);
    }
}

function isRowValidatable(row) {
    var id = row.element.data().id;

    var region = $('#igGrid').igGrid('getCellValue', id, 'Region');
    var identifier = $('#igGrid').igGrid('getCellValue', id, 'Identifier');
    var brand = $('#igGrid').igGrid('getCellValue', id, 'BrandName');
    var itemDescription = $('#igGrid').igGrid('getCellValue', id, 'ItemDescription');
    var posDescription = $('#igGrid').igGrid('getCellValue', id, 'PosDescription');
    var packageUnit = $('#igGrid').igGrid('getCellValue', id, 'PackageUnit');
    var posScaleTare = $('#igGrid').igGrid('getCellValue', id, 'PosScaleTare');
    var merchandiseClass = $('#igGrid').igGrid('getCellValue', id, 'MerchandiseHierarchyClassId');
    var taxClass = $('#igGrid').igGrid('getCellValue', id, 'TaxHierarchyClassId');
    var nationalClass = $('#igGrid').igGrid('getCellValue', id, 'NationalHierarchyClassId');

    var validatable = !isNullOrEmpty(region)
        && !isNullOrEmpty(identifier)
        && !isNullOrEmpty(brand)
        && !isNullOrEmpty(itemDescription)
        && !isNullOrEmpty(posDescription)
        && !isNullOrEmpty(packageUnit)
        && !isNullOrEmpty(merchandiseClass)
        && !isNullOrEmpty(taxClass)
        && (!isNullOrEmpty(nationalClass));

    return validatable;
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

function exportItems() {
    var items = { items: $('#igGrid').igGrid('option', 'dataSource').Records };
    $.fileDownload('Excel/IrmaItemExport', {
        httpMethod: "POST",
        data: items
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