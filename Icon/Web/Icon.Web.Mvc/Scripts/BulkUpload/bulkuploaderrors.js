var BulkUploadErrorsFunctions = (function () {

    const bulkUploadDataType = $('#bulkUploadDataType').val();

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

    function getBulkUploadErrorData(id) {

        $.get({
            dataType: "json",
            url: `/BulkUpload/GetBulkUploadErrors?bulkUploadDataType=${bulkUploadDataType}&id=${id}`
        }).done(function(data) {
            $("#errorsGrid").igGrid("dataSourceObject", data).igGrid("dataBind");            
            if (data.length == 0) {               
                $("#anch_BulkUploadId").addClass('disabled').removeAttr("href");
            }
        });
    }

    function refreshGrid(id) {
        getBulkUploadErrorData(id);
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
