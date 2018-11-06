
/* ---------------------------------------------------------------------------
// Class : GlobalFiscalPeriod(data)
// Purpose: Class definition for a Fiscal Period that mirrors the C# class.
// You can pass in a data object and will auto-populate the class.
//---------------------------------------------------------------------------*/
var GlobalFiscalPeriod = function (data) {
    var self = this;
    self.Regions = ko.observableArray();

    var mappedData = $.map(data.Regions, function (item) { return new RegionalFiscalPeriod(item); });
    self.Regions(mappedData);

    self.Period = data.Period;
    self.Year = data.Year;
    self.Label = ko.computed(function () {
        return "FP" + self.Period + " <span class=\"icon-caret-right\"></span>";
    });
    self.TotalPO = data.TotalPO;
    self.SuspendedPO = data.SuspendedPO;
    self.PercentOfTotal = ko.computed(function () {
        var p = parseFloat(self.SuspendedPO / self.TotalPO) * 100;
        p = isNaN(p) ? 0 : Math.round(p * 100) / 100;
        return p;
    });
    self.PercentOfTotalLabel = ko.computed(function () {
        return self.PercentOfTotal() + '%';
    });
    self.weekRatingClass = ko.observable('');
}

/* ---------------------------------------------------------------------------
// Class : RegionalFiscalPeriod(data)
// Purpose: Class definition for a Fiscal Period that mirrors the C# class.
// You can pass in a data object and will auto-populate the class.
//---------------------------------------------------------------------------*/
var RegionalFiscalPeriod = function (data) {
    var self = this;
    self.Period = data.Period;
    self.Year = data.Year;
    self.TotalPO = data.TotalPO;
    self.SuspendedPO = data.SuspendedPO;
    self.PercentOfTotal = ko.computed(function () {
        var p = parseFloat(self.SuspendedPO / self.TotalPO) * 100;
        p = isNaN(p) ? 0 : Math.round(p * 100) / 100;
        return p;
    });
    self.PercentOfTotalLabel = ko.computed(function () {
        return self.PercentOfTotal() + '%';
    });
    self.Region = data.Region;
    self.RegionName = data.RegionName;
    self.weekRatingClass = ko.observable('');
}

/* ---------------------------------------------------------------------------
// Class : FiscalPeriod(data)
// Purpose: Class definition for a Fiscal Period that mirrors the C# class.
// You can pass in a data object and will auto-populate the class.
//---------------------------------------------------------------------------*/
var FiscalPeriod = function (data) {
    var self = this;
    self.FiscalWeeks = ko.observableArray();
    self.Period = 0;
    self.Year = 0;
    self.TotalPO = 0;
    self.SuspendedPO = 0;
    self.Region = "";

    if (data != undefined && data != null) {
        var mappedData = $.map(data.FiscalWeeks, function (item) { return new FiscalWeek(item); });
        self.FiscalWeeks(mappedData);

        self.Period = data.Period;
        self.Year = data.Year;
        self.TotalPO = data.TotalPO;
        self.SuspendedPO = data.SuspendedPO;
        self.Region = data.Region;
    }

    self.Label = ko.computed(function () {
        return "FP" + self.Period + " <span class=\"icon-caret-right\"></span>";
    });

    self.PercentOfTotal = ko.computed(function () {
        var p = parseFloat(self.SuspendedPO / self.TotalPO) * 100;
        p = isNaN(p) ? 0 : Math.round(p * 100) / 100;
        return p;
    });
    self.PercentOfTotalLabel = ko.computed(function () {
        return self.PercentOfTotal() + '%';
    });
    self.weekRatingClass = ko.observable('');

}

