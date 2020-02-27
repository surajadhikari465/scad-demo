$(document).ready(function () {
    $('form').submit(function (e) {
        e.preventDefault();
    });

    var getStoresSuccessCallback = function (data, textStatus, xhr) {
        var storesControl = $("#Stores");
        storesControl.empty();
        $.each(data,
            function (index, store) {
                storesControl.append(
                    $("<option />",
                        {
                            value: store.BusinessUnit,
                            text: store.BusinessUnit + ": " + store.Name
                        }))
            });
        //set stores select box height
        if (data) {
            var size = data.length > 1 && data.length < 41 ? data.length : 40;
            $(storesControl).attr("size", size);
        }
    }

    var getStoresErrorCallback = function (xhr, textStatus, errorThrown) {
        responseHTML = $.parseHTML(xhr.responseText);

        var errorInfo = "Error attempting to load store data for region.\n"
            + "url: \\PriceReset\\Stores \n"
            + "data: \"" + $("#RegionIndex :selected").text() + "\" (" + $("#RegionIndex").val() + ")\n"
            + "call state: \"" + xhr.state() + "\".\n"
            + "response: \"" + errorThrown + "\" (" + xhr.status + ")\n"
            + "server error: \"" + $(responseHTML).find('h2').text() + "\"\n";
        console.log(errorInfo);
        console.log(xhr.responseText);
        alert(errorInfo)

        $("#Stores").empty();
    }

    $("#RegionIndex").change(function () {
        var data = { regionCode: $(this).val() };

        var storesControl = $("#Stores");
        var loadingDiv = $("#storesDiv");

        storesControl.fadeOut();
        storesControl.addClass("hidden");
        loadingDiv.removeClass("hidden");

        $.getJSON('PriceReset/Stores', data, getStoresSuccessCallback)
            .fail(getStoresErrorCallback);

        loadingDiv.addClass("hidden");
        storesControl.removeClass("hidden");
        storesControl.fadeIn();
    });

    $("#btnSubmit").click(function (e) {
        return isValid(); //Prevent dialog to show if validation fails
    });

    $('#dlgConfirm').on('shown.bs.modal', function () {
        let isOK = isValid();
        let isR10 = isOK ? isR10Selected() : false;

        $("#dlgBusy").hide();
        $(".dlgWarning").show();
        $("#dlgSubmit").attr('disabled', !isOK);
        $("#headerText").text('User Confirmation Required');
        $("#dlgHeader").removeClass("bg-danger").addClass("bg-info");
        $("#dlgText").text(isOK ? 'Are you sure you would like to send GPM price messages to ESB?' : 'Validation failed. Please verify your selection and try again.');

        if(!isR10) {
            $("#dlgR10Warning").hide();
        }
    })

});

function isR10Selected() {
    let isAll = false;

    $('#lbSystems :selected').each(function (i, selected) {
        if ($(selected).text().toUpperCase() === 'R10') {
            isAll = (jQuery.inArray("*", $('#Items').val().split('\n'))) === 0;
        }
    });
    return isAll;
}

function isValid() {
    let frm = $('#priceRefresh');
    frm.validate();
    return frm.valid();
}

function sendMessages() {
    let isOK = isValid()

    if (isOK) {
        let view = $("#priceRefresh").serialize();
        $("#dlgX").attr('disabled', true);
        $("#dlgCancel").attr('disabled', true);
        $("#dlgSubmit").attr('disabled', true);
        $("#dlgText").text('Processing request... Please wait...');
        $("#dlgBusy").show();
        $(".dlgWarning").hide();

        $.ajax({
            url: "/PriceRefresh/Index",
            type: "POST",
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: view,

            success: function (response) {
                $("#dlgBusy").hide();
                $("#dlgX").attr('disabled', false)
                $("#dlgCancel").attr('disabled', false);
                $("#dlgHeader").removeClass("bg-info").removeClass("bg-danger").addClass("bg-success");
                $("#dlgText").text(response);
                $("#dlgWarning").hide();
            },

            error:
                function (xhr, status, error) {
                    $("#dlgBusy").hide();
                    $("#dlgX").attr('disabled', false);
                    $("#dlgCancel").attr('disabled', false);
                    $("#headerText").text('Request Failed');
                    $("#dlgHeader").removeClass("bg-info").addClass("bg-danger");
                    $("#dlgText").text(JSON.parse(xhr.responseText));
                }
        });
    }
    else {
        $("#dlgSubmit").attr('disabled', true);
        $("#dlgText").text('Validation failed. Please verify your selection and try again.');
    }
}