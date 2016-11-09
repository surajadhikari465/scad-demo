$(document).ready(function() {
    $('#importButton').click(function () {
        $('#importButton').val('Importing Spreadsheet...');
        $('#importButton').disable(true);
        $('#exportButton').disable(true);
        
        // Prevent the import button from being clicked like a link.
        $('body').on('click', 'a.disabled', function (event) {
            event.preventDefault();
        });

        $('#divLoading').show();
    });
});
