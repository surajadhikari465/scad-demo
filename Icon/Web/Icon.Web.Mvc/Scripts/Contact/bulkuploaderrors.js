var BulkUploadErrorsFunctions = (function () {

	console.log("bulk upload errors");

	function setupGrid() {
		$("#errorsGrid").igGrid({
			width: "85%",
			columns: [
				{ headerText: "Row", key: "RowId", dataType: "number", width: "50px" },
				{ headerText: "Message", key: "Message", dataType: "string" }
			], features: [

				{ name: "Resizing" } // this line enables column resizing
			],
		});
	}

	function getBulkContactUploadErrorData(id) {

		$.get({
			dataType: "json",
			url: "/Contact/GetBulkUploadErrors?Id=" + id
		}).done(function (data) {
			$("#errorsGrid").igGrid("dataSourceObject", data).igGrid("dataBind");
		});
	}

	function refreshGrid(id) {
		getBulkContactUploadErrorData(id);
	}


	function initialize(id) {
		setupGrid();

		refreshGrid(id);
	}


	return {
		init: function (id) {
			initialize(id);
		}
	};

}());