/* ---------------------------------------------------------------------------
// Class : FiscalWeek(data)
// Purpose: Class definition for a Fiscal Week that mirrors the C# class.
// You can pass in a data object and will auto-populate the class.
//---------------------------------------------------------------------------*/
var FiscalWeek = function (data) {
    var self = this;

    self.Period = 0;
    self.Week = 0;
    self.Year = 0;
    self.Description = "";
    self.StartDate = "";
    self.EndDate = "";
    self.Vendor = "";
    self.TotalPO = 0;
    self.SuspendedPO = 0;
    self.Region = "";

    if (data != undefined && data != null) {
        self.Period = data.Period;
        self.Week = data.Week;
        self.Year = data.Year;
        self.Description = data.Description;
        self.StartDate = data.StartDate;
        self.EndDate = data.EndDate;
        self.Vendor = data.Vendor;
        self.TotalPO = data.TotalPO;
        self.SuspendedPO = data.SuspendedPO;
        self.Region = data.Region;
    }

    self.PercentOfTotal = ko.computed(function () {
        var p = parseFloat(self.SuspendedPO / self.TotalPO) * 100;
        p = isNaN(p) ? 0 : Math.round(p * 100) / 100;
        return p;
    });
    self.PercentOfTotalLabel = ko.computed(function () {
        return self.PercentOfTotal() + '%';
    });
    self.weekRatingClass = ko.observable('');

    self.Label = ko.computed(function () {
        return "Week " + self.Week + "<div style=\"font-size:0.8em\">" + dateFromString(self.StartDate) + ' - ' + dateFromString(self.EndDate) + "</div>";
    });
}

/* ---------------------------------------------------------------------------
// Class : VendorFiscalData(data)
// Purpose: Class definition for Vendor Fiscal Data record that mirrors the C# 
// class. You can pass in a data object and will auto-populate the class.
//---------------------------------------------------------------------------*/
var VendorFiscalData = function (data) {
    var self = this;
    self.FiscalWeek = new FiscalWeek(data.FiscalWeek);
    self.FiscalPeriod = new FiscalPeriod(data.FiscalPeriod);
    self.Vendor = data.Vendor;
    self.SuspendedContribution = data.SuspendedContribution;
    self.Label = ko.computed(function () {
        return "FP" + self.FiscalWeek.Period + "FW" + self.FiscalWeek.Week + "<div style=\"font-size:0.8em\">" + dateFromString(self.FiscalWeek.StartDate) + ' - ' + dateFromString(self.FiscalWeek.EndDate) + "</div>";
    });
    self.TotalPO = data.TotalPO;
    self.SuspendedPO = data.SuspendedPO;
    self.PercentOfTotal = ko.computed(function () {
        var p = parseFloat(self.SuspendedPO / self.TotalPO) * 100;
        p = isNaN(p) ? 0 : Math.round(p * 100) / 100;
        return p;
    });
    self.PercentOfTotalLabel = ko.computed(function () {
        return self.PercentOfTotal() + '%';
    });

    self.SuspendedContributionLabel = ko.computed(function () {
        return self.SuspendedContribution + '%';
    });
    self.weekRatingClass = ko.observable('');
    self.contributionRatingClass = ko.observable('');
    self.Region = data.Region;
}

/* ---------------------------------------------------------------------------
// Class : StoreFiscalData(data)
// Purpose: Class definition for Store Fiscal Data record that mirrors the C# 
// class. You can pass in a data object and will auto-populate the class.
//---------------------------------------------------------------------------*/
var StoreFiscalData = function (data) {
    var self = this;
    self.FiscalWeek = new FiscalWeek(data.FiscalWeek);
    self.Store = data.Store;
    self.SuspendedContribution = data.SuspendedContribution;
    self.Label = ko.computed(function () {
        return "FP" + self.FiscalWeek.Period + "FW" + self.FiscalWeek.Week + "<div style=\"font-size:0.8em\">" + dateFromString(self.FiscalWeek.StartDate) + ' - ' + dateFromString(self.FiscalWeek.EndDate) + "</div>";
    });
    self.TotalPO = data.TotalPO;
    self.SuspendedPO = data.SuspendedPO;
    self.PercentOfTotal = ko.computed(function () {
        var p = parseFloat(self.SuspendedPO / self.TotalPO) * 100;
        p = isNaN(p) ? 0 : Math.round(p * 100) / 100;
        return p;
    });
    self.PercentOfTotalLabel = ko.computed(function () {
        return self.PercentOfTotal() + '%';
    });

    self.SuspendedContributionLabel = ko.computed(function () {
        return self.SuspendedContribution + '%';
    });
    self.weekRatingClass = ko.observable('');
    self.contributionRatingClass = ko.observable('');
    self.Region = data.Region;
}

