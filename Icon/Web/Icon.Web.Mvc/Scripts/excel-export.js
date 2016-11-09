function exportItems() {
    var dataSource = $('#igGrid').igGrid('option', 'dataSource').Records;
    var scanCodes = [];

    for (var i = 0; i < dataSource.length; i++) {
        scanCodes[i] = dataSource[i].ScanCode;
    }

    var postData = { items: scanCodes };

    $.fileDownload('/Excel/ItemExport', {
        httpMethod: "POST",
        data: postData
    });
}