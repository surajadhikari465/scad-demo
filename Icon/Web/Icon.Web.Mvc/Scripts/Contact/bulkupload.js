$(function () {
	function setupGrid() {
        $("#bulkUploadStatusGrid").igGrid({
            primaryKey: "BulkContactUploadId",
            height: "520px",
            rowHeight: "5px",
			columns: [
                { headerText: "ID", key: "BulkContactUploadId", dataType: "number", width: "3%", hidden: true },
				{ headerText: "File Name", key: "FileName", dataType: "string", width: "45%" },
				{ headerText: "Uploade dBy", key: "UploadedBy", dataType: "string", width: "25%" },
				{ headerText: "Uploaded At", key: "FileUploadTime", dataType: "date", format: "MM-dd-yyyy hh:mm:ss tt", width: "20%" },
			],
			features: [{
				name: "Resizing",
				deferredResizing: true
			},
			{
				name: "Filtering",
				columnSettings: [
					{ columnKey: "FileUploadTime", allowFiltering: false },
					{ columnKey: "FileModeType", allowFiltering: false }
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
                pageSize: 10,
                hidden: true
			}
			]
		});
	}

	//function fileTypeFormatter(val) {
	//	if (val === 'false')
	//		return '<img src="/Content/Images/plus-square.svg" style="width: 16px; height: 16px;" />';
	//	else
	//		return '<img src="/Content/Images/edit.svg" style="width: 16px; height: 16px;" />';
	//}

	function statusFormatter(val, record) {

		if (val === 'Error')
			return "<a href='/Contact/BulkUploaderrors?Id=" + record.BulkContactUploadId + "'>Error</a>";
		else
			return val;
	}

    function refreshGrid(rowCount) {
		getBulkUploadStatusData(rowCount);
	}

	function initialize() {
		setupGrid();
		refreshGrid(1000);
	}

    initialize();


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


function getBulkUploadStatusData(rowCount) {
    $.ajax({
        dataType: "json",
        url: "/Contact/BulkUploadStatus?rowCount=" + rowCount,
        data: { rows: rowCount }
    }).done(function (data) {
        var grid = $("#bulkUploadStatusGrid");
        grid.igGrid("dataSourceObject", data).igGrid("dataBind");
    });
}

function uploadFile() {
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

        $("#dlgConfirm").dialog({
            resizable: false,
            height: 'auto',
            width: 500,
            modal: true,
            closeOnEscape: true,
            show: { effect: "drop", duration: 300 },
            hide: { effect: "fade", duration: 300 },
            open: function () {
                $('ul').empty();
                $("#dlgLink").hide();
                $("#dlgFYI").show();

                $('#progress').hide();
                $('.progress-bar').css('width', '0%');

                $(".ui-dialog-titlebar-close").hide();
                $(".ui-dialog-title").text("User Confirmation Required");

                $(".ui-dialog-buttonpane").show();
                $(".ui-dialog-buttonpane").children().show();
                $(".ui-dialog-titlebar").css("background-color", "gainsboro");

                if(!isSelected) {
                    $('#dlgUpload').hide();
                }
            },
            buttons: {
                'Upload':
                {
                    id: 'dlgUpload',
                    text: 'Upload',
                    click: function () {
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
                                            $("#dlgMsg").html('File uploaded. Validating data... Please wait... &nbsp<span class="spinner-border float-right ml-auto text-info" role="status" aria-hidden="true" />');
                                        }

                                    }
                                }, false);

                                return xhr;
                            },

                            url: '/Contact/UploadFiles',
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
                            },
                            error:
                                function (xhr, status, error) {
                                    $(".ui-dialog-titlebar").css("background-color", "red");
                                    $(".ui-dialog-title").text("System Information:");
                                    $(".ui-dialog-buttonset").show();
                                    $('#dlgCancel').show();
                                    $("#dlgMsg").html('');
                                    $("#dlgFYI").hide();
                                    $('#progress').hide();

                                    try {
                                        $("#dlgMsg").text(error);
                                    }
                                    catch (err) {
                                        $("#dlgMsg").text('Failed to upload and proces file');
                                    };

                                    try {
                                        let liItems = [];
                                        let obj = JSON.parse(JSON.parse(xhr.responseText));
                                        
                                        let ar = obj.validation;
                                        if (ar !== null) {
                                            $.each(ar, function (i, item) {
                                                liItems.push($('<li/>').text(item.key + ': ' + item.value));
                                            })
                                        }
                                        else {
                                            liItems.push($('<li/>').text('No validation messages'));
                                        };

                                        $('#ulList').append.apply($('#ulList'), liItems);

                                        $("#dlgLink").attr("href", '/Contact/DownloadRefFile?fileName=' + obj.fileName);
                                        $("#dlgLink").show();
                                    }
                                    catch{}
                                }
                        });
                    }
                },
                'Cancel':
                {
                    id: 'dlgCancel',
                    text: 'Cancel',
                    click: function () {
                        if (isRefresh) {
                            //window.location.reload();   //Reload page
                            getBulkUploadStatusData(1000); //Update grid
                        }
                        $(this).dialog("close");
                    },
                }
            }
        })
    }
}