/* ---------------------------------------------------------------------------
// Class : RCFiscalData(data)
// Purpose: Class definition for Resolution Code Fiscal Data record that 
// mirrors the C#  class. You can pass in a data object and will auto-populate 
// the class.
//---------------------------------------------------------------------------*/
var RCFiscalData = function (data) {
    var self = this;
    self.FiscalWeek = new FiscalWeek(data.FiscalWeek);
    self.ResolutionCode = data.ResolutionCode;
    self.SuspendedContribution = data.SuspendedContribution;
    self.Label = ko.computed(function () {
        return "FP" + self.FiscalWeek.Period + "FW" + self.FiscalWeek.Week + "<div style=\"font-size:0.8em\">" + dateFromString(self.FiscalWeek.StartDate) + ' - ' + dateFromString(self.FiscalWeek.EndDate) + "</div>";
    });
    self.TotalPO = data.TotalPO;
    self.SuspendedPO = data.SuspendedPO;
    self.PercentOfTotal = ko.computed(function () {
        var p = parseFloat(self.SuspendedPO / self.TotalPO) * 100;
        p = isNaN(p) ? 0 : Math.round(p * 100) / 100;
        return p;
    });
    self.PercentOfTotalLabel = ko.computed(function () {
        return self.PercentOfTotal() + '%';
    });

    self.SuspendedContributionLabel = ko.computed(function () {
        return self.SuspendedContribution + '%';
    });
    self.weekRatingClass = ko.observable('');
    self.contributionRatingClass = ko.observable('');
    self.Region = data.Region;
}

/* ---------------------------------------------------------------------------
// Class : PACFiscalData(data)
// Purpose: Class definition for PAC Fiscal Data record that mirrors the C# 
// class. You can pass in a data object and will auto-populate the class.
//---------------------------------------------------------------------------*/
var PACFiscalData = function (data) {
    var self = this;
    self.FiscalWeek = new FiscalWeek(data.FiscalWeek);
    self.Vendor = data.Vendor;
    self.SuspendedPO = data.SuspendedPO;
    self.SuspendedLineItems = data.SuspendedLineItems;
    self.Label = ko.computed(function () {
        return "FP" + self.FiscalWeek.Period + "FW" + self.FiscalWeek.Week + "<div style=\"font-size:0.8em\">" + dateFromString(self.FiscalWeek.StartDate) + ' - ' + dateFromString(self.FiscalWeek.EndDate) + "</div>";
    });

    self.SuspendedItemsPerPO = ko.computed(function () {
        var p = 0;
        try { p = parseFloat(self.SuspendedLineItems / self.SuspendedPO); }
        catch (ex) { }
        p = isNaN(p) ? 0 : p;
        return p;
    });

    self.weekRatingClass = ko.observable('');
    self.contributionRatingClass = ko.observable('');
    self.Region = data.Region;
}


/* ---------------------------------------------------------------------------
// Class : POData(data)
// Purpose: Class definition for Purchase Order Data record that mirrors the
// C#  class. You can pass in a data object and will auto-populate the class.
//---------------------------------------------------------------------------*/
var POData = function (data) {
    var self = this;
    self.PONumber = data.PONumber;
    self.Suspended = data.Suspended;
    self.CloseDate = data.CloseDate;
    self.ResolutionCode = data.ResolutionCode;
    self.AdminNotes = data.AdminNotes;
    self.Vendor = data.Vendor;
    self.Subteam = data.Subteam;
    self.Store = data.Store;
    self.AdjustedCost = data.AdjustedCost;
    self.CreditPO = data.CreditPO;
    self.VendorType = data.VendorType;
    self.POCreator = data.POCreator;
    self.EInvoiceMatchedToPO = data.EInvoiceMatchedToPO;
    self.PONotes = data.PONotes;
    self.ClosedBy = data.ClosedBy;
    self.Region = data.Region;
    self.FormattedCloseDate = ko.computed(function () {
        return dateFromStringWithTime(self.CloseDate);
    });
}

/* ---------------------------------------------------------------------------
// Class : TrendData(data)
// Purpose: Class definition for Trending Data record that mirrors the
// C#  class. This is used for the charting plugin.
//---------------------------------------------------------------------------*/
var TrendData = function (data) {
    var self = this;
    self.Label = data.Label;
    self.Period = data.Period;
    self.TotalPOs = data.TotalPOs;
    self.SuspendedPOs = data.SuspendedPOs;
    self.PercentOfTotal = data.PercentOfTotal;
    self.Region = data.Region;
}

/* ---------------------------------------------------------------------------
// Class : ResolutionCodeData(data)
// Purpose: Class definition for Resolution Code Data record that mirrors the
// C#  class. This is used for the side bar on the detail screens.
//---------------------------------------------------------------------------*/
var ResolutionCodeData = function (data) {
    var self = this;
    self.Name = data.Name;
    self.Total = data.Total;
}


