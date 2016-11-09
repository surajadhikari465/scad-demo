$(function () {
    $('#igGrid').on('iggridrendered', setColumnHeaderTooltips);
});

function bitColumnFormatter(value) {

    if (value === '1') {
        value = "Y";
    }
    else if (value === '0') {
        value = "N";
    }

    return value;
}

function setColumnHeaderTooltips() {
    $('#igGrid_ScanCode').attr("title", "Scan Code");
    $('#igGrid_BrandName').attr("title", "Brand");
    $('#igGrid_ProductDescription').attr("title", "Product Description");
    $('#igGrid_PosDescription').attr("title", "POS Description");
    $('#igGrid_PackageUnit').attr("title", "Item Pack");
    $('#igGrid_FoodStampEligible').attr("title", "Food Stamp Eligible");
    $('#igGrid_PosScaleTare').attr("title", "POS Scale Tare");
    $('#igGrid_PackageUnit').attr("title", "Item Pack");
}