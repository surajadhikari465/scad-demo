$(document).ready(function () {
    var skuTable = $('#tblSku').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Sku/AllSku",
            "type": 'POST'
        },
        columns: [
            { data: "SkuId", name: "SKU ID", autoWidth: false },
            { data: "SkuDescription", name: "SKU Description", autoWidth: true },
            { data: "PrimaryItemUpc", name: "SKU Primary Scan Code", autoWidth: true },
            { data: "CountOfItems", name: "Count Of Items", autoWidth: true, searchable: false },
            {
                data: "SkuId",
                render: function (data, type, row, meta) {
                    return '<a href="/sku/' + data + '">Edit</a>';
                },
                orderable: false,
                searchable: false
            }
        ],
        lengthMenu: [[20, 50, 100, 1000], [20, 50, 100, 1000]],
        loadingRecords: "Loading Sku data... Please wait...",
        searchDelay: 2000,
        initComplete: function () {
            $('.dataTables_filter input').unbind();
            $('.dataTables_filter input').bind('keyup', function (e) {
                var code = e.keyCode || e.which;
                if (code == 13) {
                    skuTable.search(this.value).draw();
                }
            });
        },
    });
});