/* ---------------------------------------------------------------------------
// Class : FiscalVendorTable
// Purpose: The primary object used to control pagination via knockout 
//---------------------------------------------------------------------------*/
function FiscalVendorTable(id, scope, region, fw, fp, fy) {
    var self = this;
    self.ID = ko.observable();
    self.Region = ko.observable(region);
    self.Scope = ko.observable(scope);
    self.FiscalPeriod = ko.observable(fp);
    self.FiscalYear = ko.observable(fy);
    self.FiscalWeek = ko.observable(fw);
    self.fields = ko.observableArray([]);
    self.pageSize = ko.observable(25);
    self.pageIndex = ko.observable(0);
    self.filterValue = ko.observable('');
    self.isLoading = ko.observable(false);
    self.isCompareTable = ko.observable(false);

    self.previousPage = function () {
        var newPage = self.pageIndex() - 1
        if (newPage < 0) return;
        self.pageIndex(newPage);
    };

    self.nextPage = function () {
        var newPage = self.pageIndex() + 1
        if (newPage > self.maxPageIndex()) return;
        self.pageIndex(newPage);
    };

    self.firstPage = function () {
        self.pageIndex(0);
    };

    self.lastPage = function () {
        self.pageIndex(self.maxPageIndex());
    };

    self.refreshData = function () {
        var action = 'get_fw_detail';
        if (self.Scope() == 'fiscal_period') {
            action = 'get_fp_detail';
        }

        self.isLoading(true);
        // Ajax call to get data for the page
        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: action, type: 'regional_vendor', fy: self.FiscalYear(), fp: self.FiscalPeriod(), fw: self.FiscalWeek(), region: self.Region() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    // Map results 
                    var mappedData = $.map(result.data.VendorRows, function (item) { return new VendorFiscalData(item); });
                    self.fields(mappedData);
                    self.pageIndex(0);
                    $("table#fiscal-vendor-table-" + self.ID()).find('th:first').addClass('headerSortAsc');
                    self.fields.sortByPropertyAsc('Vendor');
                    self.setRatings();

                    self.isLoading(false);
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to load Fiscal Detail");
            }
        });
    };

    self.sort = function (prop, obj) {
        var th = $(obj);

        // Reset other columns
        th.parent().find('th').not(th).removeClass('headerSortAsc');
        th.parent().find('th').not(th).removeClass('headerSortDesc');

        if (th.hasClass('headerSortAsc')) {
            // Sort Descending
            th.removeClass('headerSortAsc').addClass('headerSortDesc');
            self.fields.sortByPropertyDesc(prop);
        } else {
            // Sort Ascending
            th.removeClass('headerSortDesc').addClass('headerSortAsc');
            self.fields.sortByPropertyAsc(prop);
        }
    };

    self.filteredRows = ko.computed(function () {
        if (typeof self.filterValue != 'function' || self.filterValue().length < 3) return [];
        var searchTerm = self.filterValue().toLowerCase();
        return $.grep(self.fields(), function (n, i) {
            if (n.Vendor.toLowerCase().indexOf(searchTerm) !== -1) return n;
        });
    });

    self.maxPageIndex = ko.computed(function () {
        if (self.filteredRows().length > 0) {
            // Using a filter
            return Math.ceil(self.filteredRows().length / self.pageSize()) - 1;
        } else {
            return Math.ceil(self.fields().length / self.pageSize()) - 1;
        }
    });

    self.totalPages = ko.computed(function () {
        return self.maxPageIndex() + 1;
    });

    self.currentPage = ko.computed(function () {
        return self.pageIndex() + 1;
    });

    self.pagedRows = ko.computed(function () {
        var size = self.pageSize();
        var start = self.pageIndex() * size;

        if (self.filteredRows().length > 0) {
            // Using a filter
            // Check to make sure we've not exceeding the new max page size
            if (self.pageIndex() > self.maxPageIndex()) self.pageIndex(0); start = self.pageIndex() * size;
            return self.filteredRows().slice(start, start + size);
        } else {
            return self.fields.slice(start, start + size);
        }
    });

    self.setRatings = function () {
        var d = [];
        var previousValue = 0;
        var minValue = null;
        var maxValue = 0;

        // Percent of Total Suspended
        for (var i in self.fields()) {
            var value = self.fields()[i].PercentOfTotal();
            if (value > maxValue) {
                maxValue = value;
            }
            if (minValue == null || value < minValue) {
                minValue = value;
            }
            d.push({ id: i, value: value });
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

            self.fields()[d[i].id].weekRatingClass(css);
        }

        // Suspended Contribution
        d = [];
        previousValue = 0;
        minValue = null;
        maxValue = 0;

        for (var i in self.fields()) {
            var value = self.fields()[i].SuspendedContribution;
            if (value > maxValue) {
                maxValue = value;
            }
            if (minValue == null || value < minValue) {
                minValue = value;
            }
            d.push({ id: i, value: value });
        }

        // Sort the array
        d.sort(dynamicSort("value"));

        increment = (maxValue - minValue) / 4;
        bestRatio = minValue + increment;
        goodRatio = minValue + (increment * 2);
        mehRatio = minValue + (increment * 3);
        for (var i = 0; i < d.length; i++) {
            var css = '';
            var v = d[i].value;
            if (v >= minValue && v <= bestRatio) { css = 'week-rating-best' }
            else if (v >= bestRatio && v <= goodRatio) { css = 'week-rating-good'; }
            else if (v >= goodRatio && v <= mehRatio) { css = 'week-rating-meh'; }
            else { css = 'week-rating-bad' }

            self.fields()[d[i].id].contributionRatingClass(css);
        }
    }
}

