function refreshValidateButtonForSelection() {

    var validateButtons = $('[name="validate-button"]');

    var selectedRows = $('#igGrid').igGrid('selectedRows');
    if (selectedRows.length) {
        for (var i = 0; i < selectedRows.length; i++) {
            if (!(isRowValidatable(selectedRows[i]))) {
                validateButtons.disable(true);
                return;
            }
        }
        validateButtons.disable(false);
    }
    else {
        validateButtons.disable(true);
    }
}

function setupValidateButtonOnClickEvent() {
    $('[name="validate-button"]').click(validateRows);
}

function getSelectedRows() {
    var selectedRows = $("#igGrid").igGridSelection("selectedRows");
    var rowDataObject = new Array();

    for (var i = 0; i < selectedRows.length; i++) {
        rowDataObject[i] = $("#igGrid").igGrid("findRecordByKey", selectedRows[i].element.data().id);
    }

    return rowDataObject;
}

function isRowValidatable(row) {
    var id = row.element.data().id;

    // If IsValidated value is true, disable the Validate button.
    var status = $('#igGrid').igGrid('getCellValue', id, 'IsValidated');
    if (status) {
        return false;
    }

    // If IsValidated value is false then check all other fields.
    var validatable = doesRowHaveValidatableColumnsHydrated(id);

    return validatable;
}

function doesRowHaveValidatableColumnsHydrated(rowId) {
    var scanCode = $('#igGrid').igGrid('getCellValue', rowId, 'ScanCode');
    var brand = $('#igGrid').igGrid('getCellValue', rowId, 'BrandHierarchyClassId');
    var productDescription = $('#igGrid').igGrid('getCellValue', rowId, 'ProductDescription');
    var posDescription = $('#igGrid').igGrid('getCellValue', rowId, 'PosDescription');
    var packageUnit = $('#igGrid').igGrid('getCellValue', rowId, 'PackageUnit');
    var posScaleTare = $('#igGrid').igGrid('getCellValue', rowId, 'PosScaleTare');
    var merchandiseClass = $('#igGrid').igGrid('getCellValue', rowId, 'MerchandiseHierarchyClassId');
    var taxClass = $('#igGrid').igGrid('getCellValue', rowId, 'TaxHierarchyClassId');

    // POS Scale Tare, Retail Size, and Retail UOM should not prevent something being validated.
    var validatable = !isNullOrEmpty(scanCode)
        && (brand > 0)
        && !isNullOrEmpty(productDescription)
        && !isNullOrEmpty(posDescription)
        && !isNullOrEmpty(packageUnit)
        && (merchandiseClass > 0)
        && (taxClass > 0);

    return validatable;
}

function isNullOrEmpty(str) {
    return (str === '' || str === null);
}