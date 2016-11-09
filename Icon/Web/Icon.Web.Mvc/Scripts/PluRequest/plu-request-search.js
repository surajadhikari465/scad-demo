// 'global' variables needed for combo datasource formatter functions
var merchNameLookup = {}, nationalClassLookup = {}, finClassLookup = {};


// These are used to fix the bug in IE10 where the 'undefined' showed up in tax, merch, and uom lookups.
var alreadyRunTaxLookup = false;
var alreadyRunMerchLookup = false;
var alreadyRunNationalClassLookup = false;
var alreadyRunFinClassLookup = false;

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

  

    $(document).on('iggridrendered', '#igGrid', function (evt, ui) {
        setColumnHeaderTooltips();
    });

    enableSearchButton();
    rowEditSaveChanges(); 
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
    });
}


//Populate Merchandise Lookup Array for displaying Merchandise HierarchyClass Names instead of IDs
function fillMerchLookup() {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];

        if (setting.columnKey === 'MerchandiseClassID') {
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

        if (setting.columnKey === 'NationalClassID') {
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

//Populate National Class Lookup Array for displaying National HierarchyClass Names instead of IDs
function fillFinClassLookup() {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];

        if (setting.columnKey === 'FinancialClassID') {
            var comboDataSource = setting.editorOptions.dataSource;
            var textKey = setting.editorOptions.textKey;
            var valueKey = setting.editorOptions.valueKey;
            var item;
            for (var j = 0; j < comboDataSource.length; j++) {
                item = comboDataSource[j];
                finClassLookup[item[valueKey]] = item[textKey];
            }
        }
    }

    alreadyRunFinClassLookup = true;
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



// FormatterFunction for FinHierarchyClassId column in igGrid
function lookupFinName(finClassId) {
    if (!alreadyRunFinClassLookup) {
        fillFinClassLookup();
    }
    if (finClassId === null) {
        return "";
    }
    return finClassLookup[finClassId];
}



function setColumnHeaderTooltips() {
    $('#igGrid_FinancialClassID').attr("title", "SubTeam");
    $('#igGrid_NationalPLU').attr("title", "National PLU");
    $('#igGrid_randName').attr("title", "Brand Name");
    $('#igGrid_ItemDescription').attr("title", "Item Dbescription");
    $('#igGrid_PosDescription').attr("title", "POS Description");
    $('#igGrid_PackageUnit').attr("title", "Item Pack");
    $('#igGrid_RetailSize').attr("title", "Retail Size");
    $('#igGrid_RetailUom').attr("title", "Retail UOM");
    $('#igGrid_NationalClassID').attr("title", "National Class");
    $('#igGrid_RequestStatus').attr("title", "PLU Request Status");
    $('#igGrid_MerchandiseHierarchyClassId').attr("title", "Merchandise Hierarchy");
    $('#igGrid_RequesterName').attr("title", "Requested By");
}