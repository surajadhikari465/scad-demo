$(document).ready(function () {
    if ($('#reprocess').length !== 0) {
        $('#reprocess').click(function () {
            reprocessFailedData('igGrid', 'reprocess', '/FailedProductSelectionGroupMessage/Reprocess');
        });
    }
});