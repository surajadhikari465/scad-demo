//Populate Tax Lookup Array for displaying Tax Names instead of IDs
function fillTaxLookup() {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];
        if (setting.columnKey === 'TaxClassId') {
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

//Populate Merchandise Lookup Array for displaying Merchandise Class Hierarchy Names instead of IDs
function fillMerchLookup() {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
    var setting;

    for (var i = 0; i < columnSettings.length; i++) {
        setting = columnSettings[i];

        if (setting.columnKey === 'MerchandiseClassId') {
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
// FormatterFunction for TaxClassId column in igGrid
function lookupTaxName(taxId) {
    if (!alreadyRunTaxLookup) {
        fillTaxLookup();
    }

    if (taxId === null) {
        return "";
    }
    return taxNameLookup[taxId];
}

// FormatterFunction for MerchandiseClassId column in igGrid
function lookupMerchName(merchId) {
    if (!alreadyRunMerchLookup) {
        fillMerchLookup();
    }
    if (merchId === null) {
        return "";
    }
    return merchNameLookup[merchId];
}