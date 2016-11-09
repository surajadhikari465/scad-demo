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

    enableImportButton();
}

function displayModelValidation(result) {

    // Display the import options with included validation errors.
    $('#importOptions').html(result.responseText);

    enableImportButton();
}

function importButtonClick() {

    $('#importButton').val('Importing Spreadsheet...');
    $('#importButton').disable(true);
    $('#divLoading').show();

    // Prevent the import button from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
}

function enableImportButton() {

    $('#importButton').val('Search');
    $('#importButton').disable(false);
    $('#divLoading').hide;
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