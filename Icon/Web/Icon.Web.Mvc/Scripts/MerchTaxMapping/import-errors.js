$(function () {
    $('#igGrid').on('iggridrendered', setColumnHeaderTooltips);
});

function setColumnHeaderTooltips() {
    $('#igGrid_ScanCode').attr("title", "Scan Code");
    $('#igGrid_Brand').attr("title", "Brand");
    $('#igGrid_ProductDescription').attr("title", "Product Description");
    $('#igGrid_Merchandise').attr("title", "Merchandise");
    $('#igGrid_DefaultTaxClass').attr("title", "Default Tax Class");
    $('#igGrid_TaxClassOverride').attr("title", "Tax Class Override");
    $('#igGrid_Error').attr("title", "Error");
}