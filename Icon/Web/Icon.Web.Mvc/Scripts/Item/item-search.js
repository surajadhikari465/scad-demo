var validateRowsButton;

var filledBrandsFormatLookup = false;
var filledTaxFormatLookup = false;
var filledMerchandiseFormatLookup = false;
var filledNationalFromatLookup = false;

var brandsLookup = {};
var taxLookup = {};
var merchandiseLookup = {};
var nationalLookup = {};

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
    $('#igGrid').on('ig')

    enableSearchButton();
    rowEditSaveChanges();    
    disableValidationButtons(true);

    $('[name="validate-button"]').click(validateIconRows);
    $('[name="export-button"]').click(exportItems);
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
                        $('#igGrid').igGrid('rollback');
                    }
                },
                function saveFailure() {
                    displayAlertMessage('There was an unexpected error updating the item.', true);
                    $('#igGrid').igGrid('rollback');
                });
        }

        refreshValidateButtonForSelection();
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
    $('#igGrid_IsValidated').attr("title", "Status");
    $('#igGrid_ScanCode').attr("title", "Scan Code");
    $('#igGrid_BrandHierarchyClassId').attr("title", "Brand");
    $('#igGrid_ProductDescription').attr("title", "Product Description");
    $('#igGrid_PosDescription').attr("title", "POS Description");
    $('#igGrid_PackageUnit').attr("title", "Item Pack");
    $('#igGrid_FoodStampEligible').attr("title", "Food Stamp Eligible");
    $('#igGrid_PosScaleTare').attr("title", "POS Scale Tare");
    $('#igGrid_RetailSize').attr("title", "Retail Size");
    $('#igGrid_RetailUom').attr("title", "Retail UOM");
    $('#igGrid_MerchandiseHierarchyClassId').attr("title", "Merchandise Hierarchy");
    $('#igGrid_TaxHierarchyClassId').attr("title", "Tax Class"); 
    $('#igGrid_DepartmentSale').attr("title", "Department Sale");
}

function validateIconRows() {
    if ($(this).hasClass('disabled'))
        return;

    var rowDataObject = getSelectedRows();
    
    var request = $.ajax({
        url: 'Item/ValidateSelected',
        type: 'POST',
        data: JSON.stringify(rowDataObject),
        contentType: 'application/json; charset=utf-8'
    });

    request.done(function (data) {
        if (data.Success == true) {
            displayAlertMessage(data.Message, false);
            runConsolidatedSearch();
        } else {
            displayAlertMessage(data.Message, true);
        }
    });
}

function runConsolidatedSearch() {
    var viewModel = {};

    viewModel.BrandName = $("#BrandName").val();
    viewModel.MerchandiseHierarchy = $("#MerchandiseHierarchy").val();
    viewModel.TaxHierarchy = $("#TaxHierarchy").val();
    viewModel.Description = $("#ProductDescription").val();
    viewModel.PosDescription = $("#PosDescription").val();
    viewModel.ScanCode = $("#ScanCode").val();
    viewModel.SelectedStatusId = $("#SelectedStatusId").val();
    viewModel.SelectedFoodStampId = $("#SelectedFoodStampId").val();
    viewModel.PosScaleTare = $("#PosScaleTare").val();
    viewModel.PackageUnit = $("#PackageUnit").val();
    viewModel.SelectedDepartmentSaleId = $("#SelectedDepartmentSaleId").val();
    viewModel.RetailSize = $("#RetailSize").val();
    viewModel.SelectedRetailUom = $("#SelectedRetailUom").val();
    viewModel.DeliverySystem = $("SelectedDeliverySystem").val();
    viewModel.AlcoholByVolume = $('#AlcoholByVolume').val();
    viewModel.CaseineFree = $('#CaseineFree').val();
    viewModel.DrainedWeight = $('#DrainedWeight').val();
    viewModel.DrainedWeightUom = $('#DrainedWeightUom').val();
    viewModel.FairTradeCertified = $('#FairTradeCertified').val();
    viewModel.Hemp = $('#Hemp').val();
    viewModel.LocalLoanProducer = $('#LocalLoanProducer').val();
    viewModel.MainProductName = $('#MainProductName').val();
    viewModel.NutritionRequired = $('#NutritionRequired').val();
    viewModel.OrganicPersonalCare = $('#OrganicPersonalCare').val();
    viewModel.Paleo = $('#Paleo').val();
    viewModel.ProductFlavorType = $('#ProductFlavorType').val();
    viewModel.NationalHierarchy = $("#NationalHierarchy").val();
    viewModel.SelectedHiddenItemStatusId = $("#SelectedHiddenItemStatusId").val();
    viewModel.CreatedDate = $("#CreatedDate").val();
    viewModel.LastModifiedDate = $("#LastModifiedDate").val();
    viewModel.LastModifiedUser = $("#LastModifiedUser").val();
        
    if ($("#PartialScanCode").is(":checked")) {
        viewModel.PartialScanCode = true;
    } else {
        viewModel.PartialScanCode = false;
    }
    if ($("#PartialBrandName").is(":checked")) {
        viewModel.PartialBrandName = true;
    } else {
        viewModel.PartialBrandName = false;
    }

    viewModel.ItemSignAttributes = {};
    viewModel.ItemSignAttributes.SelectedAnimalWelfareRatingId = $("#ItemSignAttributes_SelectedAnimalWelfareRatingId").val();
    viewModel.ItemSignAttributes.SelectedBiodynamicOption = $("#ItemSignAttributes_SelectedBiodynamicOption").val();
    viewModel.ItemSignAttributes.SelectedCheeseMilkTypeId = $("#ItemSignAttributes_SelectedCheeseMilkTypeId").val();
    viewModel.ItemSignAttributes.SelectedCheeseRawOption = $("#ItemSignAttributes_SelectedCheeseRawOption").val();
    viewModel.ItemSignAttributes.SelectedEcoScaleRatingId = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.GlutenFreeAgency = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.KosherAgency = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.NonGmoAgency = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.OrganicAgency = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedPremiumBodyCareOption = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedSeafoodFreshOrFrozenId = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedSeafoodCatchTypeId = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.VeganAgency = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedVegetarianOption = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedWholeTradeOption = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedGrassFedOption = $('#ItemSignAttributes_SelectedGrassFedOption').val();
    viewModel.ItemSignAttributes.SelectedPastureRaisedOption = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedFreeRangeOption = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedDryAgedOption = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedAirChilledOption = $("#ItemSignAttributes_SelectedStatusId").val();
    viewModel.ItemSignAttributes.SelectedMadeInHouseOption = $("#ItemSignAttributes_SelectedStatusId").val();    
    
    searchButtonClick();

    var request = $.ajax({
        url: 'Item/Search',
        type: 'GET',
        data: $.postify(viewModel)
    });

    request.done(function (data) {
        $('#searchResults').html(data);
        displayResults(data);
    });

    request.fail(function (data) {
        displayModelValidation(data);
    });
}

