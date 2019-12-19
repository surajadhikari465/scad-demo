$(document).ready(function () {

    $('form').submit(function () {
        if ($(':focus') != null) {
            $(':focus').blur(); //Remove focuse to force saving changes
        }

        $(this).validate();
        if ($(this).valid()) {
            $(':submit', this).attr('disabled', true).val('Processing...');
            $('#divBlock').show();
        }
    });

    const isAttributeActionUpdate = $("#attributeAction").first().val() === 'Update';

    let processDataTypeIdChange = () => {
        var $dataTypeId = $("select[name='DataTypeId']");
        var dataTypeId = $dataTypeId.val();
        var appendToUrl = 'dataTypeId=' + dataTypeId;

        if ($("#AttributeId").length) {
            var attributeId = $("#AttributeId").val();
            appendToUrl = appendToUrl + '&attributeId=' + attributeId;
        }

        $.get('/Attribute/GetDataTypeView?' + appendToUrl, function (data) {
            $('#partialViewPlaceHolder').html(data);
            $('#partialViewPlaceHolder').fadeIn('fast', () => {
                processSpecCharChange();
                processSpecCharTypeChange();
                processIsPickListChange();

                if (isAttributeActionUpdate) {
                    disableFieldsForUpdate();
                }
            });
        });
    };

    let processSpecCharChange = () => {
        var $elem = $("#cbx_special_characters");
        if ($elem.length > 0) {
            var val = $elem[0].checked;
            if (val) {
                if (isAttributeActionUpdate) {
                    if (!$('#special_char_all').prop('checked') && !$('#special_char_specific').prop('checked')) {
                        $('#special_char_all').click();
                    }
                }
                $("#special_characters .content").show();
            } else {
                $("#special_characters .content").hide();
            }
        }
    };

    let processSpecCharTypeChange = () => {
        var $elem = $('input[type=radio][name=SpecialCharacterSetSelected]');
        if ($elem.length > 0) {
            if ($elem[0].checked) {
                $(".special-characters-allowed-specific").attr("disabled", "disabled");
                $(".special-characters-allowed-all").removeAttr("disabled");
            } else {
                $(".special-characters-allowed-all").attr("disabled", "disabled");
                $(".special-characters-allowed-specific").removeAttr("disabled");
            }
        }
    };

    let processIsPickListChange = () => {
        var $elem = $('input[type=radio][name=IsPickList]');
        if ($elem.length > 0) {
            if ($elem[0].checked) {
                $("#picklist").removeClass("d-none");
                if($(".pick-list-item").length == 0) { processPickListAdd(); } //Add default empty item
            } else {
                $("#picklist").addClass("d-none");
            }
        }
    };

    let processPickListAdd = () => {
        var id = -1;
        var isEmpty = false;
        $elem = $("#picklist .content");
        var items = $elem.find("input.pick-list-item");

        $.each(items, function (inx, item) {
            id = parseInt(item.id.replace('pld_', ''));

            if (item.value.trim().length == 0) {
                item.focus();
                isEmpty = true;
                return false;
            }
        });

        if (!isEmpty) {
            ++id;
            $elem.append('<div class="row" id="div_' + id + '" style="display:flex; margin-left: auto; margin-bottom: 3px"><input class="form-control form-input pick-list-item" name="PickListData[' + id + '].PickListValue" type="text" value="" id=pld_' + id + '><input type="button" class="btn btn-link" id="btnRemovePickList_' + id + '" value="remove"/></div>');
            var item = $("#pld_" + id);
            item.focus();
        }
    };

    let disableFieldsForUpdate = () => {
        $('#cbx_special_characters:checked,.character-set-cbx:checked,#special_char_all:checked')
            .click((e) => e.preventDefault())
            .addClass('disabled-checkbox');

        if ($('#special_char_all:checked').length > 0) {
            $('#special_char_specific')
                .click((e) => e.preventDefault())
                .addClass('disabled-checkbox');
        }

        $('.pick-list-item').prop('readonly', true);
    }

    //Hide PickList value. Validation rule is applied later on to verify deleted value is not in use in Item table.
    let deletePickListData = (deleteControlId) => {
        $("#" + deleteControlId).prev('input[type=hidden]').val('True');
        $("#" + deleteControlId).parent('div.row').hide();
    };

    //Removes PickList value. No validation against Item table needed (new value)
    let removePickListData = (removeControlId) => {
        var id = parseInt(removeControlId.replace('btnRemovePickList_', ''));
        var items = $(".pick-list-item");
        var i = 0;
        
        for (i = id; i < items.length - 1; i++) {
            items[i].value = (items[i + 1].value.trim());
        }

        $("#btnRemovePickList_" + (i)).parent().remove();

        if(i == 0) {
            var $opt = $('input[type=radio][name=IsPickList]');
            if ($opt.length > 0) {
                $opt[1].checked = true;
                processIsPickListChange();
            }
        }
    };

    $(document).on('change', "select[name='DataTypeId']", function (e) {
        processDataTypeIdChange();
    });
    $(document).on('change', "#cbx_special_characters", function (e) {
        processSpecCharChange();
    });
    $(document).on('change', 'input[type=radio][name=SpecialCharacterSetSelected]', function () {
        processSpecCharTypeChange();
    });
    $(document).on('change', 'input[type=radio][name=SpecialCharacterSetSelected]', function () {
        processSpecCharTypeChange();
    });
    $(document).on('change', 'input[type=radio][name=IsPickList]', function () {
        processIsPickListChange();
    });
    $(document).on('click', 'input#btnAddPickList', function () {
        processPickListAdd();
    });
    $(document).on('click', 'input[id*="btnDeletePickList"]', function () {
        deletePickListData($(this)[0].id);
    });
    $(document).on('click', 'input[id*="btnRemovePickList"]', function () {
        removePickListData($(this)[0].id);
    });

    // init
    processDataTypeIdChange();
});