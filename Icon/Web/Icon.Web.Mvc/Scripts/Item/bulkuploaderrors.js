var BulkUploadErrorsFunctions= (function() {

    console.log("bulk upload errors");

    function setupGrid() {
        $("#errorsGrid").igGrid({
            width: "85%",
            columns: [
                { headerText: "Row", key: "RowId", dataType: "number", width: "50px" },
                { headerText: "Message", key: "Message", dataType: "string"  }
            ], features: [
                
                 { name: "Resizing" } // this line enables column resizing
            ],
        });
    }

    function getBulkItemUploadErrorData(id) {

        $.get({
            dataType: "json",
            url: "/Item/GetBulkUploadErrors?Id=" + id
        }).done(function(data) {
            $("#errorsGrid").igGrid("dataSourceObject", data).igGrid("dataBind");            
            if (data.length == 0) {               
                $("#anch_BulkItemUploadId").addClass('disabled').removeAttr("href");
            }
        });
    }

    function refreshGrid(id) {
        getBulkItemUploadErrorData(id);
    }


    function initialize(id) {
        setupGrid();
        
        refreshGrid(id);
    }


    return {
        init: function(id) {
            initialize(id);
        }
    };

}());
