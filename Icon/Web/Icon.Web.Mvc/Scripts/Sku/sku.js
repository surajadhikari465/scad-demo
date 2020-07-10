$(document).ready(function () {
    var skuTable = $('#tblSku').DataTable({
        "ajax": '/Sku/AllSku',
        columns: [
            { data: "SkuId", name: "SkuId", autoWidth: false },
            { data: "SkuDescription", name: "Sku Description", autoWidth: true },
            { data: "PrimaryItemUpc", name: "Primary Item Upc", autoWidth: true },
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
        lengthMenu: [[20, 50, 100, -1], [20, 50, 100, "All"]],
        loadingRecords: "Loading Sku data... Please wait...",
    });
    skuTable.on('init.dt', function () {
        // Get the Sku Count for all skus
        $.getJSON("/Sku/AllSkuCount", function (data) {

            // Update Sku Cells
            var skuCountTable = new Object();
            $.each(data, function (index, skuItemCount) {
                skuCountTable[skuItemCount.SkuId] = skuItemCount.CountOfItems;
            });

            skuTable.rows().every(function () {
                var skuData = this.data();
                if (skuCountTable[skuData.SkuId] != null) {
                    skuData.CountOfItems = skuCountTable[skuData.SkuId];
                    this.invalidate(); // invalidate the data DataTables has cached for this row
                }
            });

            // Draw once all updates are done
            table.draw();
        });
    });
});