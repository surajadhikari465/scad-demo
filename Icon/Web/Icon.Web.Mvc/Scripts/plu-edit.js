function saveButtonClick() {

    $('#saveButton').val('Saving...');
    $('#saveButton').disable(true);

    // Prevent the search button from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
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