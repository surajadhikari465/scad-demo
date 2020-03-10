$(function () {

    let interval;

    function setupGrid() {
        $("#bulkUploadStatusGrid").igGrid({
            width: "100%",
            fixedHeaders: false,
            columns: [
                { headerText: "ID", key: "BulkItemUploadId", dataType: "number", width: "3%" },
                { headerText: "Type", key: "FileModeType", dataType: "string", width: "2%", formatter: fileTypeFormatter, columnCssClass: "gridCenter" },
                { headerText: "Name", key: "FileName", dataType: "string", width: "25%" },
                { headerText: "UploadedBy", key: "UploadedBy", dataType: "string", width: "10%" },
                {
                    headerText: "Status",
                    key: "Status",
                    dataType: "string",
                    width: "8%",
                    formatter: statusFormatter
                },
                { headerText: "Uploaded At", key: "FileUploadTime", dataType: "date", format: "MM-dd-yyyy hh:mm:ss tt", width: "10%" },
                { headerText: "Message", key: "Message", dataType: "string", width: "30%" },
                { headerText: "", key: "PercentageProcessed", dataType: "string", width: "10%", formatter: getProgress },
                { headerText: "NumberOfRowsWithError", key: "NumberOfRowsWithError", dataType: "string", width: "0%" }
            ],
            features: [{
                name: "Resizing",
                deferredResizing: true
            },
            {
                name: "Filtering",
                columnSettings: [
                    { columnKey: "FileUploadTime", allowFiltering: false },
                    { columnKey: "FileModeType", allowFiltering: false },
                    { columnKey: "PercentageProcessed", allowFiltering: false }
                ]
            },
            {
                name: "Sorting",
                type: "local",
                persist: true
            },
            {
                name: "Paging",
                type: "local",
                pageSize: 20
            }
            ]
        });
    }

    function fileTypeFormatter(val) {
        if (val === 'false')
            return '<img src="/Content/Images/plus-square.svg" style="width: 16px; height: 16px;" />';
        else
            return '<img src="/Content/Images/edit.svg" style="width: 16px; height: 16px;" />';
    }

    function getProgress(val, record) {
        let stat = record === null ? "" : record.Status.toLowerCase();
        
        if (stat === "new" ) {
            return "";
        }
        else {
            let i = record.PercentageProcessed === null ? 0 : record.PercentageProcessed;
         
            if (i <= 0 || i > 100) {
                if (stat != "processing") {
                    i = 100;
                }
            }

            if (stat === "processing") {
                return "<div class='progress'><div class='progress-bar bg-info' role='progressbar' style='width:" + i + "%' aria-valuenow='" + i + "' aria-valuemin='0' aria-valuemax='100'>" + i + " %</div></div>";
            }
            else if (stat === "error") {
               
                return "<div class='progress'><div class='progress-bar bg-danger' role='progressbar' style='width:" + i + "%' aria-valuenow='" + i + "' aria-valuemin='0' aria-valuemax='100'>" + i + " %</div></div>";
            }
            else {
                return "<div class='progress'><div class='progress-bar bg-success' role='progressbar' style='width:100%' aria-valuenow='100' aria-valuemin='0' aria-valuemax='100'>100%</div></div>";   
            }
        }
    }

    function statusFormatter(val, record) {

        if (val === 'Error') {
            let errorRowsCount = record.NumberOfRowsWithError;
            if (errorRowsCount > 0) {
                return "<a href='/Item/BulkUploaderrors?Id=" + record.BulkItemUploadId + "'>Errors: " + errorRowsCount + "</a>";
            }
            else {
                return "<a href='/Item/BulkUploaderrors?Id=" + record.BulkItemUploadId + "'>Error</a>";
            }
        }
        else
            return val;
    }

    function alertSuccess(message) {
        $("#uploadDiv").append('<div class="alert alert-success alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true" >&times;</button ><strong>' + message + '</strong></div >');
    }

    function alertError(message) {
        $("#uploadDiv").append('<div class="alert alert-danger alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><strong>' + message + '</strong></div>');
    }


    function getBulkUploadStatusData(rowCount) {

        $.ajax({
            dataType: "json",
            url: "/Item/BulkUploadStatus?rowCount=" + rowCount,
            data: { rows: rowCount }
        }).done(function (data) {
            var grid = $("#bulkUploadStatusGrid");
            //grid.Rows.Refresh(data);
            grid.igGrid("dataSourceObject", data).igGrid("dataBind");
        });
    }

    function refreshIntervalChanged() {
        clearInterval(interval);
        let timeInSeconds = $("#refreshInterval").val();
        let pageSize = $("#bulkUploadStatusGrid").igGridPaging("option", "pageSize");
        let rowCount = $("#gridRowCount").val();
        interval = setInterval(function () { refreshGrid(rowCount); }, timeInSeconds * 1000);
    }

    function getRecords(value) {
        let numberOfRecordsToBeFetched = $("#gridRowCount").val();
        refreshGrid(numberOfRecordsToBeFetched);
    }

    function refreshGrid(rowCount) {
        getBulkUploadStatusData(rowCount);
    }

    function initialize() {
        setupGrid();
        refreshGrid(100);
    }

    initialize();
    let timeInSeconds = $("#refreshInterval").val();
    let rowCount = $("#gridRowCount").val();
    interval = setInterval(function () { refreshGrid(rowCount); }, timeInSeconds * 1000);

    $('#refreshInterval').change(function () {
        refreshIntervalChanged();
    });

    $('#gridRowCount').change(function () {
        getRecords();
    });

    $('#inputGroupFile').change(
        function (e) {
            let fileExt = ['xlsx'];

            if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExt) == -1) {
                $('#inputGroupFile').val('');
                alert('Unsupported file type has been selected. Supported files types are: ' + fileExt.join(', '));
            }
            else {
                $('#inputFileName').text(e.target.files[0].name);
            }
        });
});