/* ---------------------------------------------------------------------------
// Class : FiscalStoreTable
// Purpose: The primary object used to control pagination via knockout 
//---------------------------------------------------------------------------*/
function FiscalStoreTable(id, scope, region, fw, fp, fy) {
    var self = this;
    self.ID = ko.observable();
    self.Region = ko.observable(region);
    self.Scope = ko.observable(scope);
    self.FiscalPeriod = ko.observable(fp);
    self.FiscalYear = ko.observable(fy);
    self.FiscalWeek = ko.observable(fw);
    self.fields = ko.observableArray([]);
    self.pageSize = ko.observable(25);
    self.pageIndex = ko.observable(0);
    self.filterValue = ko.observable('');
    self.isLoading = ko.observable(false);
    self.isCompareTable = ko.observable(false);

    self.previousPage = function () {
        var newPage = self.pageIndex() - 1
        if (newPage < 0) return;
        self.pageIndex(newPage);
    };

    self.nextPage = function () {
        var newPage = self.pageIndex() + 1
        if (newPage > self.maxPageIndex()) return;
        self.pageIndex(newPage);
    };

    self.firstPage = function () {
        self.pageIndex(0);
    };

    self.lastPage = function () {
        self.pageIndex(self.maxPageIndex());
    };

    self.refreshData = function () {
        var action = 'get_fw_detail';
        if (self.Scope() == 'fiscal_period') {
            action = 'get_fp_detail';
        }

        self.isLoading(true);
        // Ajax call to get data for the page
        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: action, type: 'regional_store', fy: self.FiscalYear(), fp: self.FiscalPeriod(), fw: self.FiscalWeek(), region: self.Region() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    // Map results 
                    var mappedData = $.map(result.data.StoreRows, function (item) { return new StoreFiscalData(item); });
                    self.fields(mappedData);
                    self.pageIndex(0);
                    $("table#fiscal-store-table-" + self.ID()).find('th:first').addClass('headerSortAsc');
                    self.fields.sortByPropertyAsc('Store');
                    self.setRatings();

                    self.isLoading(false);
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to load Fiscal Detail");
            }
        });
    };

    self.sort = function (prop, obj) {
        var th = $(obj);

        // Reset other columns
        th.parent().find('th').not(th).removeClass('headerSortAsc');
        th.parent().find('th').not(th).removeClass('headerSortDesc');

        if (th.hasClass('headerSortAsc')) {
            // Sort Descending
            th.removeClass('headerSortAsc').addClass('headerSortDesc');
            self.fields.sortByPropertyDesc(prop);
        } else {
            // Sort Ascending
            th.removeClass('headerSortDesc').addClass('headerSortAsc');
            self.fields.sortByPropertyAsc(prop);
        }
    };

    self.filteredRows = ko.computed(function () {
        if (typeof self.filterValue != 'function' || self.filterValue().length < 3) return [];
        var searchTerm = self.filterValue().toLowerCase();
        return $.grep(self.fields(), function (n, i) {
            if (n.Store.toLowerCase().indexOf(searchTerm) !== -1) return n;
        });
    });

    self.maxPageIndex = ko.computed(function () {
        if (self.filteredRows().length > 0) {
            // Using a filter
            return Math.ceil(self.filteredRows().length / self.pageSize()) - 1;
        } else {
            return Math.ceil(self.fields().length / self.pageSize()) - 1;
        }
    });

    self.totalPages = ko.computed(function () {
        return self.maxPageIndex() + 1;
    });

    self.currentPage = ko.computed(function () {
        return self.pageIndex() + 1;
    });

    self.pagedRows = ko.computed(function () {
        var size = self.pageSize();
        var start = self.pageIndex() * size;

        if (self.filteredRows().length > 0) {
            // Using a filter
            // Check to make sure we've not exceeding the new max page size
            if (self.pageIndex() > self.maxPageIndex()) self.pageIndex(0); start = self.pageIndex() * size;
            return self.filteredRows().slice(start, start + size);
        } else {
            return self.fields.slice(start, start + size);
        }
    });

    self.setRatings = function () {
        var d = [];
        var previousValue = 0;
        var minValue = null;
        var maxValue = 0;

        // Percent of Total Suspended
        for (var i in self.fields()) {
            var value = self.fields()[i].PercentOfTotal();
            if (value > maxValue) {
                maxValue = value;
            }
            if (minValue == null || value < minValue) {
                minValue = value;
            }
            d.push({ id: i, value: value });
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

            self.fields()[d[i].id].weekRatingClass(css);
        }

        // Suspended Contribution
        d = [];
        previousValue = 0;
        minValue = null;
        maxValue = 0;

        for (var i in self.fields()) {
            var value = self.fields()[i].SuspendedContribution;
            if (value > maxValue) {
                maxValue = value;
            }
            if (minValue == null || value < minValue) {
                minValue = value;
            }
            d.push({ id: i, value: value });
        }

        // Sort the array
        d.sort(dynamicSort("value"));

        increment = (maxValue - minValue) / 4;
        bestRatio = minValue + increment;
        goodRatio = minValue + (increment * 2);
        mehRatio = minValue + (increment * 3);
        for (var i = 0; i < d.length; i++) {
            var css = '';
            var v = d[i].value;
            if (v >= minValue && v <= bestRatio) { css = 'week-rating-best' }
            else if (v >= bestRatio && v <= goodRatio) { css = 'week-rating-good'; }
            else if (v >= goodRatio && v <= mehRatio) { css = 'week-rating-meh'; }
            else { css = 'week-rating-bad' }

            self.fields()[d[i].id].contributionRatingClass(css);
        }
    }
}

