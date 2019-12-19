$(document).ready(function () {

    ko.applyBindings(new StoreScanViewModel());

    $('#tabs').tabs();
    blink('#zeCompareButton', -1, 500);
    
    $('#GetScans').click(function () {

        var recordCounter = 0;

        $.getJSON('/Utility/GetBadScans', function (data) {
            $.each(data, function (i, item) {
                recordCounter++;
                var create = timeConverter(item.Created_Date);
                var update = timeConverter(item.Last_Updated_Date);

                //var newLine = '<tr><td><input type="checkbox" name="' + item.Id + '" id="' + item.Id + '">' +
                //    '<label for="' + item.Id + '"></label></td><td>' + item.StoreName + '</td>' +
                //    '<td>' + item.StoreAbbreviation + '</td><td>' + create + '</td>' +
                //    '<td>' + update + '</td></tr>';
                
                var newLine = '<tr><td><input type="checkbox" name="' + item.Id + '" id="' + item.Id + '">' +
                   '</td><td>' + item.StoreName + '</td>' +
                    '<td>' + item.StoreAbbreviation + '</td><td>' + create + '</td>' +
                    '<td>' + update + '</td></tr>';

                $('#badScanTable tr:last').after(newLine);
            });
        });

        //if (recordCounter > 0) {
        //    $('#badScans').html('<h2>No bad scans found.' + recordCounter + '</h2>');
        //} else {


        //}

        $('#tabs').css('max-height', '600px').css('overflow-y', 'auto');
        $('#fixThem').show();
        $('#badScans').show();

    });

    $('#fixThem').click(function () {
        var toBeFixed = new Array();

        $('input:checked').each(function () {
            //console.log($(this).attr('id'));
            toBeFixed.push($(this).attr('id'));
        });


        $.ajax({
            type: "POST",
            url: "/Utility/FixTheseScans",
            traditional: true,
            data: { badScanIds: toBeFixed }
        }).done(function (data) {
            //console.log(data);

            //loop through array and take out those rows from the table
            for (var i = 0; i < toBeFixed.length; i++) {
                $('#' + toBeFixed[i]).parent().parent().fadeOut("slow", function () {
                    $(this).remove();

                    //hide table if there are no more rows
                    if ($('#badScanTable tr').length < 2) {
                        $('#badScans').html('<h3>NO MORE BAD SCANS</h3>');
                        $('#fixThem').hide();
                        $('#GetScans').hide();
                    }
                });
            }


        });


    });

    $('#boltSmash').click(function () {
        $(':checkbox').each(function () {
            if (!this.checked) {
                this.checked = true;
            }
        });



    });

    //$('.investigate').click(function () {
    //    //console.log($(this).val() + '   ' + $(this).attr('id'));
    //    console.log('fired');
    //});

    $(document).on('click', 'i.icon-arrow-down', function() {
        var parentId = $(this).parent().attr('id');
        console.log(parentId + ' fired');

        
        $(this).toggleClass('icon-arrow-right');
        $(this).toggleClass('icon-arrow-down');


    });

    $(document).on('click', 'i.icon-arrow-right', function () {

        var parentId = $(this).parent().attr('id');
        console.log(parentId + ' fired');

        $(this).toggleClass('icon-arrow-down');
        $(this).toggleClass('icon-arrow-right');
        
        if ($('#store-' + parentId).length > 1) {

        } else {
            var resultTable = '<tr><td colspan="3"><table id="store-' + parentId + '">' +
            '<tr><th>id</th><th>Scandate</th><th>Scans</th></tr>' +
            '</table><td><tr>';
            //This adds another line after the row
            //$(this).parent().parent().after(resultTable);
            
            //This adds it to global modal dialog
            $('#globalModal').html(resultTable).dialog({ modal: true, height: 675, width : 325, title: 'Recent Scans' });
            

            $.getJSON('/Utility/GetStoreScans', { storeId: parentId }, function (data) {
                $.each(data, function (i, item) {
                    var update = timeConverter(item.Last_Updated_Date);
                    var newLine = '<tr><td id="scan-' + item.Id + '"><i class="icon-arrow-right"></i></td>' +
                        '<td>' + update + '</td><td>' + item.ItemsScanned + '</td></tr>';
                    $('#store-' + parentId + ' tr:last').after(newLine);

                });
            });
        }

        
        
        

    });

    $('#PickYourRegion').change(function() {
        var regionId = $(this).val();
        var recordCounter = 0;

        $.getJSON('/Utility/GetStoresForRegion', { regionId: regionId }, function (data) {
            $.each(data, function (i, item) {
                recordCounter++;
                var update = timeConverter(item.ScanDate);

                var newLine = '<tr><td id="' + item.Id + '"><i class="icon-arrow-right"></i></td>' +
                    '<td>' + item.Store_Name + '</td><td>' + update + '</td></tr>';
                //var newLine = '<tr><td colspan="5">Booms?</td></tr>'
                $('#storeTable tr:last').after(newLine);
            });
        });


    });


});

function blink(elem, times, speed) {
    if (times > 0 || times < 0) {
        if ($(elem).hasClass("blink")) {
            $(elem).removeClass("blink");
            $(elem).addClass("unblink");
        } else {
            $(elem).removeClass("unblink");
            $(elem).addClass("blink");
            
        }
            
    }

    clearTimeout(function () { blink(elem, times, speed); });

    if (times > 0 || times < 0) {
        setTimeout(function () { blink(elem, times, speed); }, speed);
        times -= .5;
    }
}

