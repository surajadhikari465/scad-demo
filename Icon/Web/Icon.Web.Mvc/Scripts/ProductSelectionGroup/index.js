// 'global' variables needed for combo datasource formatter functions
var traitNameLookup = {};
var merchNameLookup = {};
var psgTypeNameLookup = {};

// These are used to fix the bug in IE10 where the 'undefined' showed up in tax, merch, and uom lookups.
var alreadyRunTraitLookup = false;
var alreadyRunMerchLookup = false;
var alreadyRunPsgTypeLookup = false;

$(document).ready(function () {
    rowEditSaveChanges();
});

function rowEditSaveChanges() {
    var grid = $('#igGrid');
    var updates;

    grid.on("iggridupdatingeditrowended", function (evt, ui) {
        if (ui.update) {
            grid.igGrid("saveChanges",
                function saveSuccess(data) {
                    if (data.Success === true) {
                        alertMessage('psgAlert', 'Successfully updated PSGs.', false);
                    }
                    else {
                        alertMessage('psgAlert', data.Error, true);
                        rollbackPendingGridChanges();
                    }
                },
                function saveFailure() {
                    alertMessage('psgAlert', 'There was an unexpected error updating the PSG.', true);
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

//Populate Trait Lookup Array for displaying Trait Names instead of IDs
function fillTraitLookup() {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];
        if (setting.columnKey === 'TraitId') {
            var comboDataSource = setting.editorOptions.dataSource;
            var textKey = setting.editorOptions.textKey;
            var valueKey = setting.editorOptions.valueKey;
            var item;
            for (var j = 0; j < comboDataSource.length; j++) {
                item = comboDataSource[j];
                traitNameLookup[item[valueKey]] = item[textKey];
            }
        }
    }

    alreadyRunTraitLookup = true;
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

//Populate ProductSelectionGroupType Lookup Array for displaying Merchandise HierarchyClass Names instead of IDs
function fillPsgNameLookup() {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];

        if (setting.columnKey === 'ProductSelectionGroupTypeId') {
            var comboDataSource = setting.editorOptions.dataSource;
            var textKey = setting.editorOptions.textKey;
            var valueKey = setting.editorOptions.valueKey;
            var item;
            for (var j = 0; j < comboDataSource.length; j++) {
                item = comboDataSource[j];
                psgTypeNameLookup[item[valueKey]] = item[textKey];
            }
        }
    }

    alreadyRunPsgTypeLookup = true;
}

// These Formatter Functions display the Names instead of the IDs and are called on the asp.net view
// FormatterFunction for TraitId column in igGrid
function lookupTraitName(traitId) {
    if (!alreadyRunTraitLookup) {
        fillTraitLookup();
    }

    if (traitId === null) {
        return "";
    }
    return traitNameLookup[traitId];
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

// FormatterFunction for ProductSelectionGroupTypeId column in igGrid
function lookupPsgTypeName(psgTypeId) {
    if (!alreadyRunPsgTypeLookup) {
        fillPsgNameLookup();
    }
    if (psgTypeId === null) {
        return "";
    }
    return psgTypeNameLookup[psgTypeId];
}