function refreshValidateButtonForSelection() {
    var selectedRows = $('#igGrid').igGrid('selectedRows');

    if (selectedRows.length) {
        for (var i = 0; i < selectedRows.length; i++) {
            if (!(isRowValidatable(selectedRows[i]))) {
                disableValidationButtons(true);
                return;
            }
        }

        disableValidationButtons(false);
    }
    else {
        disableValidationButtons(true);
    }
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
    var nationalClass = $('#igGrid').igGrid('getCellValue', rowId, 'NationalHierarchyClassId');

    // POS Scale Tare, Retail Size, and Retail UOM should not prevent something being validated.
    var validatable = !isNullOrEmpty(scanCode)
        && (brand > 0)
        && !isNullOrEmpty(productDescription)
        && !isNullOrEmpty(posDescription)
        && !isNullOrEmpty(packageUnit)
        && (merchandiseClass > 0)
        && (taxClass > 0)
        && (nationalClass > 0);

    return validatable;
}

function formatBrandHierarchy(val) {
    if (!filledBrandsFormatLookup) {
        fillHierarchyClassLookup("BrandHierarchyClassId", brandsLookup);
        filledBrandsFormatLookup = true;
    }

    if (val === null || val === -1) {
        return "";
    }

    return brandsLookup[val];
}

function formatMerchandiseHierarchy(val) {
    if (!filledMerchandiseFormatLookup) {
        fillHierarchyClassLookup("MerchandiseHierarchyClassId", merchandiseLookup);
        filledMerchandiseFormatLookup = true;
    }

    if (val === null || val === -1) {
        return "";
    }

    return merchandiseLookup[val];
}

function formatNationalHierarchy(val) {
    if (!filledNationalFromatLookup) {
        fillHierarchyClassLookup("NationalHierarchyClassId", nationalLookup);
        filledNationalFromatLookup = true;
    }

    if (val === null || val === -1) {
        return "";
    }

    return nationalLookup[val];
}

function formatTaxHierarchy(val) {
    if (!filledTaxFormatLookup) {
        fillHierarchyClassLookup("TaxHierarchyClassId", taxLookup);
        filledTaxFormatLookup = true;
    }

    if (val === null || val === -1) {
        return "";
    }

    return taxLookup[val];
}

function fillHierarchyClassLookup(columnKey, lookup) {
    var columnSettings = $('#igGrid').igGridUpdating('option', 'columnSettings');
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

function exportItems() {
    var dataSourceUrl = $('#igGrid').igGrid('option', 'dataSource');
    var indexOfQueryString = dataSourceUrl.indexOf("?");
    dataSourceUrl = "/Excel/ItemSearchExport" + dataSourceUrl.substring(indexOfQueryString);
    
    $.fileDownload(dataSourceUrl, {
        httpMethod: "GET"
    });
}

function disableValidationButtons(buttonState) {
    $('[name="validate-button"]').disable(buttonState);
}


$.postify = function (value) {
    var result = {};

    var buildResult = function (object, prefix) {
        for (var key in object) {

            var postKey = isFinite(key)
                ? (prefix != "" ? prefix : "") + "[" + key + "]"
                : (prefix != "" ? prefix + "." : "") + key;

            switch (typeof (object[key])) {
                case "object":                    
                        buildResult(object[key], postKey != "" ? postKey : key);                    
                    break;
                default:
                        result[postKey] = object[key];
                     
            }
        }
    };

    buildResult(value, "");

    return result;
};