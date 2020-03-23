var selectedItem = null;

$(function () {
    var tbl = $('#tblUpload').DataTable({
        rowId: function (val) { return 'id' + val.BulkContactUploadId },
        paging: true,
        ordering: true,
        order: [[0, "desc"]],
        info: true,
        sDom: 'RSlftip',
        processing: true,
        responsive: true,
        orderMulti: false,
        keys: false,    //Key navigation
        select: false,  //Row selection
        scrollY: "50vh",
        emptyTable: "No upload data available...",
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        infoEmpty: "",
        loadingRecords: "Loading upload files data... Please wait...",
        processing: "Processing data...",

        language: {
            info: "Total Files: _TOTAL_",
            search: "Search Files ",
            lengthMenu: "Show &nbsp _MENU_ &nbsp files",
        },

        ajax: {
            url: "/Contact/BulkUploadStatus?rowCount=10000",
            type: "POST",
            datatype: "json"
        },

        columnDefs: [
            {
                targets: [0],
                visible: false,
                orderable: true,
                searchable: false
            },
            {
                targets: [3],
                orderable: true,
                searchable: false,
            },
            {
                targets: [4],
                orderable: false,
                searchable: false,
                width: 5
            },
        ],

        columns: [
            { data: "BulkContactUploadId", name: "BulkContactUploadId", autoWidth: false },
            { data: "FileName", name: "File Name", autoWidth: true },
            { data: "UploadedBy", name: "Uploaded By", autoWidth: true },
            { data: "FileUploadTime", name: "Uploaded At", autoWidth: true },
            { class: "action-control text-info", orderable: false, data: null, defaultContent: '<p class="action-control fa fa-bars text-secondary m-1"/>' },
        ]
    });

    $('#tblUpload tbody').on('click', 'td.action-control', function (e) {
        selectedItem = null;
        let tr = $(this).closest('tr');
        let row = tbl.row(tr);
        selectedItem = row.data();

        var top = e.pageY - 10;
        var left = e.pageX - 40;

        $("#context-menu").css({
            display: "block",
            top: top,
            left: left,
            width: "30px"
            /*}).on("click a", function () {
                $("#context-menu").hide();*/
        });

        e.stopPropagation(); //To prevent menu close
    });

    $("div.dropdown-menu a").click(function () {
        $("#context-menu").hide();
        let key = $(this).attr("id");

        if (window.selectedItem != null && window.selectedItem != undefined) {
            switch (key) {
                case "mnuDownload":
                    location.href = '/Contact/DownloadFile?id=' + window.selectedItem.BulkContactUploadId;
                    break;
                case 'mnuRename':
                    alert('mnuRename is not implemented');
                    //renameItem(window.selectedItem.BulkContactUploadId);
                    break;
                case 'mnuDelete':
                    alert('mnuDelete is not implemented');
                    //renameItem(window.selectedItem.BulkContactUploadId);
                    break;
                default:
                    break;
            }
        }
    })

    $(document).on("click", function (event) {
        if ($('#context-menu').is(":visible")) {
            $('#context-menu').slideUp("fast");
        }
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


function getBulkUploadStatusData(rowCount) {
    $.ajax({
        dataType: "json",
        url: "/Contact/BulkUploadStatus?rowCount=" + rowCount,
        data: { rows: rowCount }
    }).done(function (data) {
        var grid = $("#tblUpload");
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
                            //window.location.reload();   //If full Reload page is required
                            let tbl = $('#tblUpload').DataTable();
                            tbl.ajax.reload(null, false);
                        }
                        $(this).dialog("close");
                    },
                }
            }
        })
    }
}