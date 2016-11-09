

function updateButtonClick() {
    $('form').valid()

    // If there are any client-side validation messages on the form, don't
    // change the Save button.
    if (!$('.input-validation-error').length) {
        $('#updateButton').val('Saving Changes...');
        $('#updateButton').disable(true);

        // Prevent button from being clicked like a link.
        $('body').on('click', 'a.disabled', function (event) {
            event.preventDefault();
        });
    }
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