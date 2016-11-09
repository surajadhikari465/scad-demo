$(function () {
    $("input[data-icon-autocomplete]").each(applyAutocomplete);
});

function applyAutocomplete() {

    var $input = $(this);

    var options = {
        source: $input.attr("data-icon-autocomplete"),
        delay: 0,
        minLength: 3
    };

    $input.autocomplete(options);
}