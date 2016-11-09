var subTeamNameRegEx = '/^[A-Za-z\s]+$/';

$(function () {    
    rowEditSaveChanges();
});

function validatePeopleSoftNumberUniqeness(evt, ui) {
    var value = ui.value;
    var rows = $("#subTeamGrid").igGrid("rows");

    for (var i = 0; i < rows.length; i++) {
        var currentRow = rows[i];
        var currentValue = $("#subTeamGrid").igGrid("getCellValue", $(currentRow).attr("data-id"), "PeopleSoftNumber");

        if (value == currentValue && $(currentRow).find(".ui-iggrid-editingcell").length == 0) {
            ui.message = "Please enter a unique PeopleSoft number.";
            return false;
        }
    }
}

function validatePosDeptNumberUniqeness(evt, ui) {
    var value = ui.value;
    var rows = $("#subTeamGrid").igGrid("rows");

    if (value != null && value != "") {
        for (var i = 0; i < rows.length; i++) {
            var currentRow = rows[i];
            var currentValue = $("#subTeamGrid").igGrid("getCellValue", $(currentRow).attr("data-id"), "PosDeptNumber");

            if (value == currentValue && $(currentRow).find(".ui-iggrid-editingcell").length == 0) {
                ui.message = "Please enter a unique POS Dept number.";
                return false;
            }
        }
    }
}

function validateSubTeamUniqeness(evt, ui) {
    var value = ui.value;
    var rows = $("#subTeamGrid").igGrid("rows");

    for (var i = 0; i < rows.length; i++) {
        var currentRow = rows[i];
        var currentValue = $("#subTeamGrid").igGrid("getCellValue", $(currentRow).attr("data-id"), "SubTeamName");

        if (value == currentValue && $(currentRow).find(".ui-iggrid-editingcell").length == 0) {
            ui.message = "Please enter a unique Sub-Team name.";
            return false;
        }
    }
}

function rowEditSaveChanges() {
    $("#subTeamGrid").on("iggridupdatingeditrowended", function (evt, ui) {
        if (ui.update) {
            $("#subTeamGrid").igGrid("saveChanges");
        }
    });
}

function onSuccess(data) {
    var message;
    var isError;

    if (data.Success) {
        message = "Update was applied successfully.";
        isError = false;
        displayAlertMessage(message, isError);
    } else {
        message = "Unable to apply update.  Please refresh the grid and check for duplicate sub-team names, PeopleSoft numbers, or POS Dept Numbers.";
        isError = true;
        displayAlertMessage(message, isError);
    }
}

function onError() {
    var message = "Unable to apply update.  Please refresh the grid and check for duplicate sub-team names, PeopleSoft numbers, or POS Dept Numbers.";
    var isError = true;
    displayAlertMessage(message, isError);
}