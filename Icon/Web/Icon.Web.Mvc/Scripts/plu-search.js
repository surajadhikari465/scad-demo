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

    $('#pluGrid').on('iggridrendered', setColumnHeaderTooltips);

    enableSearchButton();
}

function displayModelValidation(result) {

    // Display the search options with included validation errors.
    $('#searchOptions').html(result.responseText);

    enableSearchButton();
}

function searchButtonClick() {

    $('#searchButton').val('Searching...');
    $('#searchButton').disable(true);

    // Prevent the search button from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
}

function enableSearchButton() {

    $('#searchButton').val('Search');
    $('#searchButton').disable(false);
}

// Helper function to allow toggling of the 'disabled' CSS class.
jQuery.fn.extend({
    disable: function (state) {
        return this.each(function () {
            var $this = $(this);
            $this.toggleClass('disabled', state);
        });
    }
});

function setColumnHeaderTooltips() {
    $('#pluGrid_NationalPlu').attr("title", "National PLU");
    $('#pluGrid_Brand').attr("title", "Brand");
    $('#pluGrid_PluDescription').attr("title", "PLU Description");
    $('#pluGrid_flPLU').attr("title", "FL");
    $('#pluGrid_maPLU').attr("title", "MA");
    $('#pluGrid_mwPLU').attr("title", "MW");
    $('#pluGrid_naPLU').attr("title", "NA");
    $('#pluGrid_ncPLU').attr("title", "NC");
    $('#pluGrid_nePLU').attr("title", "NE");
    $('#pluGrid_pnPLU').attr("title", "PN");
    $('#pluGrid_rmPLU').attr("title", "RM");
    $('#pluGrid_soPLU').attr("title", "SO");
    $('#pluGrid_spPLU').attr("title", "SP");
    $('#pluGrid_swPLU').attr("title", "SW");
    $('#pluGrid_ukPLU').attr("title", "UK");
}