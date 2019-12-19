﻿
var globalFilterData = {
    "filtered": false,
    "filterupc": "",
    "filterstatus": "",
    "filterregion": selectedRegion
};

var activePanel = "";
var menuPanels = ["#SearchPanel", "#BulkEditPanel", "#AddNewPanel"];
var defaultpageheight;

function isValidUPC(upc, region, outputElement) {
    
    if (upc != "") {
        var jsondata = '{ "upc": "' + upc + '", "region": "'+ region +'" }';
        console.log(jsondata);
        $.ajax({
            type: "POST",
            url: "/ProductStatus/IsValidUPC",
            data: jsondata,
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).success(function(results) {

            var msg = "";


            if (results.VimInfo != null) {
                msg = "<span style='font-weight: bold;'>[ " + results.VimInfo.LONG_DESCRIPTION + " ]</span>";
                if (results.ExistsInOOS) {
                    msg += " <span style='color: darkred; font-weight: bold;'>This item already has a product status assigned to it.</span>";
                }

            } else {
                msg = "<span style='color: darkred; font-weight: bold;'>This UPC [" + upc + " ] does not exist in the VIM. </span>";
            }
            $(outputElement).html(msg);
        });
    } else {
        $(outputElement).html("");
    }


}

function Menu_OnSelect(e) {

    $(".t-state-active", this).removeClass("t-state-active");
    $(e.item).addClass("t-state-active");

    
    var index = $(e.item).index();
    var panel = $(menuPanels[index]);

    for (var i = 0; i < menuPanels.length; i++) {
        var currentpanel = $(menuPanels[i]);

        if (currentpanel.is(":visible")) {
            currentpanel.slideUp(100);
        }


    }
    panel.slideDown(100);
    activePanel = menuPanels[index];

}

regionChanged = function(t) {
    var status = $("#productStatusToFind");
    var upc = $("#upctofind");
    var expiration = $("#expirationtofind");
    var grid = $("#MyGrid").data('tGrid');
    var element = $("tr.t-no-data td");
    var filtered = false;
    selectedRegion = $(t).val();

    if (status.val() != "" || upc.val() != "" || expiration.val() != "") filtered = true;

    setFilterData(filtered, upc.val(), status.val(), expiration.val(), selectedRegion, true);
    if (element.length == 0) {
        var z = $("thead.t-grid-header + tbody");
        z.prepend("<tr class='t-no-data'><td><div id='loadingPanel'><img src='/Content/images/loading.gif' alt='Loading...'/></div></td></tr>");
    }

    grid.ajaxRequest();
};

setFilterData = function(isFiltered, searchUPC, searchStatus, searchExpiration, region, refresh) {
    globalFilterData = {
        "filtered": isFiltered,
        "filterupc": searchUPC,
        "filterstatus": searchStatus,
        "filterexpiration": searchExpiration,
        "filterregion": region
    };
    console.log(globalFilterData);
    if (refresh) {
        var grid = $("#MyGrid").data('tGrid');
        // clear _data so ajax will work
        grid.dataSource._data = [];
        grid.ajaxRequest();

    }
};


executeSearch = function(u) {
    if (selectedRegion == "" || selectedRegion == "CE") {
        alert('Please select a region');
        $("#Regions").focus();
        return;
    }

    var status = $("#productStatusToFind");
    var upc = $("#upctofind");
    var expiration = $("#expirationtofind");


    if (u) {
        status.val("");
        expiration.val("");
    }
    setFilterData(true, u ? u : upc.val(), status.val(), expiration.val(), selectedRegion, true);
};

clearBulkEdit = function() {
    var status = $("#BulkEditStatus");

    var expiration = $("#BulkEditExpiration");
    status.val("");
    expiration.val("");
    status.focus();
};

clearSearchCriteria = function() {
    var status = $("#productStatusToFind");
    var upc = $("#upctofind");
    var expiration = $("#expirationtofind");

    upc.val("");
    status.val("");
    expiration.val("");
    status.focus();
};

function grid_onload(e) {
}

GridEdit = function(e) {
    // change title of command images in edit window so they make more sense.
    // must be done here because they are auto generated by the grid control
    $(".t-grid-update").attr("title", "Save");
    $(".t-grid-cancel").attr("title", "Cancel");
};

function grid_onedit(e) {
    
}

Menu_OnLoad = function(e) {
    var menu = $("#ActionsMenu").data("tMenu");
    var item = $("> li", menu.element)[2]; // index of the add new item menu. must be changed if menu moves.

    
    
    var r = $("#Regions");
    if (isCentralUser) {
        r.removeAttr('disabled');
        menu.disable(item);
    } else {
        r.val(selectedRegion);
        r.attr('disabled', 'disabled');
        r.css('cursor', 'not-allowed');

    }
};

onDataBinding = function(e) {

    var grid = $("#MyGrid").data('tGrid');

    //clear so ajax will work
    grid.dataSource._data = [];

    var element = $("tr.t-no-data td");

    if (element.length == 1) {
        $("tr.t-no-data td").html("<div id='loadingPanel'><img src='/Content/images/loading.gif' alt='Loading...'/></div>");
    }

    e.data = globalFilterData;
};


function onSave(e) {

}

function onError(e) {
    
}

function onDataBound(e) {

    if (isCentralUser)
    $(".ControlsColumn").remove();
    
    

    var g = $("tr.t-no-data td");
    if (g.length > 0) {
        g.html("No records to display");
    } else {
        adjustHeights();
    }


$("input.productStatusBulkEditSelect[type=checkbox]").change(function () {
        ToggleBulkEditMenu();
    });
    
}

ToggleBulkEditMenu = function () {
    
    var menu = $("#ActionsMenu").data("tMenu");
    var item = $("> li", menu.element)[1]; // index of the bulk edit menu. must be changed if menu moves.

    if (isCentralUser) {
        menu.disable(item);
    } else
    if ($(".productStatusBulkEditSelect:checked").length > 0) {
        menu.enable(item);
    } else {
        menu.disable(item);
    }
};

jQuery.fn.makeVisible = function () {
    return this.css('visibility', 'visible');
};
jQuery.fn.makeInvisible = function () {
    return this.css('visibility', 'hidden');
};


AddNew = function () {
    var status = $("input[name=AddNew_Status]");
    var upc = $("input[name=UPCToAdd]");
    var expiration = $("input[name=AddNew_Expiration]");

    status.removeClass("t-state-error");
    upc.removeClass("t-state-error");

    if (status.val() == "") {
        // status is required.
        $("#AddNew_Information").html("A Product Status is required");
        status.addClass("t-state-error");
        status.focus();
        return;
    }


    if (upc.val() == "") {
        // upc is required.
        $("#AddNew_Information").html("A Upc is required");
        upc.addClass("t-state-error");
        upc.focus();
        return;
    } 

    

    var combinedData = {
        "upc": upc.val(),
        "status": status.val(),
        "expiration": expiration.val(),
        "region": selectedRegion
    };


    $.ajax({
        type: "POST",
        url: "/ProductStatus/AddNew",
        data: JSON.stringify(combinedData),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).success(function(results) {
        if (results) {
            $("#AddNew_Information").html("<span style='font-weight: bold; color:green;'>Product status saved.</span> ");
            executeSearch(upc.val());

            status.val("");
            expiration.val("");
            upc.val("");

        } else {
            //
        }
    });
};

BulkDelete = function() {
    var itemids = [];
    if (confirm("Are you sure you wish to delete the selected items?")) {
        $(".productStatusBulkEditSelect:checked").each(function() {
            var $this = $(this);
            itemids.push($this.attr("value"));
        });
        var combinedData = { "Ids": itemids };

        if (itemids.length > 0) {
            $.ajax({
                type: "POST",
                url: "/ProductStatus/BulkDelete",
                data: JSON.stringify(combinedData),
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).success(function(results) {
                if (results) {
                    $("#BulkEdit_Information").html(results + " record(s) deleted. ");
                    ClearBulkEdit();
                    executeSearch();
                } else {
                    //
                }
            });
        }
    }
};

BulkSave = function() {
    var status = $("input[name=BulkEditStatus]");
    var expiration = $("input[name=BulkEditExpiration]").val();
    var itemids = [];

    $(".productStatusBulkEditSelect:checked").each(function() {
        var $this = $(this);
        itemids.push($this.attr("value"));
    });


    if (itemids.length == 0)
        return;


    var combinedData = {
        "status": status.val(),
        "expiration": expiration,
        "Ids": itemids
    };

    $.ajax({
        type: "POST",
        url: "/ProductStatus/BulkSave",
        data: JSON.stringify(combinedData),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).success(function(results) {
        if (results) {
            $("#BulkEdit_Information").html(results + " record(s) saved. ");
            ClearBulkEdit();
            executeSearch();
        } else {
            //
        }
    });
};

ClearBulkEdit = function() {
    var status = $("input[name=BulkEditStatus]");
    var expiration = $("input[name=BulkEditExpiration]");

    status.val("");
    status.removeClass("t-state-error");
    status.focus();

    expiration.val("");
    expiration.removeClass("t-state-error");
};

PageInit = function() {
    defaultpageheight = $(".page").height();
    $("input").bind("keydown", function (event) {
        // track enter key
        var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
        if (keycode == 13) { // keycode for enter key
            // force the 'Enter Key' to implicitly click the Update button     
            
            switch (activePanel) {
                case "#SearchPanel":
                    executeSearch();
                    break;
                case "#BulkEditPanel":
                    BulkSave();
                    break;
                case "#AddNewPanel":
                    AddNew();
                    break;
                default:
                    break;
            }
            return false;
        } else {
            return true;
        }
    });

    $("#bulkSelectAll").change(function () {
        var selectAll = $("#bulkSelectAll")[0].checked;
        var theCheckboxes = $(".productStatusBulkEditSelect");

        if (theCheckboxes) {
            for (var ix = 0; ix < theCheckboxes.length; ++ix)
                theCheckboxes[ix].checked = selectAll;
        }
        ToggleBulkEditMenu();
    });

    $('#formSubmitBulkEdit').submit(function () {
        var csvIds = "";
        var theCheckboxes = $(".productStatusBulkEditSelect");
        if (theCheckboxes) {
            for (var ix = 0; ix < theCheckboxes.length; ++ix) {
                if (theCheckboxes[ix].checked) {
                    if (csvIds.length > 0)
                        csvIds += ",";
                    csvIds += theCheckboxes[ix].value;
                }
            }
            $('#gridSelectedItems').val(csvIds);
        }
    });

    $('.ProductStatusBulkEditDialogShowTrigger').click(function () {
        var theBulkEdits = $('.ProductStatusBulkEditDialogShow');
        if (theBulkEdits) {
            for (var ix = 0; ix < theBulkEdits.length; ++ix) {
                $(theBulkEdits[ix]).hide();
            }
        }
        theBulkEdits = $('.ProductStatusBulkEditDialogHide');
        if (theBulkEdits) {
            for (ix = 0; ix < theBulkEdits.length; ++ix) {
                $(theBulkEdits[ix]).show('slow');
            }
        }
    });

    $('.ProductStatusBulkEditDialogHideTrigger').click(function () {
        var theBulkEdits = $('.ProductStatusBulkEditDialogHide');
        if (theBulkEdits) {
            for (var ix = 0; ix < theBulkEdits.length; ++ix) {
                $(theBulkEdits[ix]).hide();
            }
        }
        theBulkEdits = $('.ProductStatusBulkEditDialogShow');
        if (theBulkEdits) {
            for (ix = 0; ix < theBulkEdits.length; ++ix) {
                $(theBulkEdits[ix]).show('slow');
            }
        }
    });

    $('#productStatusToFind').first().focus();
    
};

$(function () {
    PageInit();
});
