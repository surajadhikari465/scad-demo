$(function () {
    $('#pluErrorsGrid').on('iggridrendered', setColumnHeaderTooltips);
    $('#pluRemapGrid').on('iggridrendered', setRemapColumnHeaderTooltips);
});

function remapRegionFormatter(region) {
    region = region.substring(0, 2).toUpperCase();
    return region;
}

function beginOverwrite() {
    $('#overwriteButton').disable(true);

    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
}

function applyBootstrapAlert() {
    $('#alert').addClass('alert alert-success');
    $('#overwriteButton').disable(false);
}

function setColumnHeaderTooltips() {
    $('#pluErrorsGrid_NationalPlu').attr("title", "National PLU");
    $('#pluErrorsGrid_BrandName').attr("title", "Brand");
    $('#pluErrorsGrid_ProductDescription').attr("title", "PLU Description");
    $('#pluErrorsGrid_flPLU').attr("title", "FL");
    $('#pluErrorsGrid_maPLU').attr("title", "MA");
    $('#pluErrorsGrid_mwPLU').attr("title", "MW");
    $('#pluErrorsGrid_naPLU').attr("title", "NA");
    $('#pluErrorsGrid_ncPLU').attr("title", "NC");
    $('#pluErrorsGrid_nePLU').attr("title", "NE");
    $('#pluErrorsGrid_pnPLU').attr("title", "PN");
    $('#pluErrorsGrid_rmPLU').attr("title", "RM");
    $('#pluErrorsGrid_soPLU').attr("title", "SO");
    $('#pluErrorsGrid_spPLU').attr("title", "SP");
    $('#pluErrorsGrid_swPLU').attr("title", "SW");
    $('#pluErrorsGrid_ukPLU').attr("title", "UK");
}

function setRemapColumnHeaderTooltips() {
    $('#pluRemapGrid_Region').attr("title", "Region");
    $('#pluRemapGrid_RegionalPlu').attr("title", "Regional PLU");
    $('#pluRemapGrid_CurrentNationalPlu').attr("title", "Current National PLU");
    $('#pluRemapGrid_CurrentNationalPluDescription').attr("title", "Current National PLU Description");
    $('#pluRemapGrid_NewNationalPlu').attr("title", "NewNationalPlu");
    $('#pluRemapGrid_NewNationalPluDescription').attr("title", "New National PLU Description");
}