/* ---------------------------------------------------------------------------
// main.js - v1.0.1
// ---------------------------------------------------------------------------
// Author: Bryce Bartley (SW SWC)
// Last Modified: 7/2/2013
// 
// CHANGELOG 
//    - 7/2/2013 - Files prepped for global use
//---------------------------------------------------------------------------*/

/* ---------------------------------------------------------------------------
// Purpose: Local variables
//---------------------------------------------------------------------------*/
var potool; // App View Model
var defaultPage = "global_home";
var timeoutPollingInterval = 120;
var timeoutSeconds = 600;
var trendXHR = null;

/* ---------------------------------------------------------------------------
// Purpose: Extend knockout observable arrays to add sorting functionality
//---------------------------------------------------------------------------*/
ko.observableArray.fn.sortByPropertyAsc = function (prop) {
    this.sort(function (obj1, obj2) {
        var val1 = obj1[prop];
        var val2 = obj2[prop];
        if (typeof obj1[prop] == 'function') {
            val1 = obj1[prop]();
            val2 = obj2[prop]();
        }
        if (val1 == val2)
            return 0;
        else if (val1 < val2)
            return -1;
        else
            return 1;
    });
}

ko.observableArray.fn.sortByPropertyDesc = function (prop) {
    this.sort(function (obj1, obj2) {
        var val1 = obj1[prop];
        var val2 = obj2[prop];
        if (typeof obj1[prop] == 'function') {
            val1 = obj1[prop]();
            val2 = obj2[prop]();
        }
        if (val1 == val2)
            return 0;
        else if (val1 < val2)
            return 1;
        else
            return -1;
    });
}

/* ---------------------------------------------------------------------------
// Purpose: App initalization code
//---------------------------------------------------------------------------*/
$(document).ready(function () {
    // Setup view model
    potool = new appViewModel();
    potool.currentRegion($("input#current_region_name").val());
    potool.selectedFY($("#current_fy").val());

    // Init the model
    potool.init();

    // Initalize knockout bindings
    ko.applyBindings(potool);


    // Setup the idle timer
    $("#session_dialog").dialog({
        autoOpen: false,
        modal: true,
        width: 400,
        height: 400,
        closeOnEscape: false,
        draggable: false,
        resizable: false,
        buttons: {
            'Yes, Keep Working': function () {
                // Just close the dialog. We pass a reference to this
                // button during the init of the script, so it'll automatically
                // resume once clicked
                $(this).dialog('close');
            },
            'No, Logoff': function () {
                // fire whatever the configured onTimeout callback is.
                $.idleTimeout.options.onTimeout.call(this);
            }
        }
    });

    $.idleTimeout('#session_dialog', 'div.ui-dialog-buttonpane button:first', {
        idleAfter: timeoutSeconds, // seconds. user is considered idle after 10 minutes of no movement
        pollingInterval: timeoutPollingInterval, // a request to keepalive.php (below) will be sent to the server every minute
        keepAliveURL: 'Ajax/KeepAlive.aspx',
        AJAXTimeout: 500,
        serverResponseEquals: 'OK', // the response from keepalive.php must equal the text "OK"
        onTimeout: function () {
            // redirect the user when they timeout.
            //$("#session_dialog").dialog('close');
            window.location = "Login.aspx?A=logout";
        },
        onIdle: function () {
            // show the dialog when the user idles
            $(this).dialog("open");
        },
        onCountdown: function (counter) {
            // update the counter span inside the dialog during each second of the countdown
            $("#dialog-countdown").html(counter);
        },
        onResume: function () {
            // the dialog is closed by a button in the dialog
            // no need to do anything else
        }
    });
    ct();

    // Bind a callback that executes when document.location.hash changes.
    $(window).bind("hashchange", function (e) {
        var data = $.deparam($.param.fragment());
        //console.log(data);
        if (data == undefined || data == null || data.p == undefined || data.p == '') {
            potool.changePage({ p: defaultPage });
            return;
        }
        potool.changePage(data);
    });

    // Since the event is only triggered when the hash changes, we need
    // to trigger the event now, to handle the hash the page may have
    // loaded with.
    $(window).trigger("hashchange");

});