/* ---------------------------------------------------------------------------
// Class : FiscalRCTable
// Purpose: The primary object used to control pagination via knockout 
//---------------------------------------------------------------------------*/
function FiscalRCTable(id, scope, region, fw, fp, fy) {
    var self = this;
    self.ID = ko.observable();
    self.Region = ko.observable(region);
    self.Scope = ko.observable(scope);
    self.FiscalPeriod = ko.observable(fp);
    self.FiscalYear = ko.observable(fy);
    self.FiscalWeek = ko.observable(fw);
    self.fields = ko.observableArray([]);
    self.pageSize = ko.observable(25);
    self.pageIndex = ko.observable(0);
    self.filterValue = ko.observable('');
    self.isLoading = ko.observable(false);
    self.isCompareTable = ko.observable(false);

    self.previousPage = function () {
        var newPage = self.pageIndex() - 1
        if (newPage < 0) return;
        self.pageIndex(newPage);
    };

    self.nextPage = function () {
        var newPage = self.pageIndex() + 1
        if (newPage > self.maxPageIndex()) return;
        self.pageIndex(newPage);
    };

    self.firstPage = function () {
        self.pageIndex(0);
    };

    self.lastPage = function () {
        self.pageIndex(self.maxPageIndex());
    };

    self.refreshData = function () {
        var action = 'get_fw_detail';
        if (self.Scope() == 'fiscal_period') {
            action = 'get_fp_detail';
        }

        self.isLoading(true);
        // Ajax call to get data for the page
        $.ajax({
            url: 'ajax/GetData.aspx',
            data: { A: action, type: 'regional_rc', fy: self.FiscalYear(), fp: self.FiscalPeriod(), fw: self.FiscalWeek(), region: self.Region() },
            dataType: 'JSON',
            success: function (result) {
                if (result.msg == "OK") {
                    // Map results 
                    var mappedData = $.map(result.data.ResolutionRows, function (item) { return new RCFiscalData(item); });
                    self.fields(mappedData);
                    self.pageIndex(0);
                    $("table#fiscal-rc-table-" + self.ID()).find('th:first').addClass('headerSortAsc');
                    self.fields.sortByPropertyAsc('ResolutionCode');
                    self.setRatings();

                    self.isLoading(false);
                } else {
                    self.isLoading(false);
                    alert(result.msg);
                }
            },
            error: function () {
                self.isLoading(false);
                alert("ERROR: Unable to load Fiscal Detail");
            }
        });
    };

    self.sort = function (prop, obj) {
        var th = $(obj);

        // Reset other columns
        th.parent().find('th').not(th).removeClass('headerSortAsc');
        th.parent().find('th').not(th).removeClass('headerSortDesc');

        if (th.hasClass('headerSortAsc')) {
            // Sort Descending
            th.removeClass('headerSortAsc').addClass('headerSortDesc');
            self.fields.sortByPropertyDesc(prop);
        } else {
            // Sort Ascending
            th.removeClass('headerSortDesc').addClass('headerSortAsc');
            self.fields.sortByPropertyAsc(prop);
        }
    };

    self.filteredRows = ko.computed(function () {
        if (typeof self.filterValue != 'function' || self.filterValue().length < 3) return [];
        var searchTerm = self.filterValue().toLowerCase();
        return $.grep(self.fields(), function (n, i) {
            if (n.ResolutionCode.toLowerCase().indexOf(searchTerm) !== -1) return n;
        });
    });

    self.maxPageIndex = ko.computed(function () {
        if (self.filteredRows().length > 0) {
            // Using a filter
            return Math.ceil(self.filteredRows().length / self.pageSize()) - 1;
        } else {
            return Math.ceil(self.fields().length / self.pageSize()) - 1;
        }
    });

    self.totalPages = ko.computed(function () {
        return self.maxPageIndex() + 1;
    });

    self.currentPage = ko.computed(function () {
        return self.pageIndex() + 1;
    });

    self.pagedRows = ko.computed(function () {
        var size = self.pageSize();
        var start = self.pageIndex() * size;

        if (self.filteredRows().length > 0) {
            // Using a filter
            // Check to make sure we've not exceeding the new max page size
            if (self.pageIndex() > self.maxPageIndex()) self.pageIndex(0); start = self.pageIndex() * size;
            return self.filteredRows().slice(start, start + size);
        } else {
            return self.fields.slice(start, start + size);
        }
    });

    self.setRatings = function () {
        var d = [];
        var previousValue = 0;
        var minValue = null;
        var maxValue = 0;

        // Percent of Total Suspended
        for (var i in self.fields()) {
            var value = self.fields()[i].PercentOfTotal();
            if (value > maxValue) {
                maxValue = value;
            }
            if (minValue == null || value < minValue) {
                minValue = value;
            }
            d.push({ id: i, value: value });
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

            self.fields()[d[i].id].weekRatingClass(css);
        }

        // Suspended Contribution
        d = [];
        previousValue = 0;
        minValue = null;
        maxValue = 0;

        for (var i in self.fields()) {
            var value = self.fields()[i].SuspendedContribution;
            if (value > maxValue) {
                maxValue = value;
            }
            if (minValue == null || value < minValue) {
                minValue = value;
            }
            d.push({ id: i, value: value });
        }

        // Sort the array
        d.sort(dynamicSort("value"));

        increment = (maxValue - minValue) / 4;
        bestRatio = minValue + increment;
        goodRatio = minValue + (increment * 2);
        mehRatio = minValue + (increment * 3);
        for (var i = 0; i < d.length; i++) {
            var css = '';
            var v = d[i].value;
            if (v >= minValue && v <= bestRatio) { css = 'week-rating-best' }
            else if (v >= bestRatio && v <= goodRatio) { css = 'week-rating-good'; }
            else if (v >= goodRatio && v <= mehRatio) { css = 'week-rating-meh'; }
            else { css = 'week-rating-bad' }

            self.fields()[d[i].id].contributionRatingClass(css);
        }
    }
}