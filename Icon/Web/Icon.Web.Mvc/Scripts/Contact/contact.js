var selectedContact = null;

$(document).ready(function () {
    var tbl = $('#tblContact').DataTable({
        rowId: function (val) { return 'id' + val.ContactId },
        paging: false,
        ordering: true,
        info: true,
        sDom: 'RSlftip',
        processing: true,
        responsive: true,
        orderMulti: false,
        keys: false,    //Key navigation
        select: false, //Row selection
        scrollY: "50vh",
        emptyTable: "No contacts data available...",
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        infoEmpty: "",
        loadingRecords: "Loading contact data... Please wait...",
        processing: "Processing data...",

        language: {
            info: "Total Contacts: _TOTAL_",
            search: "Search Contacts ",
            lengthMenu: "Show &nbsp _MENU_ &nbsp contacts",
        },

        ajax: {
            url: "/Contact/ContactAll?hierarchyClassId=" + $('#hcId').val(),
            type: "POST",
            datatype: "json"
        },

        columnDefs: [
            {
                targets: [0],
                orderable: false,
                searchable: false,
                width: "1px"
            },

            {
                targets: [1],
                visible: false,
                orderable: false,
                searchable: false
            },
            {
                targets: [7],
                orderable: false,
                searchable: false,
                width: "5px"
            },
        ], 

        columns: [
            { class: "details-control", orderable: false, data: null, defaultContent: "" },
            { data: "ContactId", name: "ContactId", autoWidth: false },
            { data: "ContactTypeName", name: "Contact Type", autoWidth: true },
            { data: "ContactName", name: "Contact Name", autoWidth: true },
            { data: "Email", name: "Email", autoWidth: true },
            { data: "Title", name: "Title", autoWidth: true },
            { data: "PhoneNumber1", name: "PhoneNumber1", autoWidth: true },
            //{ render: function (data, type, full, meta) { return '<a class="btn btn-link" href="/Contact/Manage/?hierarchyClassId=' + full.HierarchyClassId + '&amp;ContactId=' + full.ContactId + '">Edit</a> <input type="button" class="btn btn-link text-danger" value="Delete" onclick="deleteContact(' + full.ContactId + ')"/>'; } }
            { class: "action-control text-info", orderable: false, data: null, defaultContent: '<p class="action-control fa fa-bars text-secondary"/>' },
        ]
    });

    $('#tblContact tbody').on('click', 'td.details-control', function () {
        let tr = $(this).closest('tr');
        let row = tbl.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            row.child(format(row.data()), 'child').show();
            tr.addClass('shown');
        }
    });

    $('#tblContact tbody').on('click', 'td.action-control', function (e) {
        selectedContact = null;
        let tr = $(this).closest('tr');
        let row = tbl.row(tr);
        selectedContact = row.data(); 
 
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


    function format(d) {
        return '<div class="bg-white rounded">' +
            '<table class="ml-3" cellpadding="5" cellspacing="0" border="0"> ' +
            '<tr class="text-secondary"><td id="compact-row">Address 1:</td><td>' + (d.AddressLine1 === null ? "" : d.AddressLine1) + '</td></tr>' +
            '<tr class="text-secondary"><td>Address 2:</td><td width=99%>' + (d.AddressLine2 === null ? "" : d.AddressLine2) + '</td></tr>' +
            '<tr class="text-secondary"><td>City:</td><td>' + (d.City === null ? "" : d.City) + '</td></tr>' +
            '<tr class="text-secondary"><td>State:</td><td>' + (d.State === null ? "" : d.State) + '</td></tr>' +
            '<tr class="text-secondary"><td>Zip Code:</td><td>' + (d.ZipCode === null ? "" : d.ZipCode) + '</td></tr>' +
            '<tr class="text-secondary"><td>Country:</td><td>' + (d.Country === null ? "" : d.Country) + '</td></tr>' +
            '<tr class="text-secondary"><td>Phone 2:</td><td>' + (d.PhoneNumber2 === null ? "" : d.PhoneNumber2) + '</td></tr>' +
            '<tr class="text-secondary"><td>Website URL:</td><td>' + (d.WebsiteURL === null ? "" : d.WebsiteURL) + '</td></tr>' +
            '</table></div >';
    }

    $(":submit").removeAttr("disabled");

    $('form').submit(function () {
            $(this).validate();

            if ($(this).valid()) {
                $(':submit', this).attr('disabled', true).val('Processing...');
                $('#divBlock').show();
            }
    });

    $(document).on("click", function (event) {
        if ($('#context-menu').is(":visible")) {
            $('#context-menu').slideUp("fast");
        }
    });

    $("div.dropdown-menu a").click(function () {
        $("#context-menu").hide();
        let key = $(this).attr("id");

        if(window.selectedContact != null && window.selectedContact != undefined) {
            switch (key) {
                case "mnuEdit":
                    $(location).attr('href', '/Contact/Manage/?hierarchyClassId=' + window.selectedContact.HierarchyClassId + '&contactId=' + window.selectedContact.ContactId);
                    break;
                case 'mnuDelete':
                    deleteContact(window.selectedContact.ContactId);
                    break;
                case 'mnuDetail':
                    let r = $('#tblContact').DataTable().row('#id' + window.selectedContact.ContactId);
                    if (r != null && r != undefined) {
                        //if (!r.child.isShown()){
                        $('td.details-control', r.node()).trigger('click');
                        //}
                    }
                    break;
                default:
                    btreak;
            }
        }
    })
});

function closeDialog() {
    $('#msgBlock').remove();
}

$(window).keydown(function(e) {
    if (e.keyCode == 13) {
        e.preventDefault();
        return false;
    }

    if (e.ctrlKey || e.metaKey) {
        switch (String.fromCharCode(e.which).toLowerCase()) {
            case 's':
                e.preventDefault();

                var element = $(':focus'); //Focused control
                if (element != null) $(':focus').blur(); //Remove focuse to force saving changes

                let isDisabled = $('#btnSubmit').is(':disabled');
                let isBlock = $('#msgBlock') == null ? false : $('#msgBlock').is(':visible');
                if (!isDisabled && !isBlock) {
                    $("#btnSubmit").click();
                }
                break;
        }
    }
});

function openContactType() {
    $("#dialogContactType").dialog({
        resizable: false,
        height: 'auto',
        width: 450,
        modal: true,
        closeOnEscape: false,
        show: { effect: "slide", duration: 300 },
        hide: { effect: "fade", duration: 300 },
        open: function () {
            $("#dlgMsg").text('');
            $("#contactTypeName").val('');
            $("#contactTypeName").prop('disabled', false);
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").text("Contact Type");
            $(".ui-dialog-buttonpane").show();
            $(".ui-dialog-buttonpane").children().show();
            $(".ui-dialog-titlebar").css("background-color", "gainsboro");
        },
        buttons: {
            'Create':
            {
                id: 'dlgCreate',
                text: 'Create',
                click: function () {
                    let refDialog = this; //Ref to dialog so we can close it on success
                    let isExist = false;
                    $("#dlgMsg").text('');
                    let name = $("#contactTypeName").val().trim().replace(/  +/g, ' '); // $("#contactTypeName").val().trim().toLowerCase();
                    if (name.length == 0) return;

                    $("#contactTypeName").val(name);
                    name = name.replace(/ /g, '').toLowerCase(); 
                    //Validate if ContactType exists before POST
                    $('#ddlType option').each(function () {
                        if (name == this.text.replace(/ /g, '').toLowerCase()) {
                            isExist = true;
                            return false;
                        }
                    });
                    
                    if (isExist) {
                        $("#dlgMsg").text('Contact Type already exists.');
                        return;
                    }

                    name = $("#contactTypeName").val();
                    $("#contactTypeName").prop('disabled', true);
                    $(".ui-dialog-buttonset").hide();
                    $(".ui-dialog-titlebar-close").hide();
                    $(".ui-dialog-buttonset").children().hide();
                    $(".ui-dialog-title").text("Processing Request...");

                    var request = $.ajax({
                        url: '/Contact/AddUpdateContactType',
                        type: 'POST',
                        data: "{ 'contactTypeName' : '" + name + "' }",
                        cache: false,
                        contentType: 'application/json; charset=utf-8',

                        success: function (response) {
                            /* //Keep the code below if we decide to show confirmation
                            $(".ui-dialog-titlebar").css("background-color", response.success ? "lightGreen" : "red");
                            $(".ui-dialog-title").text("System Information");
                            $(".ui-dialog-buttonset").show();
                            $('#dlgCancel').text('Close').show();
                            $("#dlgMsg").text(response.responseText);
                            */

                            var ar = [{ text: response.ContactTypeName, value: response.ContactTypeId }]; //Init array with new ContactType and add all existing ContactTypes 
                            $('#ddlType option').each(function () {
                                ar.push({ text: this.text, value: this.value });
                            });

                            $('#ddlType').empty(); //Reinitilize the list with sorted items
                            ar.sort(function (a, b) { return a.text.toLowerCase() > b.text.toLowerCase() });
                            $.each(ar, function (index, item) {
                                $('#ddlType').append($("<option />").val(item.value).text(item.text)); //Add item
                            });

                            $('#ddlType').val(response.ContactTypeId); //Select newly created ContactType
                            $(refDialog).dialog("close");
                        },
                        error:
                            function (xhr, status, error) {
                                $("#contactTypeName").prop('disabled', false);
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgCreate').show();
                                $('#dlgCancel').show();
                                $("#dlgMsg").text(JSON.parse(xhr.responseText));
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
                },
            }
        }
    });
}

function deleteContact(contactId) {
    $("#dlgContact").html('Contact: <span style="color:black; font-weight:bolder">' + $('#tblContact').DataTable().row('#id' + contactId).data().ContactName + '</span>');
    $("#dlgMsg").text('Are you sure you would like to delete contact?');

    $("#dlgConfirm").dialog({
        resizable: false,
        height: 'auto',
        width: 450,
        modal: true,
        closeOnEscape: true,
        show: { effect: "drop", duration: 300 },
        hide: { effect: "fade", duration: 300 },
        open: function () {
            $(".ui-dialog-titlebar-close").hide();
            $(".ui-dialog-title").text("User Confirmation Required");

            $(".ui-dialog-buttonpane").show();
            $(".ui-dialog-buttonpane").children().show();
            $(".ui-dialog-titlebar").css("background-color", "gainsboro");
        },
        buttons: {
            'Delete':
            {
                id: 'dlgDelete',
                text: 'Delete',
                click: function () {
                    let refDialog = this; //Ref to dialog so we can close it on success
                    $(".ui-dialog-buttonset").hide();
                    $(".ui-dialog-titlebar-close").hide();
                    $(".ui-dialog-buttonset").children().hide();
                    $(".ui-dialog-title").text("Processing Request...");
                    $("#dlgMsg").html('Deleting Contact... &nbsp<span class="spinner-border float-right ml-auto text-primary" role="status" aria-hidden="true" style="color: red" />');

                    let contactUrl = '/Contact/DeleteContact';
                    var request = $.ajax({
                        url: contactUrl,
                        type: 'POST',
                        data: "{ 'contactId' : '" + contactId + "' }",
                        cache: false,
                        contentType: 'application/json; charset=utf-8',

                        success: function (response) {
                            $(refDialog).dialog("close");
                            $('#tblContact').DataTable().row('#id' + contactId).remove().draw();
                        },
                        error:
                            function (xhr, status, error) {
                                $(".ui-dialog-titlebar").css("background-color", "red");
                                $(".ui-dialog-title").text("System Information:");
                                $(".ui-dialog-buttonset").show();
                                $('#dlgCancel').show();
                                $("#dlgMsg").html('');
                                $("#dlgMsg").text(JSON.parse(xhr.responseText));
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
                },
            }
        }
    });
}
