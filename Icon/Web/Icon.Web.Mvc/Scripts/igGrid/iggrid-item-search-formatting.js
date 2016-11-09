var filledBrandsFormatLookup = false;
var filledTaxFormatLookup = false;
var filledMerchandiseFormatLookup = false;
var filledNationalLookup = false;
var filledAnimalWelfareRatings = false;
var filledMilkTypes = false;
var filledEcoScaleRatings = false;
var filledProductionClaims = false;
var filledSeafoodFreshOrFrozenTypes = false;
var filledSeafoodCatchTypes = false;
var filledGlutenFreeAgencies = false;
var filledKosherAgencies = false;
var filledNonGmoAgencies = false;
var filledOrganicAgencies = false;
var filledVeganAgencies = false;

var brandsLookup = {};
var taxLookup = {};
var merchandiseLookup = {};
var nationalLookup = {}
var animalWelfareRatings = {};
var milkTypes = {};
var ecoScaleRatings = {};
var productionClaims = {};
var seafoodFreshOrFrozenTypes = {};
var seafoodCatchTypes = {};
var glutenFreeAgencies = {};
var kosherAgencies = {};
var nonGmoAgencies = {};
var organicAgencies = {};
var veganAgencies = {};

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

function formatNationalHierarchy(val) {
    if (!filledNationalLookup) {
        fillHierarchyClassLookup("NationalHierarchyClassId", nationalLookup);
        filledNationalLookup = true;
    }

    if (val === null || val === -1) {
        return "";
    }
    return nationalLookup[val];
}

function formatAnimalWelfareRating(val) {
    if (!filledAnimalWelfareRatings) {
        fillHierarchyClassLookup("AnimalWelfareRatingId", animalWelfareRatings);
        filledNationalLookup = true;
    }

    if (animalWelfareRatings[val]) {
        return animalWelfareRatings[val];
    } else {
        return '';
    }
}

function formatMilkType(val) {
    if (!filledMilkTypes) {
        fillHierarchyClassLookup("CheeseMilkTypeId", milkTypes);
        filledMilkTypes = true;
    }

    if (milkTypes[val]) {
        return milkTypes[val];
    } else {
        return '';
    }
}

function formatEcoScaleRating(val) {
    if (!filledEcoScaleRatings) {
        fillHierarchyClassLookup("EcoScaleRatingId", ecoScaleRatings);
        filledEcoScaleRatings = true;
    }

    if (ecoScaleRatings[val]) {
        return ecoScaleRatings[val];
    } else {
        return '';
    }
}

function formatProductionClaim(val) {
    if (!filledProductionClaims) {
        fillHierarchyClassLookup("ProductionClaimsId", productionClaims);
        filledProductionClaims = true;
    }

    if (productionClaims[val]) {
        return productionClaims[val];
    } else {
        return '';
    }
}

function formatSeafoodFreshOrFrozenType(val) {
    if (!filledSeafoodFreshOrFrozenTypes) {
        fillHierarchyClassLookup("SeafoodFreshOrFrozenId", seafoodFreshOrFrozenTypes);
        filledSeafoodFreshOrFrozenTypes = true;
    }

    if (seafoodFreshOrFrozenTypes[val]) {
        return seafoodFreshOrFrozenTypes[val];
    } else {
        return '';
    }
}

function formatSeafoodCatchType(val) {
    if (!filledSeafoodCatchTypes) {
        fillHierarchyClassLookup("SeafoodCatchTypeId", seafoodCatchTypes);
        filledSeafoodCatchTypes = true;
    }

    if (seafoodCatchTypes[val]) {
        return seafoodCatchTypes[val];
    } else {
        return '';
    }
}

function formatGlutenFreeAgency(val) {
    if (!filledGlutenFreeAgencies) {
        fillHierarchyClassLookup("GlutenFreeAgencyId", glutenFreeAgencies);
        filledGlutenFreeAgencies = true;
    }

    if (glutenFreeAgencies[val]) {
        return glutenFreeAgencies[val];
    } else {
        return '';
    }
}

function formatKosherAgency(val) {
    if (!filledKosherAgencies) {
        fillHierarchyClassLookup("KosherAgencyId", kosherAgencies);
        filledKosherAgencies = true;
    }

    if (kosherAgencies[val]) {
        return kosherAgencies[val];
    } else {
        return '';
    }
}

function formatNonGmoAgency(val) {
    if (!filledNonGmoAgencies) {
        fillHierarchyClassLookup("NonGmoAgencyId", nonGmoAgencies);
        filledNonGmoAgencies = true;
    }

    if (nonGmoAgencies[val]) {
        return nonGmoAgencies[val];
    } else {
        return '';
    }
}

function formatOrganicAgency(val) {
    if (!filledOrganicAgencies) {
        fillHierarchyClassLookup("OrganicAgencyId", organicAgencies);
        filledOrganicAgencies = true;
    }

    if (organicAgencies[val]) {
        return organicAgencies[val];
    } else {
        return '';
    }
}

function formatVeganAgency(val) {
    if (!filledVeganAgencies) {
        fillHierarchyClassLookup("VeganAgencyId", veganAgencies);
        filledVeganAgencies = true;
    }

    if (veganAgencies[val]) {
        return veganAgencies[val];
    } else {
        return '';
    }
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