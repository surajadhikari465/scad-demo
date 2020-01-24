$(function () {
	function setupGrid() {
		$("#bulkUploadStatusGrid").igGrid({
			columns: [
				{ headerText: "ID", key: "BulkContactUploadId", dataType: "number", width: "50px" },
				{ headerText: "Name", key: "FileName", dataType: "string", width: "500px" },
				{ headerText: "UploadedBy", key: "UploadedBy", dataType: "string", width: "150px" },
				{
					headerText: "Status",
					key: "Status",
					dataType: "string",
					width: "150px",
					formatter: statusFormatter
				},
				{ headerText: "Uploaded At", key: "FileUploadTime", dataType: "date", format: "MM-dd-yyyy hh:mm:ss tt", width: "200px" },
				{ headerText: "Message", key: "Message", dataType: "string", width: "300px" },
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
				pageSize: 50
			}
			]
		});
	}

/*    
	function setupUploadButton() {
        $('#uploadButton').click(function () {
			if (window.FormData !== undefined) {
				var fileUpload = $("#ExcelAttachment").get(0);
				var files = fileUpload.files;

				// Create FormData object  
				var fileData = new FormData();

				// Looping over all files and add it to FormData object  
				for (var i = 0; i < files.length; i++) {
					fileData.append(files[i].name, files[i]);
				}

				$.ajax({
					url: '/Contact/UploadFiles',
					type: "POST",
					contentType: false, // Not to set any content header  
					processData: false, // Not to process data  
					data: fileData,
					success: function (d) {
						if (d.Result === "Success") {
							console.log(d);
							alertSuccess(d.Message);
							$("#ExcelAttachment").val('');
						} else {
							alertError(d.Message);
						}
					},
					error: function (err) {
						alert(err.statusText);
					}
				});

			} else {
				alert("FormData is not supported.");
			}
		});
	}
*/
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


	function getBulkUploadStatusData(rowCount) {

		$.ajax({
			dataType: "json",
			url: "/Contact/BulkUploadStatus?rowCount=" + rowCount,
			data: { rows: rowCount }
		}).done(function (data) {
			var grid = $("#bulkUploadStatusGrid");
			//grid.Rows.Refresh(data);
			grid.igGrid("dataSourceObject", data).igGrid("dataBind");
		});
	}

	function refreshIntervalChanged() {
		let timeInSeconds = $("#refreshInterval").val();
		let pageSize = $("#bulkUploadStatusGrid").igGridPaging("option", "pageSize");
		let rowCount = $("#gridRowCount").val();
		setInterval(function () { refreshGrid(rowCount); }, timeInSeconds * 1000);
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
		//setupUploadButton();
		refreshGrid(100);
	}

	initialize();

	$('#refreshInterval').change(function () {
		refreshIntervalChanged();
	});
	$('#gridRowCount').change(function () {
		getRecords();
    });
});


function uploadFile() {
    if (window.FormData !== undefined) {
        let isRefresh = false;
        let fileUpload = $("#ExcelAttachment").get(0);
        let files = fileUpload.files;

        let isSelected = files.length > 0 ? true : false;

        if (isSelected == true) {
            $("#fileName").text($("#ExcelAttachment").val().split('\\').pop());
            $("#dlgMsg").text('Are you sure you would like to upload file?');
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
                        $("#dlgMsg").html('Uploading file... &nbsp<span class="spinner-border float-right ml-auto text-primary" role="status" aria-hidden="true" style="color: red" />');

                        var request = $.ajax({
                            url: '/Contact/UploadFiles',
                            type: 'POST',
                            contentType: false, // Not to set any content header  
                            processData: false, // Not to process data 
                            data: fileData,

                            success: function (response) {
                                $(".ui-dialog-titlebar").css("background-color", "lightGreen");
                                $(".ui-dialog-title").text("System Information");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgCancel').text('Close').show();
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
                                function (xhr, status, error) {//(xhr, status, error) {
                                    $(".ui-dialog-titlebar").css("background-color", "red");
                                    $(".ui-dialog-title").text("System Information:");
                                    $(".ui-dialog-buttonset").show();
                                    $('#dlgCancel').show();
                                    let obj = JSON.parse(JSON.parse(xhr.responseText));

                                    try {
                                        $("#dlgMsg").html(obj.message);
                                    }
                                    catch (err) {
                                        if (err != null) {
                                            $("#dlgMsg").html(error);
                                        }
                                        else { $("#dlgMsg").html('Failed to upload and proces file'); }
                                    };

                                    try {
                                        let liItems = [];
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
                        $(this).dialog("close");
                        if (isRefresh) {
                            window.location.reload();
                        }
                    },
                }
            }
        })
    }
}