function timeConverter(unix) {
    var stripped = unix.slice(6, -5);
    var a = new Date(stripped * 1000);
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var year = a.getFullYear();
    var month = months[a.getMonth()];
    var date = a.getDate();
    var hour = a.getHours();
    var min = a.getMinutes();
    var sec = a.getSeconds();

    var fullTime = ('0' + (a.getMonth() + 1)).slice(-2) + '-'
        + ('0' + date).slice(-2) + '-' + year + ' '
        + ('0' + hour).slice(-2) + ':'
        + ('0' + min).slice(-2) + ':'
        + ('0' + sec).slice(-2);
    return fullTime;
}

//KNOCKOUT STUFF

function MapStores(storeData) {
    this.StoreId = storeData.Id;
    this.StoreName = storeData.Store_Name;
    this.LastUpdate = timeConverter(storeData.ScanDate);
}

function StoreScan(scanNumbers) {
    this.ReportId = scanNumbers.Id;
    this.Scandate = timeConverter(scanNumbers.Last_Updated_Date);
    this.ItemCount = scanNumbers.ItemsScanned;
}

function CompareResult(scan) {
    this.UPC = scan.UPC;
    this.BrandName = scan.Brand_Name;
    this.LongDescription = scan.Long_Description;
    this.CompareResult = scan.CompareResult;
}

function CondenseCompare(line) {
    this.Description = line.Description;
    this.Count = line.Count;
}

function MapRegionAbbrs(regionAbbrs) {
    this.RegReg = regionAbbrs;
}

function TimeZoneInfo(tz) {
    this.RegionId = tz.RegionId;
    this.RegionAbbreviation = tz.RegionAbbreviation;
    this.RegionName = tz.RegionName;
    this.TimezoneOffset = tz.TimezoneOffset;
}


    function StoreScanViewModel() {
        //Data
        var self = this;
    
        //self.regions = ['PN', 'NC'];
        self.regions = ko.observableArray([]);
        self.region = ko.observable();
        self.store = ko.observableArray([]);
        self.selectedStore = ko.observable();
        self.selectedStoreName = ko.observable();
        self.storeScans = ko.observableArray([]);
        self.firstScan = ko.observable('0');
        self.secondScan = ko.observable('0');
        self.allThoseComparisons = ko.observableArray([]);
        self.timezoneData = ko.observableArray([]);


        
        //Initial Setup
        //var activeRegions = new Array();
        $.getJSON('/Utility/GetActiveRegions', function (data) {
            //$.each(data, function (i) {
            //    activeRegions.push(data[i]);
            //});
            var activeRegions = $.map(data, function (regionAbbrs) { return new MapRegionAbbrs(regionAbbrs); })
            self.regions(activeRegions);
        });
        //self.regions(activeRegions);

        

        //Actions
        self.goToRegion = function(region) {
            self.region(region.RegReg);
            $('#tabs').css('max-height', '600px').css('overflow-y', 'auto');
            $.getJSON('/Utility/KnockOutStoresForRegion', { region: region.RegReg }, function (data) {
                var mappedStores = $.map(data, function (storeData) { return new MapStores(storeData); });
                self.store(mappedStores);
            });
        };

        self.clearRegion = function() {
            self.region(undefined);
            self.selectedStore(undefined);
            self.firstScan('0');
            self.secondScan('0');
        };

        self.clearStore = function() {
            self.selectedStore(undefined);
            self.firstScan('0');
            self.secondScan('0');
        };

        self.getStoreScans = function(store) {
            self.selectedStore(store.StoreId);
            self.selectedStoreName(store.StoreName);
            $.getJSON('/Utility/GetStoreScans', { storeId: store.StoreId }, function(data) {
                var mappedScans = $.map(data, function (scanNumbers) { return new StoreScan(scanNumbers); });
                self.storeScans(mappedScans);
            });
        };

        self.reportSelection = function(report, event) {
            var buttonId = event.target.id;
            var reportNumber = (buttonId).split(/_/);
            if (reportNumber[0] == 'one') {
                self.firstScan(reportNumber[1]);
            } else {
                self.secondScan(reportNumber[1]);
            }
            self.allThoseComparisons(0);
        };

        self.compareReports = function() {
            //CompareScans reportOne reportTwo
            $.getJSON('/Utility/CompareScans', { reportOne: self.firstScan, reportTwo: self.secondScan }, function(data) {
                var mappedResults = $.map(data, function (scan) { return new CondenseCompare(scan); });
                self.allThoseComparisons(mappedResults);
            });
        };

        self.getTimeZoneData = function () {
            
            $.getJSON('/Utility/GetTimeZoneOffsetData', {}, function(data) {
                var tzi = $.map(data, function(tz) {
                    return new TimeZoneInfo(tz);
                });
                self.timezoneData(tzi);
            });
        };

        self.saveTimezoneInfo = function() {
            var x = ko.toJSON(this.timezoneData);
            console.log(x);

            $.ajax({
                type: "POST",
                url: "/Utility/SaveTimezoneOffsetData",
                dataType: "json",
                //traditional: true,
                data: {data: x} 
            }).done(function(data) {
                console.log(data);

                $('#timezonemessage').html(data.Message);
                
                $('#timezonemessage').show();

                if (data.Result === "Success") {
                    window.setTimeout(function () { $('#timezonemessage').fadeOut("slow"); }, 2000);
                }


            });
        };

        self.disregardReport = function (report) {
            $('#globalModal').html('<p>Delete this report?</p>');
            $('#globalModal').dialog({
                resizable: false,
                height: 240,
                modal: true,
                title: "Delete Report",
                buttons: { "Delete": function() {
                    $(this).dialog("close");
                    var response = $.post('/Utility/DeleteReport', { reportId: report.ReportId }, function(data) {
                        console.log(data);
                    });

                },
                Cancel: function() {
                    $(this).dialog("close");
                }}
            });

        };

        self.getTimeZoneData();

    }




