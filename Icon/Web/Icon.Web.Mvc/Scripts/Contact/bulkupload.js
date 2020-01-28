$(function () {
	function setupGrid() {
        $("#bulkUploadStatusGrid").igGrid({
            primaryKey: "BulkContactUploadId",
            height: "520px",
            rowHeight: "5px",
			columns: [
                { headerText: "ID", key: "BulkContactUploadId", dataType: "number", width: "3%", hidden: true },
				{ headerText: "File Name", key: "FileName", dataType: "string", width: "45%" },
				{ headerText: "UploadedBy", key: "UploadedBy", dataType: "string", width: "25%" },
				{
					headerText: "Status",
					key: "Status",
					dataType: "string",
					width: "10%",
					formatter: statusFormatter
				},
				{ headerText: "Uploaded At", key: "FileUploadTime", dataType: "date", format: "MM-dd-yyyy hh:mm:ss tt", width: "20%" },
				//{ headerText: "Message", key: "Message", dataType: "string", width: "300px" },
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

	function fileTypeFormatter(val) {
		if (val === 'false')
			return '<img src="/Content/Images/plus-square.svg" style="width: 16px; height: 16px;" />';
		else
			return '<img src="/Content/Images/edit.svg" style="width: 16px; height: 16px;" />';
	}

	function statusFormatter(val, record) {

		if (val === 'Error')
			return "<a href='/Contact/BulkUploaderrors?Id=" + record.BulkContactUploadId + "'>Error</a>";
		else
			return val;
	}

	function alertSuccess(message) {
		$("#uploadDiv").append('<div class="alert alert-success alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true" >&times;</button ><strong>' + message + '</strong></div >');
	}

	function alertError(message) {
		$("#uploadDiv").append('<div class="alert alert-danger alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><strong>' + message + '</strong></div>');
	}

	//function getBulkUploadStatusData(rowCount) {
	//	$.ajax({
	//		dataType: "json",
	//		url: "/Contact/BulkUploadStatus?rowCount=" + rowCount,
	//		data: { rows: rowCount }
	//	}).done(function (data) {
	//		var grid = $("#bulkUploadStatusGrid");
	//		//grid.Rows.Refresh(data);
	//		grid.igGrid("dataSourceObject", data).igGrid("dataBind");
	//	});
	//}

	//function refreshIntervalChanged() {
	//	let timeInSeconds = $("#refreshInterval").val();
	//	let pageSize = $("#bulkUploadStatusGrid").igGridPaging("option", "pageSize");
	//	let rowCount = $("#gridRowCount").val();
	//	setInterval(function () { refreshGrid(rowCount); }, timeInSeconds * 1000);
	//}

	//function getRecords(value) {
	//	let numberOfRecordsToBeFetched = $("#gridRowCount").val();
	//	refreshGrid(numberOfRecordsToBeFetched);
	//}

    function refreshGrid(rowCount) {
		getBulkUploadStatusData(rowCount);
	}

	function initialize() {
		setupGrid();
		refreshGrid(100);
	}

    initialize();

    //$('#refreshInterval').change(function () {
    //    refreshIntervalChanged();
    //});

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
                        let refDialog = this; //Ref to dialog so we can close it on success
                        $(".ui-dialog-buttonset").hide();
                        $(".ui-dialog-titlebar-close").hide();
                        $(".ui-dialog-buttonset").children().hide();
                        $(".ui-dialog-title").text("Processing Request...");
                        $("#dlgMsg").html('Uploading file... &nbsp<span class="spinner-border float-right ml-auto text-info" role="status" aria-hidden="true" />');

                        var request = $.ajax({
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
                                isRefresh = true;

                                //$(refDialog).dialog("close"); //Comment this line if confirmation is shown
                                /*//Full grid update with POST if full refresh needed
                                //let grd = $('#bulkUploadStatusGrid');
                                //grd.igGrid('dataBind');*/

                                //let grd = $('#contactGrid').data('igGrid'); //remove deleted record from UI without POST
                                //grd.dataSource.deleteRow(contactId);
                                //grd.commit();
                            },
                            error:
                                function (xhr, status, error) {
                                    $(".ui-dialog-titlebar").css("background-color", "red");
                                    $(".ui-dialog-title").text("System Information:");
                                    $(".ui-dialog-buttonset").show();
                                    $('#dlgCancel').show();
                                    $("#dlgMsg").html('');

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
                            getBulkUploadStatusData(100); //Update grid
                        }
                        $(this).dialog("close");
                    },
                }
            }
        })
    }
}