function uploadFile() {
    let selectedFileType = $("input:radio[name=NewOrExistSetSelected]:checked").val();

    if (window.FormData !== undefined) {
        let isRefresh = false;
        let fileUpload = $("#inputGroupFile").get(0);
        let files = fileUpload.files;
        let isSelected = files.length > 0 ? true : false;

        if (isSelected == true) {
            let fName = $("#inputGroupFile").val().split('\\').pop();
            $("#fileName").text($("#inputGroupFile").val().split('\\').pop());

            let fileExt = ['xlsx'];
            if ($.inArray(fName.split('.').pop().toLowerCase(), fileExt) == -1) {
                isSelected = false
                $("#dlgMsg").text('Unsupported file type has been selected. Supported files types are: ' + fileExt.join(', '));
            }
            else {
                $("#dlgMsg").text('Are you sure you would like to upload file?');
            }
        }
        else {
            $("#fileName").text('Not selected');
            $("#dlgMsg").text('Please select the file and try again...');
        }

        //Create FormData object  
        var fileData = new FormData();

        //Limit to one file
        if (isSelected) {
            fileData.append(files[0].name, files[0]);
        }
        fileData.append('fileType', selectedFileType);

        $("#dlgConfirm").dialog({
            resizable: false,
            height: 'auto',
            width: 500,
            modal: true,
            closeOnEscape: true,
            show: { effect: "drop", duration: 300 },
            hide: { effect: "fade", duration: 300 },
            open: function () {
                $("#dlgLink").hide();
                $("#dlgFYI").show();
                $("#dlgFYI").text('File will be uploaded and queued for validation and processing.');
                $('#dlgCancel').text('Cancel');

                $('#progress').hide();
                $('.progress-bar').css('width', '0%');

                $(".ui-dialog-titlebar-close").hide();
                $(".ui-dialog-title").text("User Confirmation Required");

                $(".ui-dialog-buttonpane").show();
                $(".ui-dialog-buttonpane").children().show();
                $(".ui-dialog-titlebar").css("background-color", "gainsboro");

                if (!isSelected) {
                    $('#dlgUpload').hide();
                }
            },
            buttons: {
                'Upload':
                {
                    id: 'dlgUpload',
                    text: 'Upload',
                    click: function () {
                        let refDialog = this; //Ref to dialog so we can close it on success if needed
                        let bar = $('.progress-bar');
                        $('#progress').show();
                        $(".ui-dialog-buttonset").hide();
                        $(".ui-dialog-titlebar-close").hide();
                        $(".ui-dialog-buttonset").children().hide();
                        $(".ui-dialog-title").text("Processing Request...");

                        $("#dlgMsg").html('Uploading file... &nbsp<span class="spinner-border float-right ml-auto text-info" role="status" aria-hidden="true" />');

                        var request = $.ajax({
                            xhr: function () {
                                var xhr = new window.XMLHttpRequest();

                                xhr.upload.addEventListener("progress", function (evt) {
                                    if (evt.lengthComputable) {
                                        var percentComplete = parseInt((evt.loaded / evt.total) * 100);
                                        bar.css('width', percentComplete + '%');

                                        if (percentComplete >= 100) {
                                            //bar.css('width', '100%');
                                            $('#progress').hide();
                                            $("#dlgMsg").html('File uploaded. Processing... Please wait... &nbsp<span class="spinner-border float-right ml-auto text-info" role="status" aria-hidden="true" />');
                                        }

                                    }
                                }, false);

                                return xhr;
                            },

                            url: '/Item/UploadFiles',
                            type: 'POST',
                            contentType: false, // Not to set any content header  
                            processData: false, // Not to process data 
                            data: fileData,

                            success: function (response) {
                                $('#inputGroupFile').val('');
                                $('#inputFileName').text('');
                                $(".ui-dialog-titlebar").css("background-color", "lightGreen");
                                $(".ui-dialog-title").text("System Information");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgCancel').text('Close').show();
                                $("#dlgMsg").html('');
                                $("#dlgMsg").text(response);
                                $('#progress').hide();
                                isRefresh = true;
                                //$(refDialog).dialog("close"); //Uncoment this line to hide dialog
                            },
                            error:
                                function (xhr, status, error) {
                                    $(".ui-dialog-titlebar").css("background-color", "red");
                                    $(".ui-dialog-title").text("System Information:");
                                    $(".ui-dialog-buttonset").show();
                                    $('#dlgCancel').text('Close').show();;
                                    $("#dlgMsg").html('');
                                    $("#dlgFYI").text('Please contact support if error persist.');
                                    $('#progress').hide();

                                    try {
                                        $("#dlgMsg").text(error);
                                    }
                                    catch (err) {
                                        $("#dlgMsg").text('Failed to upload and proces file');
                                    };
                                }
                        });
                    }
                },
                'Cancel':
                {
                    id: 'dlgCancel',
                    text: 'Cancel',
                    click: function () {
                        $(this).dialog("close");
                        if (isRefresh) {
                            let rowCount = $("#gridRowCount").val();
                            getBulkUploadStatusData(rowCount); //Update grid
                        }
                    },
                }
            }
        })
    }
}