/* ---------------------------------------------------------------------------
// Class : appViewModel()
// Purpose: The primary class for the PO tool application view model
//---------------------------------------------------------------------------*/
function appViewModel() {
	var self = this;

	/* ---------------------------------------------------------------------------
	// Purpose: View model internal variables 
	//---------------------------------------------------------------------------*/
	self.currentPage = ko.observable("");   // Current visible page
	self.previousPage = ko.observable("");
    self.currentRegion = ko.observable("");
	self.selectedFY = ko.observable($("#fiscal_year").attr('data-id'));
	self.selectedFY.subscribe(function () {
	    // Reload data when fiscal year is changed.
	    self.loadData(self.currentPage());
	});
	self.selectedFP = ko.observable(0);
	self.selectedFW = ko.observable(0);
	self.currentDetailView = ko.observable('vendor');
	self.selectedVendor = ko.observable('');
	self.selectedStore = ko.observable('');
	self.selectedResolutionCode = ko.observable('');
	self.selectedDetailType = ko.observable('');
	self.isLoading = ko.observable(false);
	self.showDialog = ko.observable(false);
	self.dialogView = ko.observable('');
	self.dialogFP = ko.observable(1);
	self.dialogFY = ko.observable(2013);
	self.dialogFW = ko.observable(1);
	self.globalData = ko.observableArray([]);
	self.fiscalYears = ko.observableArray([2013, 2014, 2015]);
	self.fiscalPeriods = ko.observableArray([]);
	self.fiscalWeeks = ko.observableArray([]);
	self.POTrends = ko.observableArray([]);
	self.resolutionTotals = ko.observableArray([]);

	self.fiscalVendorTables = ko.observableArray();
	self.fiscalStoreTables = ko.observableArray();
	self.fiscalRCTables = ko.observableArray();
	self.isLoadingVendorTrends = ko.observable(false);
	self.isLoadingStoreTrends = ko.observable(false);
	self.isLoadingRCTrends = ko.observable(false);

	self.selectedFPLabel = ko.computed(function () {
	    return 'FY' + self.selectedFY() + " FP" + self.selectedFP();
	});

	self.selectedFWLabel = ko.computed(function () {
	    return 'FY' + self.selectedFY() + " FP" + self.selectedFP() + " FW" + self.selectedFW();
	});

	self.selectedVendorLabel = ko.computed(function () {
	    return self.selectedVendor() + " - FY" + self.selectedFY() + " FP" + self.selectedFP() + " FW" + self.selectedFW();
	});

	self.selectedStoreLabel = ko.computed(function () {
	    return self.selectedStore() + " - FY" + self.selectedFY() + " FP" + self.selectedFP() + " FW" + self.selectedFW();
	});

	self.selectedRCLabel = ko.computed(function () {
	    return self.selectedResolutionCode() + " - FY" + self.selectedFY() + " FP" + self.selectedFP() + " FW" + self.selectedFW();
	});    
 

    /* ---------------------------------------------------------------------------
    // Object : PORowPages
    // Purpose: The primary object used to control pagination via knockout 
    // bindings for the vendor and store detail pages
    //---------------------------------------------------------------------------*/
    self.PORowPages = {
        fields: ko.observableArray([]),
        pageSize: ko.observable(25),
        pageIndex: ko.observable(0),
        filterValue: ko.observable(''),
        previousPage: function () {
            var me = self.PORowPages;
            var newPage = me.pageIndex() - 1
            if (newPage < 0) return;
            me.pageIndex(newPage);
        },
        nextPage: function () {
            var me = self.PORowPages;
            var newPage = me.pageIndex() + 1
            if (newPage > me.maxPageIndex()) return;
            me.pageIndex(newPage);
        },
        firstPage: function () {
            var me = self.PORowPages;
            me.pageIndex(0);
        },
        lastPage: function () {
            var me = self.PORowPages;
            me.pageIndex(me.maxPageIndex());
        },
        loadData: function (newData) {
            var me = self.PORowPages;
            me.fields(newData);
            me.pageIndex(0);
            $("table.vendor_detail_table").find('th:first').addClass('headerSortAsc');
            me.fields.sortByPropertyAsc('PONumber');
        },
        sort: function (prop, obj) {
            var me = self.PORowPages;
            var th = $(obj);

            // Reset other columns
            th.parent().find('th').not(th).removeClass('headerSortAsc');
            th.parent().find('th').not(th).removeClass('headerSortDesc');

            if (th.hasClass('headerSortAsc')) {
                // Sort Descending
                th.removeClass('headerSortAsc').addClass('headerSortDesc');
                me.fields.sortByPropertyDesc(prop);
            } else {
                // Sort Ascending
                th.removeClass('headerSortDesc').addClass('headerSortAsc');
                me.fields.sortByPropertyAsc(prop);
            }
        }
    }

    self.PORowPages.maxPageIndex = ko.computed(function () {
        return Math.ceil(this.fields().length / this.pageSize()) - 1;
    }, self.PORowPages);

    self.PORowPages.totalPages = ko.computed(function () {
        return this.maxPageIndex() + 1;
    }, self.PORowPages);

    self.PORowPages.currentPage = ko.computed(function () {
        return this.pageIndex() + 1;
    }, self.PORowPages);

    self.PORowPages.pagedRows = ko.computed(function () {
        var size = this.pageSize();
        var start = this.pageIndex() * size;
        return this.fields.slice(start, start + size);
    }, self.PORowPages);

    /* ---------------------------------------------------------------------------
    // Function : showLoading()
    // Purpose: Centers and shows the loading dialog
    //---------------------------------------------------------------------------*/
    self.showLoading = function () {
        // Resize mask to fit document
        $(".mask").css({ height: $(document).height() + 'px' });

        // Center loading message on screen
        var w = $('.loader').width();
        var h = $('.loader').height();
        var t = $(window).scrollTop() + ($(window).height() / 2) - (h / 2);
        var l = $(window).scrollLeft() + ($(window).width() / 2) - (w / 2);
        $(".loader").css({ top: t + 'px', left: l + 'px' });

        // Setup window scroll event
        $(window).bind('scroll', function () {
            var w = $('.loader').width();
            var h = $('.loader').height();
            var t = $(window).scrollTop() + ($(window).height() / 2) - (h / 2);
            var l = $(window).scrollLeft() + ($(window).width() / 2) - (w / 2);
            $(".loader").css({ top: t + 'px', left: l + 'px' });
        });

        self.isLoading(true);
    };

    self.showBCFiscalPeriod= ko.computed(function () {
        if (self.currentPage() == 'fiscal_period') return true;
        else if (self.previousPage() == 'fiscal_period' && (self.currentPage() == 'vendor_detail' || self.currentPage() == 'store_detail' || self.currentPage() == 'rc_detail')) return true;
        else if (self.selectedDetailType() == 'fiscal_period' && (self.currentPage() == 'vendor_detail' || self.currentPage() == 'store_detail' || self.currentPage() == 'rc_detail')) return true;
        else return false;
    });

    self.showBCFiscalWeek = ko.computed(function () {
        if(self.currentPage() == 'fiscal_week') return true;
        else if (self.previousPage() == 'fiscal_week' && (self.currentPage() == 'vendor_detail' || self.currentPage() == 'store_detail' || self.currentPage() == 'rc_detail')) return true;
        else if (self.selectedDetailType() == 'fiscal_week' && (self.currentPage() == 'vendor_detail' || self.currentPage() == 'store_detail' || self.currentPage() == 'rc_detail')) return true;
        else return false;
    });

    /* ---------------------------------------------------------------------------
    // Function : changePage(string pageID, object data)
    // Purpose: Sets the active UI page based on the pageID variable passed in 
    // which is used in the visble data-bind attribute in the HTML
    //---------------------------------------------------------------------------*/

    self.changePage = function (data) {
        var pageID = data.p;
        if (pageID == undefined || pageID == '') return;

        // Clear in-progress trend pulls
        if (trendXHR != null) {
            try { trendXHR.abort(); } catch (ex) { }
            trendXHR = null;
            self.isLoadingRCTrends(false);
            self.isLoadingStoreTrends(false);
            self.isLoadingVendorTrends(false);
        }

        // Assign any provided common variables
        if (data.fp != undefined && data.fp != '' && data.fp != self.selectedFP()) self.selectedFP(data.fp);
        if (data.fy != undefined && data.fy != '' && data.fy != self.selectedFY()) self.selectedFY(data.fy);
        if (data.fw != undefined && data.fw != '' && data.fw != self.selectedFW()) self.selectedFW(data.fw);
        if (data.region != undefined && data.region != '' && data.region != self.currentRegion()) self.currentRegion(data.region);
        if (data.dt != undefined && data.dt != '') self.selectedDetailType(data.dt);
        if (pageID == self.currentPage()) {
            // No need to switch to the suggested page since we're already there!

            if (pageID == 'fiscal_week' && data.view != undefined && data.view != '' && self.currentDetailView() != data.view) {
                self.currentDetailView(data.view);
                self.changeFWDetailView();
            } else if (pageID == 'fiscal_period' && data.view != undefined && data.view != '' && self.currentDetailView() != data.view) {
                self.currentDetailView(data.view);
                self.changeFPDetailView();
            }
            return;
        }

        

        // Set previous page 
        self.previousPage(self.currentPage());

        if (pageID == "regional_home") {
            // Clear detail data which will be loaded in on-demand later
            //self.fiscalVendorPages.fields([]);
            self.fiscalVendorTables(null);
            self.fiscalRCTables(null);
            self.fiscalStoreTables(null);
            self.PORowPages.fields([]);
        }
        else if (pageID == 'fiscal_period') {  // Fiscal Period Detail Page
            if (data.view != undefined && data.view != '') self.currentDetailView(data.view);
            else self.currentDetailView('vendor');

            self.currentPage(pageID);
        }
        else if (pageID == 'fiscal_week') {  // Fiscal Week Detail Page
            if (data.view != undefined && data.view != '') self.currentDetailView(data.view);
            else self.currentDetailView('vendor');

            self.currentPage(pageID);
        }
        else if (pageID == 'vendor_detail') {
            self.selectedVendor(data.vendor);
        }
        else if (pageID == 'store_detail') {
            self.selectedStore(data.store);
        }
        else if (pageID == 'rc_detail') {
            self.selectedResolutionCode(data.rc);
        } else if (pageID == 'global_home') {
            // Do nothing
        }
        else {
            self.currentPage(pageID);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        }

        // Load the data in
        self.loadData(pageID);
    };  

    /* ---------------------------------------------------------------------------
    // Function : loadData(string type)
    // Purpose: Gets the data for a page from the server via an AJAX call. The
    // provided "type" variable determines which data set to load in.
    //---------------------------------------------------------------------------*/
    self.loadData = function (type, scope) {
        //console.log("loadData: " + type);
        switch (type) {
            case "global_home":
                self.showLoading();
                $.getJSON("ajax/GetData.aspx?a=get_global_data&year=" + self.selectedFY(), function (result) {
                    var mappedData = $.map(result.data, function (item) { return new GlobalFiscalPeriod(item); });
                    self.globalData(mappedData);

                    self.setRatings("global_year");
                    self.isLoading(false);

                    // Auto-open latest FP
                    $("table#global_main_table").find('tr.total_row:last').find('th.clickable').trigger('click');
                    self.currentPage(type);

                    self.isLoading(false);
                });
                break;
            case "regional_home":
                self.showLoading();
                $.getJSON("ajax/GetData.aspx?a=get_fiscal_periods&year=" + self.selectedFY() + "&region=" + self.currentRegion(), function (result) {
                    var mappedTMS = $.map(result.data, function (item) { return new FiscalPeriod(item); });
                    self.fiscalPeriods(mappedTMS);

                    self.setRatings("year");
                    self.isLoading(false);

                    // Auto-open latest FP
                    $("table#fp_main_table").find('tr.total_row:last').find('th.clickable').trigger('click');

                    self.currentPage(type);
                    self.isLoading(false);
                });
                break;
            case "rc_detail":
                // Need to show loading screen
                self.showLoading();

                $.ajax({
                    url: 'ajax/GetData.aspx',
                    data: {
                        A: 'get_rc_detail',
                        fy: self.selectedFY(),
                        fp: self.selectedFP(),
                        fw: self.selectedFW(),
                        rc: self.selectedResolutionCode(),
                        region: self.currentRegion(),
                        type: self.selectedDetailType()
                    },
                    dataType: 'JSON',
                    success: function (result) {
                        if (result.msg == "OK") {
                            // Map results 
                            var mappedData = $.map(result.data, function (item) { return new POData(item); });
                            self.PORowPages.loadData(mappedData);

                            self.currentPage(type);
                            self.isLoading(false);

                            gotoTop();

                        } else {
                            self.isLoading(false);
                            alert(result.msg);
                        }
                    },
                    error: function () {
                        self.currentPage('detail');
                        self.isLoading(false);
                        alert("ERROR: Unable to load Resolution Code Detail");
                    }
                });
                break;
            case "store_detail":
                // Need to show loading screen
                self.showLoading();
                self.isLoadingStoreTrends(true);
                self.POTrends([]);
                self.resolutionTotals([]);
                self.PORowPages.fields([]);

                $.ajax({
                    url: 'ajax/GetData.aspx',
                    data: { A: 'get_store_detail', fy: self.selectedFY(), fp: self.selectedFP(), fw: self.selectedFW(), store: self.selectedStore(), region: self.currentRegion(), type: self.selectedDetailType() },
                    dataType: 'JSON',
                    success: function (result) {
                        if (result.msg == "OK") {
                            // Map results 
                            var mappedData = $.map(result.data.POData, function (item) { return new POData(item); });
                            self.PORowPages.loadData(mappedData);

                            mappedData = $.map(result.data.ResolutionTotals, function (item) { return new ResolutionCodeData(item); });
                            self.resolutionTotals(mappedData);

                            self.currentPage(type);
                            self.isLoading(false);

                            gotoTop();

                            if (trendXHR != null) {
                                try { trendXHR.abort(); } catch (ex) { }
                                trendXHR = null;
                            }

                            trendXHR = $.ajax({
                                url: 'ajax/GetData.aspx',
                                data: {
                                    A: 'get_store_trends',
                                    fy: self.selectedFY(),
                                    store: self.selectedStore(),
                                    region: self.currentRegion()
                                },
                                dataType: 'JSON',
                                success: function (result) {
                                    self.isLoadingStoreTrends(false);
                                    if (result.msg == "OK") {
                                        // Map results 
                                        var mappedData = $.map(result.data.Trends, function (item) { return new TrendData(item); });
                                        self.POTrends(mappedData);

                                        // Setup Chart
                                        var maxValue = 0;
                                        for (var i in self.POTrends()) {
                                            if (self.POTrends()[i].TotalPOs > maxValue) {
                                                maxValue = self.POTrends()[i].TotalPOs;
                                            }
                                        }
                                        var unitInterval = Math.ceil(maxValue / 10);
                                        if (unitInterval < 25) unitInterval = 25;
                                        var settings = {
                                            title: " ",
                                            //description: self.selectedVendor() + " - Fiscal Year " + self.selectedFY(),
                                            enableAnimations: true,
                                            showLegend: true,
                                            padding: { left: 10, top: 5, right: 10, bottom: 5 },
                                            titlePadding: { left: 90, top: 0, right: 0, bottom: 10 },
                                            source: self.POTrends(),
                                            categoryAxis:
                                                {
                                                    dataField: "Label",
                                                    showTickMarks: true,
                                                    valuesOnTicks: false,
                                                    tickMarksInterval: 1,
                                                    tickMarksColor: "#888888",
                                                    unitInterval: 1,
                                                    gridLinesInterval: 1,
                                                    gridLinesColor: "#888888",
                                                    axisSize: "auto"
                                                },
                                            colorScheme: "scheme01",
                                            enableAnimations: true,
                                            seriesGroups:
                                                [
                                                    {
                                                        type: "column",
                                                        columnsGapPercent: 50,
                                                        valueAxis:
                                                        {
                                                            unitInterval: unitInterval,
                                                            displayValueAxis: true,
                                                            description: "PO Totals"
                                                        },
                                                        series: [
                                                                { dataField: "TotalPOs", displayText: "Total POs" },
                                                                { dataField: "SuspendedPOs", displayText: "Suspended POs" }
                                                            ]
                                                    },
                                                    {
                                                        type: "line",
                                                        showLabels: false,
                                                        symbolType: "circle",
                                                        valueAxis:
                                                        {
                                                            unitInterval: 10,
                                                            minValue: 0,
                                                            maxValue: 100,
                                                            description: "% of Total Suspended PO",
                                                            axisSize: "auto",
                                                            tickMarksColor: "#888888"
                                                        },
                                                        series: [
                                                                { dataField: "PercentOfTotal", displayText: "% Suspended" },
                                                            ]
                                                    }
                                                ]
                                        };
                                        // setup the chart
                                        $('#store_chart').jqxChart(settings);
                                    }
                                },
                                error: function () {
                                    trendXHR = null;
                                    self.isLoadingStoreTrends(false);
                                }
                            });

                        } else {
                            self.isLoading(false);
                            alert(result.msg);
                        }
                    },
                    error: function () {
                        self.isLoading(false);
                        alert("ERROR: Unable to load Store Detail");
                    }
                });
                break;
            case "vendor_detail":
                // Need to show loading screen
                self.showLoading();
                self.isLoadingVendorTrends(true);
                self.POTrends([]);
                self.resolutionTotals([]);
                self.PORowPages.fields([]);

                $.ajax({
                    url: 'ajax/GetData.aspx',
                    data: { A: 'get_vendor_detail', fy: self.selectedFY(), fp: self.selectedFP(), fw: self.selectedFW(), vendor: self.selectedVendor(), region: self.currentRegion(), type: self.selectedDetailType() },
                    dataType: 'JSON',
                    success: function (result) {
                        if (result.msg == "OK") {
                            // Map results 
                            var mappedData = $.map(result.data.POData, function (item) { return new POData(item); });
                            self.PORowPages.loadData(mappedData);

                            mappedData = $.map(result.data.ResolutionTotals, function (item) { return new ResolutionCodeData(item); });
                            self.resolutionTotals(mappedData);

                            self.currentPage(type);
                            self.isLoading(false);

                            gotoTop();

                            if (trendXHR != null) {
                                try { trendXHR.abort(); } catch (ex) { }
                                trendXHR = null;
                            }

                            trendXHR = $.ajax({
                                url: 'ajax/GetData.aspx',
                                data: { A: 'get_vendor_trends', fy: self.selectedFY(), vendor: self.selectedVendor(), region: self.currentRegion(), type: self.selectedDetailType() },
                                dataType: 'JSON',
                                success: function (result) {
                                    trendXHR = null;
                                    self.isLoadingVendorTrends(false);
                                    if (result.msg == "OK") {
                                        var mappedData = $.map(result.data.Trends, function (item) { return new TrendData(item); });
                                        self.POTrends(mappedData);

                                        // Setup Chart
                                        var maxValue = 0;
                                        for (var i in self.POTrends()) {
                                            if (self.POTrends()[i].TotalPOs > maxValue) {
                                                maxValue = self.POTrends()[i].TotalPOs;
                                            }
                                        }
                                        var unitInterval = Math.ceil(maxValue / 10);
                                        if (unitInterval < 25) unitInterval = 25;
                                        var settings = {
                                            title: " ",
                                            //description: self.selectedVendor() + " - Fiscal Year " + self.selectedFY(),
                                            enableAnimations: true,
                                            showLegend: true,
                                            padding: { left: 10, top: 5, right: 10, bottom: 5 },
                                            titlePadding: { left: 90, top: 0, right: 0, bottom: 10 },
                                            source: self.POTrends(),
                                            categoryAxis:
                                {
                                    dataField: "Label",
                                    showTickMarks: true,
                                    valuesOnTicks: false,
                                    tickMarksInterval: 1,
                                    tickMarksColor: "#888888",
                                    unitInterval: 1,
                                    gridLinesInterval: 1,
                                    gridLinesColor: "#888888",
                                    axisSize: "auto"
                                },
                                            colorScheme: "scheme01",
                                            enableAnimations: true,
                                            seriesGroups:
                                [
                                    {
                                        type: "column",
                                        columnsGapPercent: 50,
                                        valueAxis:
                                        {
                                            unitInterval: unitInterval,
                                            displayValueAxis: true,
                                            description: "PO Totals"
                                        },
                                        series: [
                                                { dataField: "TotalPOs", displayText: "Total POs" },
                                                { dataField: "SuspendedPOs", displayText: "Suspended POs" }
                                            ]
                                    },
                                    {
                                        type: "line",
                                        showLabels: false,
                                        symbolType: "circle",
                                        valueAxis:
                                        {
                                            unitInterval: 10,
                                            minValue: 0,
                                            maxValue: 100,
                                            description: "% of Total Suspended PO",
                                            axisSize: "auto",
                                            tickMarksColor: "#888888"
                                        },
                                        series: [
                                                { dataField: "PercentOfTotal", displayText: "% Suspended" },
                                            ]
                                    }
                                ]
                                        };
                                        // setup the chart
                                        $('#vendor_chart').jqxChart(settings);
                                    }
                                },
                                error: function () {
                                    trendXHR = null;
                                    self.isLoadingVendorTrends(false);
                                }
                            });

                        } else {
                            self.isLoading(false);
                            alert(result.msg);
                        }
                    },
                    error: function () {
                        self.isLoading(false);
                        alert("ERROR: Unable to load Vendor Detail");
                    }
                });
                break;
            case "fiscal_period":
                self.changeFPDetailView();
                break;
            case "fiscal_week":
                self.changeFWDetailView();
                break;
            case "regional_vendor":
                // Need to show loading screen
                self.showLoading();
                self.fiscalVendorTables([]);
                var newTable = new FiscalVendorTable(0, scope, self.currentRegion(), self.selectedFW(), self.selectedFP(), self.selectedFY());
                self.fiscalVendorTables.push(newTable);
                self.fiscalVendorTables()[0].refreshData();

                if (scope == "fiscal_period") {
                    self.currentPage('fiscal_period');
                } else {
                    self.currentPage('fiscal_week');
                }

                // Check if we need to pop additional views into place
                if (self.fiscalStoreTables() != null && self.fiscalStoreTables().length > 1) {
                    // Use Store tables as compare base, skip 0
                    for (i = 1; i < self.fiscalStoreTables().length; i++) {
                        var d = self.fiscalStoreTables()[i];
                        var newID = self.fiscalVendorTables().length;
                        var newCopmareTable = new FiscalVendorTable(newID, scope, self.currentRegion(), d.FiscalWeek(), d.FiscalPeriod(), d.FiscalYear());
                        newCopmareTable.isCompareTable(true);
                        self.fiscalVendorTables.push(newCopmareTable);

                        // Subscribe to 0 index filter
                        self.fiscalVendorTables()[0].filterValue.subscribe(function () {
                            self.fiscalVendorTables()[newID].filterValue(self.fiscalVendorTables()[0].filterValue());
                        });

                        self.fiscalVendorTables()[newID].refreshData();
                    }
                } else if (self.fiscalRCTables() != null && self.fiscalRCTables().length > 1) {
                    // Use RC Table as compare base
                    // Use Store tables as compare base, skip 0
                    for (i = 1; i < self.fiscalRCTables().length; i++) {
                        var d = self.fiscalRCTables()[i];
                        var newID = self.fiscalVendorTables().length;
                        var newCopmareTable = new FiscalVendorTable(newID, scope, self.currentRegion(), d.FiscalWeek(), d.FiscalPeriod(), d.FiscalYear());
                        newCopmareTable.isCompareTable(true);
                        self.fiscalVendorTables.push(newCopmareTable);

                        // Subscribe to 0 index filter
                        self.fiscalVendorTables()[0].filterValue.subscribe(function () {
                            self.fiscalVendorTables()[newID].filterValue(self.fiscalVendorTables()[0].filterValue());
                        });

                        self.fiscalVendorTables()[newID].refreshData();
                    }
                }

                self.isLoading(false);
                gotoTop();

                break;
            case "regional_store":
                // Need to show loading screen
                self.showLoading();
                self.fiscalStoreTables([]);
                var newTable = new FiscalStoreTable(0, scope, self.currentRegion(), self.selectedFW(), self.selectedFP(), self.selectedFY());
                self.fiscalStoreTables.push(newTable);
                self.fiscalStoreTables()[0].refreshData();

                if (scope == "fiscal_period") {
                    self.currentPage('fiscal_period');
                } else {
                    self.currentPage('fiscal_week');
                }

                // Check if we need to pop additional views into place
                if (self.fiscalVendorTables() != null && self.fiscalVendorTables().length > 1) {
                    // Use vendor tables as compare base, skip 0
                    for (i = 1; i < self.fiscalVendorTables().length; i++) {
                        var d = self.fiscalVendorTables()[i];
                        var newID = self.fiscalStoreTables().length;
                        var newCopmareTable = new FiscalStoreTable(newID, scope, self.currentRegion(), d.FiscalWeek(), d.FiscalPeriod(), d.FiscalYear());
                        newCopmareTable.isCompareTable(true);
                        self.fiscalStoreTables.push(newCopmareTable);

                        // Subscribe to 0 index filter
                        self.fiscalStoreTables()[0].filterValue.subscribe(function () {
                            self.fiscalStoreTables()[newID].filterValue(self.fiscalStoreTables()[0].filterValue());
                        });
                        self.fiscalStoreTables()[newID].refreshData();
                    }
                } else if (self.fiscalRCTables() != null && self.fiscalRCTables().length > 1) {
                    // Use RC Table as compare base
                    for (i = 1; i < self.fiscalRCTables().length; i++) {
                        var d = self.fiscalRCTables()[i];
                        var newID = self.fiscalStoreTables().length;
                        var newCopmareTable = new FiscalStoreTable(newID, scope, self.currentRegion(), d.FiscalWeek(), d.FiscalPeriod(), d.FiscalYear());
                        newCopmareTable.isCompareTable(true);
                        self.fiscalStoreTables.push(newCopmareTable);

                        // Subscribe to 0 index filter
                        self.fiscalStoreTables()[0].filterValue.subscribe(function () {
                            self.fiscalStoreTables()[newID].filterValue(self.fiscalStoreTables()[0].filterValue());
                        });
                        self.fiscalStoreTables()[newID].refreshData();
                    }
                }

                self.isLoading(false);
                gotoTop();
                break;
            case "regional_rc":
                // Need to show loading screen
                self.showLoading();
                self.fiscalRCTables([]);
                var newTable = new FiscalRCTable(0, scope, self.currentRegion(), self.selectedFW(), self.selectedFP(), self.selectedFY());
                self.fiscalRCTables.push(newTable);
                self.fiscalRCTables()[0].refreshData();

                if (scope == "fiscal_period") {
                    self.currentPage('fiscal_period');
                } else {
                    self.currentPage('fiscal_week');
                }

                // Check if we need to pop additional views into place
                if (self.fiscalVendorTables() != null && self.fiscalVendorTables().length > 1) {
                    // Use vendor tables as compare base, skip 0
                    for (i = 1; i < self.fiscalVendorTables().length; i++) {
                        var d = self.fiscalVendorTables()[i];
                        var newID = self.fiscalRCTables().length;
                        var newCopmareTable = new FiscalRCTable(newID, scope, self.currentRegion(), d.FiscalWeek(), d.FiscalPeriod(), d.FiscalYear());
                        newCopmareTable.isCompareTable(true);
                        self.fiscalRCTables.push(newCopmareTable);

                        // Subscribe to 0 index filter
                        self.fiscalRCTables()[0].filterValue.subscribe(function () {
                            self.fiscalRCTables()[newID].filterValue(self.fiscalRCTables()[0].filterValue());
                        });
                        self.fiscalRCTables()[newID].refreshData();
                    }
                } else if (self.fiscalStoreTables() != null && self.fiscalStoreTables().length > 1) {
                    // Use store Table as compare base
                    for (i = 1; i < self.fiscalStoreTables().length; i++) {
                        var d = self.fiscalStoreTables()[i];
                        var newID = self.fiscalRCTables().length;
                        var newCopmareTable = new FiscalRCTable(newID, scope, self.currentRegion(), d.FiscalWeek(), d.FiscalPeriod(), d.FiscalYear());
                        newCopmareTable.isCompareTable(true);
                        self.fiscalRCTables.push(newCopmareTable);

                        // Subscribe to 0 index filter
                        self.fiscalRCTables()[0].filterValue.subscribe(function () {
                            self.fiscalRCTables()[newID].filterValue(self.fiscalRCTables()[0].filterValue());
                        });
                        self.fiscalRCTables()[newID].refreshData();
                    }
                }

                self.isLoading(false);
                gotoTop();
                break;
            default: return;
        }
    };

    /* ---------------------------------------------------------------------------
    // Function : changeFWDetailView(string view)
    // Purpose: Changes the detail view using a provided string ID. If data is
    // required but not present for store or rc views the data will be loaded in.
    // These views are used in visble data-bind attributes in the HTML page.
    //---------------------------------------------------------------------------*/
    self.changeFWDetailView = function () {
        // Check if data is loaded already
        var view = self.currentDetailView();
        //console.log("Change FW Detail View");
        if (view == "store") {
            if (self.fiscalStoreTables() == null || self.fiscalStoreTables().length == 0 || self.fiscalStoreTables()[0].fields().length == 0) {
                self.loadData("regional_store", 'fiscal_week');
            }
        } else if (view == "rc") {
            if (self.fiscalRCTables() == null || self.fiscalRCTables().length == 0 || self.fiscalRCTables()[0].fields().length == 0) {
                self.loadData("regional_rc", 'fiscal_week');
            }
        } else if (view == "vendor") {
            if (self.fiscalVendorTables() == null || self.fiscalVendorTables().length == 0 || self.fiscalVendorTables()[0].fields().length == 0) {
                self.loadData("regional_vendor", 'fiscal_week');
            }
        } else if (self.currentPage() != 'fiscal_week') {
            self.currentPage('fiscal_week');
        } else {
            return;
        }
    };

    /* ---------------------------------------------------------------------------
    // Function : changeFPDetailView(string view)
    // Purpose: Changes the detail view using a provided string ID. If data is
    // required but not present for store or rc views the data will be loaded in.
    // These views are used in visble data-bind attributes in the HTML page.
    //---------------------------------------------------------------------------*/
    self.changeFPDetailView = function () {
        // Check if data is loaded already
        //console.log("Change FP Detail View");
        var view = self.currentDetailView();
        if (view == "store") {
            if (self.fiscalStoreTables() == null || self.fiscalStoreTables().length == 0 || self.fiscalStoreTables()[0].fields().length == 0) {
                self.loadData("regional_store", 'fiscal_period');
            }
        } else if (view == "rc") {
            if (self.fiscalRCTables() == null || self.fiscalRCTables().length == 0 || self.fiscalRCTables()[0].fields().length == 0) {
                self.loadData("regional_rc", 'fiscal_period');
            }
        } else if (view == "vendor") {
            if (self.fiscalVendorTables() == null || self.fiscalVendorTables().length == 0 || self.fiscalVendorTables()[0].fields().length == 0) {
                self.loadData("regional_vendor", 'fiscal_period');
            } else {
                self.currentPage('fiscal_period');
            }
        } else if (self.currentPage() != 'fiscal_period') {
            self.currentPage('fiscal_period');
        } else {
            return;
        }
    };

    /* ---------------------------------------------------------------------------
    // Function : openDialog()
    // Purpose: Centers the dialog and makes it visible
    //---------------------------------------------------------------------------*/
    self.openDialog = function (view) {
        self.isLoading(false);

        self.dialogView(view);

        // Resize mask to fit document
        $(".mask").css({ height: $(document).height() + 'px' });

        // Center loading message on screen
        var w = $('.dialog').width();
        var h = $('.dialog').height();
        var t = $(window).scrollTop() + ($(window).height() / 2) - (h / 2);
        var l = $(window).scrollLeft() + ($(window).width() / 2) - (w / 2);
        $(".dialog").css({ top: t + 'px', left: l + 'px' });

        // Setup window scroll event
        $(window).bind('scroll', function () {
            var w = $('.dialog').width();
            var h = $('.dialog').height();
            var t = $(window).scrollTop() + ($(window).height() / 2) - (h / 2);
            var l = $(window).scrollLeft() + ($(window).width() / 2) - (w / 2);
            $(".dialog").css({ top: t + 'px', left: l + 'px' });
        });

        self.showDialog(true);
    };

    /* ---------------------------------------------------------------------------
    // Function : setRatings(type)
    // Purpose: Sets the ratings CSS classes for the supplied type ID
    //---------------------------------------------------------------------------*/
    self.setRatings = function (type) {
        var d = [];
        var previousValue = 0;
        var minValue = null;
        var maxValue = 0;

        switch (type) {
            case "global_year":
                for (var x in self.globalData()) {
                    for (var i in self.globalData()[x].Regions()) {
                        var value = self.globalData()[x].Regions()[i].PercentOfTotal();
                        if (value > maxValue) {
                            maxValue = value;
                        }

                        if (minValue == null || value < minValue) {
                            minValue = value;
                        }
                        d.push({ parentID: x, id: i, value: value });
                    }
                }
                break;
            case "year":
                for (var x in self.fiscalPeriods()) {
                    for (var i in self.fiscalPeriods()[x].FiscalWeeks()) {
                        var value = self.fiscalPeriods()[x].FiscalWeeks()[i].PercentOfTotal();
                        if (value > maxValue) {
                            maxValue = value;
                        }

                        if (minValue == null || value < minValue) {
                            minValue = value;
                        }
                        d.push({ parentID: x, id: i, value: value });
                    }
                }
                break;            
            default:
                return;
        }

        // Sort the array
        d.sort(dynamicSort("value"));

        var increment = (maxValue - minValue) / 4;
        var bestRatio = minValue + increment;
        var goodRatio = minValue + (increment * 2);
        var mehRatio = minValue + (increment * 3);

        for (var i = 0; i < d.length; i++) {
            var css = '';
            var v = d[i].value;
            if (v >= minValue && v <= bestRatio) { css = 'week-rating-best' }
            else if (v >= bestRatio && v <= goodRatio) { css = 'week-rating-good'; }
            else if (v >= goodRatio && v <= mehRatio) { css = 'week-rating-meh'; }
            else { css = 'week-rating-bad' }

            switch (type) {
                case "global_year": self.globalData()[d[i].parentID].Regions()[d[i].id].weekRatingClass(css); break;
                case "year": self.fiscalPeriods()[d[i].parentID].FiscalWeeks()[d[i].id].weekRatingClass(css); break;
                default: break;
            }
        }
    }

    /* ---------------------------------------------------------------------------
    // Function : exportOverview()
    // Purpose: Exports the Regional FY overview to excel and shows a download link
    //---------------------------------------------------------------------------*/
    self.exportGlobalOverview = function () {
        $("a#download_link").attr('href', "#");
        self.showLoading();
        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: 'export_global_overview', fy: self.selectedFY() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    $("a#download_link").attr('href', result.data);
                    self.openDialog('download');
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to generate excel file!");
            }
        });
    }

    /* ---------------------------------------------------------------------------
    // Function : exportOverview()
    // Purpose: Exports the Regional FY overview to excel and shows a download link
    //---------------------------------------------------------------------------*/
    self.exportOverview = function () {
        $("a#download_link").attr('href', "#");
        self.showLoading();
        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: 'export_overview', fy: self.selectedFY(), region: self.currentRegion() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    $("a#download_link").attr('href', result.data);
                    self.openDialog('download');
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to load Vendor Detail");
            }
        });
    }

    /* ---------------------------------------------------------------------------
    // Function : exportFiscalWeekDetail()
    // Purpose: Exports the active fiscal week detail passing the year, period, week
    // type (vendor, store, reason code) variables.
    //---------------------------------------------------------------------------*/
    self.exportFiscalWeekDetail = function () {
        $("a#download_link").attr('href', "#");
        self.showLoading();

        var dataSets = '';
        var type = self.currentDetailView();
        switch (type) {
            case "vendor":
                for (i = 0; i < self.fiscalVendorTables().length; i++) {
                    var d = self.fiscalVendorTables()[i];
                    dataSets += ',' + d.FiscalYear() + '-' + d.FiscalPeriod() + '-' + d.FiscalWeek();
                }
                if (dataSets.length > 0 && dataSets.substr(0, 1) == ',') {
                    dataSets = dataSets.substr(1, dataSets.length - 1);
                }
                break;
            case "store":
                for (i = 0; i < self.fiscalStoreTables().length; i++) {
                    var d = self.fiscalStoreTables()[i];
                    dataSets += ',' + d.FiscalYear() + '-' + d.FiscalPeriod() + '-' + d.FiscalWeek();
                }
                if (dataSets.length > 0 && dataSets.substr(0, 1) == ',') {
                    dataSets = dataSets.substr(1, dataSets.length - 1);
                }
                break;
            case "rc":
                for (i = 0; i < self.fiscalRCTables().length; i++) {
                    var d = self.fiscalRCTables()[i];
                    dataSets += ',' + d.FiscalYear() + '-' + d.FiscalPeriod() + '-' + d.FiscalWeek();
                }
                if (dataSets.length > 0 && dataSets.substr(0, 1) == ',') {
                    dataSets = dataSets.substr(1, dataSets.length - 1);
                }
                break;
            default: return;
        }

        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: 'export_fw_detail', type: type, ds: dataSets, region: self.currentRegion() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    $("a#download_link").attr('href', result.data);
                    self.openDialog('download');
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to create FW export file due to server error");
            }
        });
    }

    /* ---------------------------------------------------------------------------
    // Function : exportFiscalPeriodDetail()
    // Purpose: Exports the active fiscal period detail passing the year, period, week
    // type (vendor, store, reason code) variables.
    //---------------------------------------------------------------------------*/
    self.exportFiscalPeriodDetail = function () {
        $("a#download_link").attr('href', "#");
        self.showLoading();

        var dataSets = '';
        var type = self.currentDetailView();
        switch (type) {
            case "vendor":
                for (i = 0; i < self.fiscalVendorTables().length; i++) {
                    var d = self.fiscalVendorTables()[i];
                    dataSets += ',' + d.FiscalYear() + '-' + d.FiscalPeriod();
                }
                if (dataSets.length > 0 && dataSets.substr(0, 1) == ',') {
                    dataSets = dataSets.substr(1, dataSets.length - 1);
                }
                break;
            case "store":
                for (i = 0; i < self.fiscalStoreTables().length; i++) {
                    var d = self.fiscalStoreTables()[i];
                    dataSets += ',' + d.FiscalYear() + '-' + d.FiscalPeriod();
                }
                if (dataSets.length > 0 && dataSets.substr(0, 1) == ',') {
                    dataSets = dataSets.substr(1, dataSets.length - 1);
                }
                break;
            case "rc":
                for (i = 0; i < self.fiscalRCTables().length; i++) {
                    var d = self.fiscalRCTables()[i];
                    dataSets += ',' + d.FiscalYear() + '-' + d.FiscalPeriod();
                }
                if (dataSets.length > 0 && dataSets.substr(0, 1) == ',') {
                    dataSets = dataSets.substr(1, dataSets.length - 1);
                }
                break;
            default: return;
        }

        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: 'export_fp_detail', type: type, ds: dataSets, region: self.currentRegion() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    $("a#download_link").attr('href', result.data);
                    self.openDialog('download');
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to create FP export file due to server error");
            }
        });
    }

    /* ---------------------------------------------------------------------------
    // Function : exportVendorDetail()
    // Purpose: Exports the current vendor detail page to excel
    //---------------------------------------------------------------------------*/
    self.exportVendorDetail = function () {
        $("a#download_link").attr('href', "#");
        self.showLoading();
        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: 'export_vendor_detail', fy: self.selectedFY(), fp: self.selectedFP(), fw: self.selectedFW(), vendor: self.selectedVendor(), region: self.currentRegion() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    $("a#download_link").attr('href', result.data);
                    self.openDialog('download');
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to load Vendor Detail");
            }
        });
    }

    /* ---------------------------------------------------------------------------
    // Function : exportStoreDetail()
    // Purpose: Exports the current store detail page to excel
    //---------------------------------------------------------------------------*/
    self.exportStoreDetail = function () {
        $("a#download_link").attr('href', "#");
        self.showLoading();
        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: 'export_store_detail', fy: self.selectedFY(), fp: self.selectedFP(), fw: self.selectedFW(), store: self.selectedStore(), region: self.currentRegion() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    $("a#download_link").attr('href', result.data);
                    self.openDialog('download');
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to load store detail");
            }
        });
    }

    /* ---------------------------------------------------------------------------
    // Function : exportRCDetail()
    // Purpose: Exports the current reason code detail page to excel
    //---------------------------------------------------------------------------*/
    self.exportRCDetail = function () {
        $("a#download_link").attr('href', "#");
        self.showLoading();
        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: 'export_rc_detail', fy: self.selectedFY(), fp: self.selectedFP(), fw: self.selectedFW(), rc: self.selectedResolutionCode(), region: self.currentRegion() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    $("a#download_link").attr('href', result.data);
                    self.openDialog('download');
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to load resolution code detail");
            }
        });
    }

    /* ---------------------------------------------------------------------------
    // Function : toggleFP(fp)
    // Purpose: Toggles the supplied fiscal period detail box open or closed.
    //---------------------------------------------------------------------------*/
    self.toggleFP = function (fp, dir) {
        // Check if detail is visible
        var tr = $("table#fp_main_table").find('tr#fp-' + fp);
        var trd = $("table#fp_main_table").find('tr#fp-detail-' + fp);
        if (tr.length == 0) return;

        if (dir == 'close') {
            // Detail is visible. Hide and show totals
            tr.show();
            trd.hide();
            tr.find('th').find('span').removeClass('icon-caret-down').addClass('icon-caret-right');

        } else {
            // Show detail and hide totals.
            tr.hide();
            trd.show();
            trd.find('th').find('span').addClass('icon-caret-down').removeClass('icon-caret-right');
        }
    }

    /* ---------------------------------------------------------------------------
    // Function : toggleGlobalFP(fp)
    // Purpose: Toggles the supplied fiscal period detail box on the global page 
    // open or closed.
    //---------------------------------------------------------------------------*/
    self.toggleGlobalFP = function (fp, dir) {
        // Check if detail is visible
        var tr = $("table#global_main_table").find('tr#fp-' + fp);
        var trd = $("table#global_main_table").find('tr#fp-detail-' + fp);
        if (tr.length == 0) return;

        if (dir == 'close') {
            // Detail is visible. Hide and show totals
            tr.show();
            trd.hide();
            tr.find('th').find('span').removeClass('icon-caret-down').addClass('icon-caret-right');

        } else {
            // Show detail and hide totals.
            tr.hide();
            trd.show();
            //tr.addClass('open');
            trd.find('th').find('span').addClass('icon-caret-down').removeClass('icon-caret-right');
        }
    }

    /* ---------------------------------------------------------------------------
    // Function : showAllFPs()
    // Purpose: Opens all the fiscal period boxes on the regional fiscal year 
    // overview
    //---------------------------------------------------------------------------*/
    self.showAllGlobalFPs = function () {
        $("table#global_main_table").find('tr.total_row').hide();
        $("table#global_main_table").find('tr.detail_row').show();
    }

    /* ---------------------------------------------------------------------------
    // Function : hideAllFPs()
    // Purpose: Closes all the fiscal period boxes on the regional fiscal year 
    // overview
    //---------------------------------------------------------------------------*/
    self.hideAllGlobalFPs = function () {
        $("table#global_main_table").find('tr.total_row').show();
        $("table#global_main_table").find('tr.detail_row').hide();
    }

    /* ---------------------------------------------------------------------------
    // Function : showAllFPs()
    // Purpose: Opens all the fiscal period boxes on the regional fiscal year 
    // overview
    //---------------------------------------------------------------------------*/
    self.showAllFPs = function () {
        $("table#fp_main_table").find('tr.total_row').hide();
        $("table#fp_main_table").find('tr.detail_row').show();
    }

    /* ---------------------------------------------------------------------------
    // Function : hideAllFPs()
    // Purpose: Closes all the fiscal period boxes on the regional fiscal year 
    // overview
    //---------------------------------------------------------------------------*/
    self.hideAllFPs = function () {
        $("table#fp_main_table").find('tr.total_row').show();
        $("table#fp_main_table").find('tr.detail_row').hide();
    }

    /* ---------------------------------------------------------------------------
    // Function : addCompare()
    // Purpose: Add a new table into view to compare periods or weeks 
    //---------------------------------------------------------------------------*/
    self.addCompare = function () {
        if (self.currentDetailView() == 'vendor') {
            if (self.fiscalVendorTables().length >= 4) {
                alert("You can only compare up to 4 data sets!");
                return;
            }

            var fp = self.dialogFP();
            var fy = self.dialogFY();
            var fw = self.dialogFW();
            var id = self.fiscalVendorTables().length;

            var newTable = new FiscalVendorTable(id, self.currentPage(), self.currentRegion(), fw, fp, fy);
            newTable.isCompareTable(true);
            self.fiscalVendorTables.push(newTable);
            self.fiscalVendorTables()[id].refreshData();

            // Subscribe to 0 index filter
            var myIndex = self.fiscalVendorTables().length - 1;
            if (self.fiscalVendorTables().length > 1) {
                self.fiscalVendorTables()[0].filterValue.subscribe(function () {
                    self.fiscalVendorTables()[myIndex].filterValue(self.fiscalVendorTables()[0].filterValue());
                });
            }

            self.showDialog(false);
        } else if (self.currentDetailView() == 'store') {
            if (self.fiscalStoreTables().length >= 4) {
                alert("You can only compare up to 4 data sets!");
                return;
            }

            var fp = self.dialogFP();
            var fy = self.dialogFY();
            var fw = self.dialogFW();
            var id = self.fiscalStoreTables().length;

            var newTable = new FiscalStoreTable(id, self.currentPage(), self.currentRegion(), fw, fp, fy);
            newTable.isCompareTable(true);
            self.fiscalStoreTables.push(newTable);
            self.fiscalStoreTables()[id].refreshData();

            // Subscribe to 0 index filter
            var myIndex = self.fiscalStoreTables().length - 1;
            if (self.fiscalStoreTables().length > 1) {
                self.fiscalStoreTables()[0].filterValue.subscribe(function () {
                    self.fiscalStoreTables()[myIndex].filterValue(self.fiscalStoreTables()[0].filterValue());
                });
            }

            self.showDialog(false);
        } else if (self.currentDetailView() == 'rc') {
            if (self.fiscalRCTables().length >= 4) {
                alert("You can only compare up to 4 data sets!");
                return;
            }

            var fp = self.dialogFP();
            var fy = self.dialogFY();
            var fw = self.dialogFW();
            var id = self.fiscalRCTables().length;

            var newTable = new FiscalRCTable(id, self.currentPage(), self.currentRegion(), fw, fp, fy);
            newTable.isCompareTable(true);
            self.fiscalRCTables.push(newTable);
            self.fiscalRCTables()[id].refreshData();

            // Subscribe to 0 index filter
            var myIndex = self.fiscalRCTables().length - 1;
            if (self.fiscalRCTables().length > 1) {
                self.fiscalRCTables()[0].filterValue.subscribe(function () {
                    self.fiscalRCTables()[myIndex].filterValue(self.fiscalRCTables()[0].filterValue());
                });
            }

            self.showDialog(false);
        }
    }

    self.removeCompare = function (table) {
        if (self.currentDetailView() == 'vendor') {
            self.fiscalVendorTables.remove(table);
        } else if (self.currentDetailView() == 'rc') {
            self.fiscalRCTables.remove(table);
        } else if (self.currentDetailView() == 'store') {
            self.fiscalStoreTables.remove(table);
        }
    }

    self.showCompareDialog = function () {
        if (self.fiscalVendorTables().length >= 4) {
            alert("You can only compare up to 4 data sets!");
            return;
        }

        self.dialogFP(self.selectedFP());
        self.dialogFW(self.selectedFW());
        self.dialogFY(self.selectedFY());
        self.openDialog('compare');
    }

    /* ---------------------------------------------------------------------------
    // Function : init()
    // Purpose: Load in initial data (Current Regional Fiscal Year View)
    //---------------------------------------------------------------------------*/
    self.init = function () {
        // Load in any inital data for the default page
        self.loadData(self.currentPage());
    }
};