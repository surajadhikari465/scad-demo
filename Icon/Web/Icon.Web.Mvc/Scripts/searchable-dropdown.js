$(function () {
    $("input[data-icon-searchabledropdown]").each(applyDropDown);
});

function applyDropDown() {

    var $input = $(this);
    var variableHolder = "#";
    var idFieldName = $input.attr("data-dropdown-selectedId");
    var idFieldHolder = variableHolder + idFieldName

    var options = {
                    source: $input.attr("data-icon-searchabledropdown"),
                    delay: 0,
                    minLength: 3,
                    select:
                            function (event, ui)
                            {
                                $input.val(ui.item.label);
                                $(idFieldHolder).val(ui.item.valueID);
                                return false;
                            },
                    focus:
                            function (event, ui)
                            {
                                $input.val(ui.item.label);
                                $(idFieldHolder).val(ui.item.valueID);
                                return false;
                            }

                };

    $input.autocomplete(options);
}