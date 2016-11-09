var $searchBtn;

// Tracks when search is happening to help with button can be disabled/enabled
var isdearching = false;

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

    $searchBtn = $('#search-button');

    enableSearchButton();
    rowEditSaveChanges(); 
    
  
    $('#search-button').on('click', function () {
       
    })

}


function rowEditSaveChanges() {
    $("#igGrid").on("iggridupdatingeditrowended", function (evt, ui) {
        if (ui.update) {
            $("#igGrid").igGrid("saveChanges",
                function saveSuccess(data) {
                    if (data.Success === true) {
                        displayAlertMessage('Successfully updated item.', false);
                    }
                    else {
                        displayAlertMessage(data.Error, true);
                    }
                },
                function saveFailure() {
                    displayAlertMessage('There was an unexpected error updating the item.', true);
                });
        }
     
    });

    $("#igGrid2").on("iggridupdatingeditrowended", function (evt, ui) {
        if (ui.update) {
            $("#igGrid2").igGrid("saveChanges",
                function saveSuccess(data) {
                    if (data.Success === true) {
                        displayAlertMessage('Successfully updated item.', false);
                    }
                    else {
                        displayAlertMessage(data.Error, true);
                    }
                },
                function saveFailure() {
                    displayAlertMessage('There was an unexpected error updating the item.', true);
                });
        }

    })
}

function setColumnHeaderTooltips() {
    $('#igGrid_Region').attr("title", "Region");
    $('#igGrid_Identifier').attr("title", "Identifier");
    $('#igGrid_BrandName').attr("title", "Brand Name");
    $('#igGrid_ItemDescription').attr("title", "Item Description");
    $('#igGrid_PosDescription').attr("title", "POS Description");
    $('#igGrid_PackageUnit').attr("title", "Item Pack");
    $('#igGrid_RetailSize').attr("title", "Retail Size");
    $('#igGrid_RetailUom').attr("title", "Retail UOM");
    $('#igGrid_FoodStamp').attr("title", "Food Stamp Eligible");
    $('#igGrid_PosScaleTare').attr("title", "POS Scale Tare");
    $('#igGrid_MerchandiseHierarchyClassId').attr("title", "Merchandise Hierarchy");
    $('#igGrid_TaxHierarchyClassId').attr("title", "Tax Class");
}
