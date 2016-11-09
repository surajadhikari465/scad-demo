var alreadyRunCountryCodeLookup = false;
var alreadyRunTimeZoneCodeLookup = false;
var alreadyRunTerritoryCodeLookup = false;

var countryCodeLookup = {}, timeZoneCodeLookup = {}, territoryCodeLookup = {};

$(function () {
    $("#saveButton").bind({
        click: function (e) {
            $("#storeGrid").igHierarchicalGrid("saveChanges");
        }
    });

    $('#saveButton').click(function () {
        $('#saveButton').val('Saving...');
        $('#saveButton').disable(true);
    });

    // Prevent disabled buttons from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
});

function onSuccess(data) {
    var message;
    var isError; 

    if (data.Success) {
        message = "Updates were applied successfully.";
        isError = false;
        displayAlertMessage(message, isError);
    } else {
        message = "Unable to apply updates.  Please refresh the grid and check for duplicate store names or business unit IDs.";
        isError = true;
        displayAlertMessage(message, isError);
    }

    enableSaveButton();
}

function onError(data) {
    var message = "Unable to apply updates.  Please refresh the grid and check for duplicate store names or business unit IDs.";
    var isError = true;

    if (data.status === 403)
    {
        message = "Access denied.";
    }

    displayAlertMessage(message, isError);

    enableSaveButton();
}

function fillLookup(lookup, columnKey) {
    var columnLayouts = $("#storeGrid").igGrid('option', 'columnLayouts');
    
    // Get the store's column layouts.
    var hasChildColumnLayouts = true;
    while (hasChildColumnLayouts) {
        if (columnLayouts[0].columnLayouts) {
            columnLayouts = columnLayouts[0].columnLayouts;
        }
        else {
            hasChildColumnLayouts = false;
        }
    }

    var storeColumnLayout = columnLayouts[0];

    // Get the column layouts features for updating because this is where the data source for the lookups exist.
    var updatingFeatures;
    for (var i = 0; i < storeColumnLayout.features.length; i++) {
        if (storeColumnLayout.features[i].name === "Updating")
            updatingFeatures = storeColumnLayout.features[i];
    }

    var columnSettings = updatingFeatures.columnSettings;
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];

        if (setting.columnKey === columnKey) {
            var comboDataSource = setting.editorOptions.dataSource;
            var textKey = setting.editorOptions.textKey;
            var valueKey = setting.editorOptions.valueKey;
            var item;
            for (var j = 0; j < comboDataSource.length; j++) {
                item = comboDataSource[j];
                lookup[item[valueKey]] = item[textKey];
            }
        }
    }
}

function lookupCountryCode(countryId) {
    if (!alreadyRunCountryCodeLookup) {
        fillLookup(countryCodeLookup, "CountryId");
        alreadyRunCountryCodeLookup = true;
    }

    if (countryId === null) {
        return "";
    }
    return countryCodeLookup[countryId];
}

function lookupTimeZoneCode(timeZoneId) {
    if (!alreadyRunTimeZoneCodeLookup) {
        fillLookup(timeZoneCodeLookup, "TimeZoneId");
        alreadyRunTimeZoneCodeLookup = true;
    }

    if (timeZoneId === null) {
        return "";
    }
    return timeZoneCodeLookup[timeZoneId];
}

function lookupTerritoryCode(territoryId) {
    if (!alreadyRunTerritoryCodeLookup) {
        fillLookup(territoryCodeLookup, "TerritoryId");
        alreadyRunTerritoryCodeLookup = true;
    }

    if (territoryId === null) {
        return "";
    }
    return territoryCodeLookup[territoryId];
}

function enableSaveButton() {
    $('#saveButton').val('Save Changes');
    $('#saveButton').disable(false);
}