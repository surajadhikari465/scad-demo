function reprocessFailedData(gridId, reprocessButtonId, url) {
    var grid = $('#' + gridId);
    var reprocessButton = $('#' + reprocessButtonId);

    reprocessButton.disable(true);

    var selectedRows = grid.igGrid('selectedRows')
    var failedData = [];

    $.each(selectedRows, function (index, selectedRow) {
        var failedDataRow = grid.igGrid('option', 'dataSource').Records[selectedRow.index];
        failedData.push(failedDataRow);
    });
    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(failedData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.Success) {
                alertMessage('alert', 'Successfully reprocessed items.', false);
                $.each(data.ReprocessedData, function (index, reprocessedData) {
                    grid.igGridUpdating("deleteRow", reprocessedData.Id);
                });
            } else {
                alertMessage('alert', data.Error, true);
            }
        },
        error: function (data) {
            alertMessage('alert', 'Unexpected error occured while processing items.', true);
        },
        complete: function () {
            grid.igGridSelection("clearSelection")
            reprocessButton.disable(false);
        }
    });
}