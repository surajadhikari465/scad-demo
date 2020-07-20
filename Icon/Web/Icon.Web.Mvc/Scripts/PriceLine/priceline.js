var selectedPriceLine = null;

$(document).ready(function () {
    var priceLineTable = $('#tblPriceLine').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/PriceLine/AllPriceLine",
            "type": 'POST'
        },
        columns: [
            { data: "PriceLineId", name: "Price Line Id", autoWidth: false },
            { data: "PriceLineDescription", name: "Description", autoWidth: true },
            { data: "PriceLineSize", name: "Size", autoWidth: true },
            { data: "PriceLineUOM", name: "UOM", autoWidth: true },
            { data: "PrimaryItemUpc", name: "Primary Item Upc", autoWidth: true },
            { data: "CountOfItems", name: "Count Of Items", autoWidth: true, searchable: false },
            {
                data: "PriceLineId",
                render: function (data, type, row, meta) {
                    return '<a href="/PriceLine/' + data + '">Edit</a>';
                },
                orderable: false,
                searchable: false
            }
        ],
        lengthMenu: [[20, 50, 100, 1000], [20, 50, 100, 1000]],
        loadingRecords: "Loading price line data... Please wait...",
        initComplete: function () {
            $('.dataTables_filter input').unbind();
            $('.dataTables_filter input').bind('keyup', function (e) {
                var code = e.keyCode || e.which;
                if (code == 13) {
                    priceLineTable.search(this.value).draw();
                }
            });
        },
    });


    $('#tblPriceLine tbody').on('click', 'td.details-control', function () {
        let tr = $(this).closest('tr');
        let row = priceLineTable.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            row.child(format(row.data()), 'child').show();
            tr.addClass('shown');
        }
    });

    $('#tblPriceLine tbody').on('click', 'td.action-control', function (e) {
        selectedPriceLine = null;
        let tr = $(this).closest('tr');
        let row = priceLineTable.row(tr);
        selectedPriceLine = row.data();

        var top = e.pageY - 10;
        var left = e.pageX - 40;

        $("#context-menu").css({
            display: "block",
            top: top,
            left: left,
            width: "30px"
        });

        e.stopPropagation(); //To prevent menu close
    });


    $(document).on("click", function (event) {
        if ($('#context-menu').is(":visible")) {
            $('#context-menu').slideUp("fast");
        }
    });

    $("div.dropdown-menu a").click(function () {
        $("#context-menu").hide();
        let key = $(this).attr("id");

        if (window.selectedPriceLine != null && window.selectedPriceLine != undefined) {
            switch (key) {
                case "mnuEdit":
                    $(location).attr('href', '/Priceline?priceLineId=' + window.selectedPriceLine.PriceLineId);
                    break;
                    break;
                case 'mnuDetail':
                    let r = $('#tblPriceLine').DataTable().row('#id' + window.selectedPriceLine.PriceLineId);
                    if (r != null && r != undefined) {
                        //if (!r.child.isShown()){
                        $('td.details-control', r.node()).trigger('click');
                        //}
                    }
                    break;
                default:
                    break;
            }
        }
    })
});

