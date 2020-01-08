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
		setupUploadButton();
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
