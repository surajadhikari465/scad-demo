<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>
<!DOCTYPE html>
    <html class="no-js" lang="en">
    <head>
        <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
        <meta charset="utf-8">
        <title>WFM PO Reports</title>
        <meta name="description" content="">
        <meta name="viewport" content="width=device-width">

        <link rel="stylesheet" href="css/normalize.css" />
        <link rel="stylesheet" href="js/vendor/jqwidgets/styles/jqx.base.css" />
        <link rel="stylesheet" href="css/Elderkin/Elderkin.css" />
        <link href="css/fontawesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
        <link href="css/south-street/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
        <link href="css/custom-theme/jquery-ui-1.10.0.custom.css" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" href="css/main.css?v=1.0.3" />
        <!-- The code below will allow us to target an ie stylesheet in IE 10 -->
          <script>
              if (/*@cc_on!@*/false) {
                  var headHTML = document.getElementsByTagName('head')[0].innerHTML;
                  headHTML += '<link type="text/css" rel="stylesheet" href="css/ie.css">';
                  document.getElementsByTagName('head')[0].innerHTML = headHTML;
              } 
        </script>        
        <script src="js/vendor/modernizr-2.6.2.min.js"></script>   
    </head>
    <body>
        <!--[if lt IE 7]>
            <link rel="stylesheet" type="text/css" href="css/fontawesome/css/font-awesome-ie7.min.css" />
            <link rel="stylesheet" type="text/css" href="css/custom-theme/jquery.ui.1.10.0.ie.css" />
            <p class="chromeframe">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> or <a href="http://www.google.com/chromeframe/?redirect=true">activate Google Chrome Frame</a> to improve your experience.</p>
        <![endif]-->
       
        <input type="hidden" id="current_fy" value="<%=CurrentFiscalYear %>" />
        <input type="hidden" id="current_region" value="<%=CurrentRegion %>" />
        <input type="hidden" id="current_region_name" value="<%=CurrentRegionName %>" />

        <!-- HEADER -->
        <div class="top_content">
            <!-- HEADER: LOGO -->
            <div class="page_header">
                <a href="#p="><img src="img/po_reports_logo_7.png" alt="PO Reports" style="display:block; margin:0 auto;"/></a>
            </div>
            <!-- HEADER: TOOLBAR -->
            <div class="toolbar">
                <div class="tb_string"><%=USER_STRING %> <a href="Login.aspx?A=logout"><span class="icon-lock"></span> Logout</a></div>

                <!-- TOOLBAR: GLOBAL BUTTONS -->
                <span data-bind="visible: currentPage() == 'global_home'">
                    <span style="display:inline-block; margin:0 0.5em;">
                        <label style="font-size:0.9em"><span class="icon-calendar"></span> Fiscal Year </label>
                        <select id="fiscal_year" data-bind="value:selectedFY" data-id="<%=START_YEAR.ToString() %>">
                            <%=YEAR_OPTIONS %>
                        </select>
                    </span>
                    <a href="#" data-bind="click: function(){$root.exportGlobalOverview();}"><span class="icon-file"></span> Export</a>
                </span>

                <!-- TOOLBAR: REGIONAL BUTTONS -->
                <span data-bind="visible: currentPage() == 'regional_home'" style="display:none;">
                    <a href="#p=global_home" tilte="Back to Global Overview"><span class="icon-arrow-left"></span> Go Back</a>
                    <span style="display:inline-block; margin:0 0.5em;">
                        <label style="font-size:0.9em"><span class="icon-calendar"></span> Fiscal Year </label>
                        <select data-bind="value:selectedFY">
                            <%=YEAR_OPTIONS %>
                        </select>
                    </span>
                    <a href="#" data-bind="click: function(){$root.exportOverview();}"><span class="icon-file"></span> Export</a>
                </span>

                <!-- TOOLBAR: FISCAL PERIOD BUTTONS -->
                <span data-bind="visible: currentPage() == 'fiscal_period'" style="display:none;">
                    <a data-bind="attr:{href:'#p=regional_home&region=' + currentRegion(), title:'Back to ' + currentRegion() + ' Region'}"><span class="icon-arrow-left"></span> Go Back</a>
                    <a href="#" data-bind="click: function(){$root.exportFiscalPeriodDetail();}"><span class="icon-file"></span> Export</a>
                </span>

                <!-- TOOLBAR: FISCAL WEEK BUTTONS -->
                <span data-bind="visible: currentPage() == 'fiscal_week'" style="display:none;">
                    <a data-bind="attr:{href:'#p=regional_home&region=' + currentRegion(), title:'Back to ' + currentRegion() + ' Region'}"><span class="icon-arrow-left"></span> Go Back</a>
                    <a href="#" data-bind="click: function(){$root.exportFiscalWeekDetail();}"><span class="icon-file"></span> Export</a>
                </span>

                <!-- TOOLBAR: VENDOR DETAIL BUTTONS -->
                <span data-bind="visible: currentPage() == 'vendor_detail'" style="display:none;">
                    <a data-bind="attr:{href:'#p='+$root.previousPage()+'&region='+currentRegion()+'&fy='+$root.selectedFY()+'&fp='+selectedFP()+'&fw='+selectedFW()+'&view=vendor'}" title="Go Back"><span class="icon-arrow-left"></span> Go Back</a>
                    <a href="#" data-bind="click: function(){$root.exportVendorDetail();}"><span class="icon-file"></span> Export</a>
                </span>

                <!-- TOOLBAR: STORE DETAIL BUTTONS -->
                <span data-bind="visible: currentPage() == 'store_detail'" style="display:none;">
                    <a data-bind="attr:{href:'#p='+$root.previousPage()+'&region='+currentRegion()+'&fy='+$root.selectedFY()+'&fp='+selectedFP()+'&fw='+selectedFW()+'&view=store'}" title="Go Back"><span class="icon-arrow-left"></span> Go Back</a>
                    <a href="#" data-bind="click: function(){$root.exportStoreDetail();}"><span class="icon-file"></span> Export</a>
                </span>

                <!-- TOOLBAR: RESOLUTION CODE BUTTONS -->
                <span data-bind="visible: currentPage() == 'rc_detail'" style="display:none;">
                    <a data-bind="attr:{href:'#p='+$root.previousPage()+'&region='+currentRegion()+'&fy='+$root.selectedFY()+'&fp='+selectedFP()+'&fw='+selectedFW()+'&view=rc'}" title="Go Back"><span class="icon-arrow-left"></span> Go Back</a>
                    <a href="#" data-bind="click: function(){$root.exportRCDetail();}"><span class="icon-file"></span> Export</a>
                </span>

                <div style="clear:both;"></div>
            </div>
            <!-- HEADER: BREAD CRUMBS -->
            <div class="crumb_trail">
                <a class="crumb" href="#p=global_home" data-bind="visible: currentPage() != ''"><span class="icon-home"></span> Global</a>
                <a class="crumb" href="#" style="display:none;" data-bind="visible: currentPage() != 'global_home' && currentPage() != '', text: currentRegion() + ' Region', attr:{href: '#p=regional_home&region='+currentRegion() + '&fy='+selectedFY()}">Regional</a>
                <a class="crumb" href="#" style="display:none;" data-bind="visible: showBCFiscalPeriod, text: selectedFPLabel, attr:{href: '#p=fiscal_period&region='+currentRegion()+'&fy='+selectedFY()+'&fp='+selectedFP()+'&view='+currentDetailView()}">Fiscal Period</a>
                <a class="crumb" href="#" style="display:none;" data-bind="visible: showBCFiscalWeek, text: selectedFWLabel, attr:{href: '#p=fiscal_week&region='+currentRegion()+'&fy='+selectedFY()+'&fp='+selectedFP()+'&fw='+selectedFW()+'&view='+currentDetailView()}">Fiscal Week</a>
                <a class="crumb" href="#" style="display:none;" data-bind="visible: currentPage() == 'vendor_detail', text: selectedVendor(), attr:{href:'#p=vendor_detail&region='+currentRegion()+'&fy='+selectedFY()+'&fp='+selectedFP()+'&fw='+selectedFW()+'&view=vendor&vendor='+selectedVendor()}">Vendor</a>
                <a class="crumb" href="#" style="display:none;" data-bind="visible: currentPage() == 'store_detail', text: selectedStore(), attr:{href:'#p=store_detail&region='+currentRegion()+'&fy='+selectedFY()+'&fp='+selectedFP()+'&fw='+selectedFW()+'&view=store&store='+selectedStore()}">Store</a>
                <a class="crumb" href="#" style="display:none;" data-bind="visible: currentPage() == 'rc_detail', text: selectedResolutionCode(), attr:{href:'#p=rc_detail&region='+currentRegion()+'&fy='+selectedFY()+'&fp='+selectedFP()+'&fw='+selectedFW()+'&view=rc&rc='+selectedResolutionCode()}">Resolution Code</a>
            </div>
        </div>
        <!-- INITIALIZING PAGE -->
        <div class="content" id="init_home" style="width:65%; min-width:650px;" data-bind="visible: $root.currentPage() == ''">
            <div class="wrapper">
                <h1>Initializing ...</h1>
            </div>
        </div>

        <!-- GLOBAL CONTENT -->
        <div class="content" id="global_home" style="min-width:650px;" data-bind="visible: $root.currentPage() == 'global_home'">
            <div class="wrapper">
                <h1 data-bind="text:'All Regions Fiscal Year ' + $root.selectedFY()">All Regions</h1>
                <div style="margin-bottom:1em; text-align:center;">
                    <a data-bind="click:function(){$root.showAllGlobalFPs();}" href="javascript:void(0);">Open All</a> / 
                    <a data-bind="click:function(){$root.hideAllGlobalFPs();}" href="javascript:void(0);">Close All</a>
                </div>

                <table id="global_main_table" class="fw_table">
                    <thead>
                        <tr>
                            <th style="width:10%"></th>
                            <th style="width:90%" colspan="3" data-bind="text:'PO Totals for FY ' + selectedFY()">PO Totals for FY XX</th>
                        </tr>
                        <tr class="subheader">
                            <th></th>
                            <th>Total PO</th>
                            <th>Suspended PO</th>
                            <th>Percent of Total</th>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: globalData">
                        <tr class="total_row" data-bind="attr:{id:'fp-' + Period}">
                            <th class="clickable" data-bind="html: Label(), click: function(data, event){ $root.toggleGlobalFP(data.Period, 'open'); }"></th>
                            <td class="fp_totals" data-bind="text: numberWithCommas(TotalPO)"></td>
                            <td class="fp_totals" data-bind="text: numberWithCommas(SuspendedPO)"></td>
                            <td class="fp_totals" data-bind="text: PercentOfTotalLabel"></td>
                        </tr>
                        <tr class="detail_row" data-bind="attr:{id:'fp-detail-' + Period}" style="display:none;">
                            <th class="clickable" data-bind="html: Label(), click: function(data, event){ $root.toggleGlobalFP(data.Period, 'close'); }"></th>
                            <td colspan="3">
                            <table class="fp_detail">
                                    <thead>
                                        <tr>
                                            <th></th>
                                            <th>Total PO Count</th>
                                            <th>Suspended PO Count</th>
                                            <th>% of Total</th>
                                            <th class="{sorter: false}"></th>
                                        </tr>
                                    </thead>
                                    <tbody data-bind="foreach: Regions">
                                        <tr>
                                            <th data-bind="text: RegionName"></th>
                                            <td data-bind="text: numberWithCommas(TotalPO)">0</td>
                                            <td data-bind="text: numberWithCommas(SuspendedPO)">0</td>
                                            <td data-bind="text: PercentOfTotalLabel(), css: weekRatingClass()">0%</td>
                                            <td><a data-bind="attr:{href: '#p=regional_home&region=' + Region + '&fy=' + $root.selectedFY(), title: 'Goto ' + RegionName}"><span class="icon-circle-arrow-right" title="More"></span></a></td>
                                        </tr>
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <th></th>
                                            <td data-bind="text: numberWithCommas(TotalPO)"></td>
                                            <td data-bind="text: numberWithCommas(SuspendedPO)"></td>
                                            <td data-bind="text: PercentOfTotalLabel() + ' FY'"></td>
                                            <td></td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <h5 style="margin: 0 0 0.5em; text-align: center;"><%=CurrentFiscalWeek %></h5>
                <a href="#" class="center" data-bind="click: function(){gotoTop()}">Top of page</a>
            </div>
        </div>
        <!-- REGIONAL FISCAL YEAR VIEW -->
        <div class="content" id="regional_home" style="min-width:650px;" data-bind="visible: $root.currentPage() == 'regional_home'">
            <div class="wrapper">
                <h1 data-bind="text: $root.currentRegion() + ' Region Fiscal Year ' + $root.selectedFY()">Region Name</h1>

                <div style="margin-bottom:1em; text-align:center;">
                    <a data-bind="click:function(){$root.showAllFPs();}" href="javascript:void(0);">Open All</a> / 
                    <a data-bind="click:function(){$root.hideAllFPs();}" href="javascript:void(0);">Close All</a>
                </div>

                <table id="fp_main_table" class="fw_table">
                    <thead>
                        <tr>
                            <th style="width:10%"></th>
                            <th style="width:90%" colspan="4" data-bind="text: currentRegion() + ' PO Totals for FY ' + selectedFY()">PO Totals for FY XX</th>                            
                        </tr>
                        <tr class="subheader">
                            <th></th>
                            <th>Total PO</th>
                            <th>Suspended PO</th>
                            <th>Percent of Total</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: fiscalPeriods">
                        <tr class="total_row" data-bind="attr:{id:'fp-' + Period}">
                            <th class="clickable" data-bind="html: Label(), click: function(data, event){ $root.toggleFP(data.Period, 'open'); }"></th>
                            <td data-bind="text: numberWithCommas(TotalPO)"></td>
                            <td data-bind="text: numberWithCommas(SuspendedPO)"></td>
                            <td data-bind="text: PercentOfTotalLabel"></td>
                            <td><a data-bind="attr:{href:'#p=fiscal_period&region='+$root.currentRegion()+'&fy='+Year+'&fp='+Period+'&view=vendor', title:'Goto FY'+Year + ' FP' + Period}" class="fp_detail_btn"><span class="icon-circle-arrow-right" title="View Period"></span></a></td>
                        </tr>
                        <tr class="detail_row" data-bind="attr:{id:'fp-detail-' + Period}" style="display:none;">
                            <th class="clickable" data-bind="html: Label(), click: function(data, event){ $root.toggleFP(data.Period, 'close'); }"></th>
                            <td colspan="4">
                                <table class="fp_detail">
                                    <thead>
                                        <tr>
                                            <th></th>
                                            <th>Total PO Count</th>
                                            <th>Suspended PO Count</th>
                                            <th>% of Total</th>
                                            <th class="{sorter: false}"></th>
                                        </tr>
                                    </thead>
                                    <tbody data-bind="foreach: FiscalWeeks">
                                        <tr>
                                            <th data-bind="html: Label()"></th>
                                            <td data-bind="text: numberWithCommas(TotalPO)">0</td>
                                            <td data-bind="text: numberWithCommas(SuspendedPO)">0</td>
                                            <td data-bind="text: PercentOfTotalLabel(), css: weekRatingClass()">0%</td>
                                            <td><a data-bind="attr:{href:'#p=fiscal_week&region='+$root.currentRegion()+'&fy='+Year+'&fp='+Period+'&fw='+Week+'&view=vendor', title:'Goto FY'+Year + ' FP'+Period + ' FW'+Week}"><span class="icon-circle-arrow-right" title="View Week"></span></a></td>
                                        </tr>
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <th></th>
                                            <td data-bind="text: numberWithCommas(TotalPO)"></td>
                                            <td data-bind="text: numberWithCommas(SuspendedPO)"></td>
                                            <td data-bind="text: PercentOfTotalLabel() + ' FY'"></td>
                                            <td></td>
                                        </tr>
                                    </tfoot>
                                </table>
                                <div style="text-align:right;">
                                <a data-bind="attr:{href:'#p=fiscal_period&region='+$root.currentRegion()+'&fy='+Year+'&fp='+Period+'&view=vendor', title:'Goto FY'+Year + ' FP' + Period}" class="fp_detail_btn">View Period <span class="icon-circle-arrow-right" title="View Period"></span></a>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <h5 style="margin: 0 0 0.5em; text-align: center;"><%=CurrentFiscalWeek %></h5>
                <a href="#" class="center" data-bind="click: function(){gotoTop()}">Top of page</a>
            </div>
        </div>
        <!-- FISCAL WEEK/PERIOD DETAIL PAGE -->
        <div class="content" id="fiscal_week" style="display:none;" data-bind="visible: $root.currentPage() == 'fiscal_week' || $root.currentPage() == 'fiscal_period'">
            <div class="wrapper">
                <a class="detail_button" style="float:right;" title="Compare to another data set" data-bind="click: showCompareDialog">Compare</a>
                <label>View by: </label>
                <a class="detail_button view_vendor" title="Show by Vendor" data-bind="css:{selected: $root.currentDetailView() == 'vendor'}, attr:{href: '#p='+$root.currentPage()+'&region='+$root.currentRegion() + '&view=vendor&fy='+$root.selectedFY()+'&fp='+$root.selectedFP()+'&fw='+$root.selectedFW()}">Vendor</a>
                <a class="detail_button view_store" title="Show by Store" data-bind="css:{selected: $root.currentDetailView() == 'store'}, attr:{href: '#p='+$root.currentPage()+'&region='+$root.currentRegion() + '&view=store&fy='+$root.selectedFY()+'&fp='+$root.selectedFP()+'&fw='+$root.selectedFW()}">Store</a>
                <a class="detail_button view_rc" title="Show by Resolution Code" data-bind="css:{selected: $root.currentDetailView() == 'rc'}, attr:{href: '#p='+$root.currentPage()+'&region='+$root.currentRegion() + '&view=rc&fy='+$root.selectedFY()+'&fp='+$root.selectedFP()+'&fw='+$root.selectedFW()}">Resolution Code</a>
                <div style="clear:both;"></div>

            <!-- VENDOR TABLE -->
            <!-- ko with: fiscalVendorTables -->
            <div data-bind="visible: $root.currentDetailView() == 'vendor'" class="compare-tables-wrapper">
                <!-- ko with: $root.fiscalVendorTables()[0] -->
                <div class="inp-group" style="margin-top:1.17em;">
                    <label>Filter Vendor: </label><input type="text" data-bind="value: filterValue, valueUpdate:'afterkeydown'" style="width:100px;"/>
                    <a href="#" class="detail_button" title="Clear filter" data-bind="click: function($data){ $data.filterValue(''); }"><span class="icon-remove"></span></a>
                </div>
                <!-- /ko -->
                <!-- ko foreach: $data -->
                <div data-bind="attr: { class: 'compare-tables-' + $root.fiscalVendorTables().length }">
                <div style="float:right; margin: 1em 0;">                    
                    <a class="warning_button" title="Remove" data-bind="visible: isCompareTable, click: $root.removeCompare"><span class="icon-remove"></span> <span class="rp-hide">Remove</span></a>
                </div>
                <h3 data-bind="visible: $root.currentPage() == 'fiscal_period'" style="float:left; display:none;"><span data-bind="text: 'FP' + FiscalPeriod() + ' FY' + FiscalYear()"></span></h3>
                <h3 data-bind="visible: $root.currentPage() == 'fiscal_week'" style="float:left; display:none;"><span data-bind="text: 'Week '+ FiscalWeek() +' FP' + FiscalPeriod() + ' FY' + FiscalYear()"></span></h3>
                <div style="clear:both;"></div>
                <div data-bind="visible: isLoading" style="width: 100%; height: 100%; min-height: 100%; position: relative; overflow: hidden; background: none repeat scroll 0px 0px rgb(255, 255, 255); border: 1px solid rgb(170, 170, 170);">
                    <img src="img/wfm_loading_circle2.gif" style="display:block; margin:1em auto;" alt="Loading"/>
                </div>
                <table class="fw_detail_table" data-bind="visible: isLoading() === false, attr:{id: 'fiscal-vendor-table-' + ID() }">
                    <thead>                    
                        <tr>
                            <th data-bind="click: sort.bind($data, 'Vendor', $element)"><span class="sort_arrow"></span>Vendor</th>
                            <th data-bind="visible: $root.fiscalVendorTables().length == 1, click: sort.bind($data, 'TotalPO', $element)"><span class="sort_arrow"></span>Total PO Count</th>                        
                            <th data-bind="visible: $root.fiscalVendorTables().length == 1,click: sort.bind($data, 'SuspendedPO', $element)"><span class="sort_arrow"></span>Suspended PO Count</th>
                            <th data-bind="visible: $root.fiscalVendorTables().length < 3,click: sort.bind($data, 'PercentOfTotal', $element)"><span class="sort_arrow"></span>% of Total</th>
                            <th data-bind="click: sort.bind($data, 'SuspendedContribution', $element)"><span class="sort_arrow"></span>Suspended Contribution</th>
                            <th></th>
                        </tr>  
                        <tr>
                            <th colspan="6" class="navbar">
                                <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: lastPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-fast-forward"></span></a>
                                <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: nextPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-forward" ></span></a>
                                <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: firstPage, css: {disabled: currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                                <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: previousPage, css: {disabled: currentPage() <= 1}"><span class="icon-backward"></span></a>
                                <span class="nav_label">Page <span data-bind="text: currentPage()"></span> of <span data-bind="text: totalPages()"></span></span>
                            </th>
                        </tr>                  
                    </thead>
                    <tbody data-bind="foreach: pagedRows">
                        <tr>
                            <td data-bind="text: Vendor" style="text-align:left;"></td>
                            <td data-bind="visible: $root.fiscalVendorTables().length == 1, text: numberWithCommas(TotalPO)">0</td>
                            <td data-bind="visible: $root.fiscalVendorTables().length == 1, text: numberWithCommas(SuspendedPO)">0</td>
                            <td data-bind="visible: $root.fiscalVendorTables().length < 3, text: PercentOfTotalLabel(), css: weekRatingClass()">0%</td>
                            <td data-bind="text: SuspendedContributionLabel(), css: contributionRatingClass()">0%</td>
                            <td><a data-bind="attr:{href:'#p=vendor_detail&region='+$root.currentRegion() + '&fy='+$parent.FiscalYear() + '&fp='+$parent.FiscalPeriod()+'&fw='+$parent.FiscalWeek()+'&vendor='+Vendor + '&dt=' + $root.currentPage(), title:'Goto ' + Vendor + ' detail'}"><span class="icon-circle-arrow-right" title="More"></span></a></td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <th colspan="6" class="navbar">
                                <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: lastPage, css: {disabled: currentPage() < totalPages()}"><span class="icon-fast-forward"></span></a>
                                <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: nextPage, css: {disabled: currentPage() < totalPages()}"><span class="icon-forward" ></span></a>
                                <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: firstPage, css: {disabled: currentPage() > 1}"><span class="icon-fast-backward"></span></a>                            
                                <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: previousPage, css: {disabled: currentPage() > 1}"><span class="icon-backward"></span></a>
                                <span class="nav_label">Page <span data-bind="text: currentPage()"></span> of <span data-bind="text: totalPages()"></span></span>
                            </th>
                        </tr>
                    </tfoot>
                </table>
                </div>
                <!-- /ko -->
                <div style="clear:both;"></div>
            </div>
            <!-- /ko -->
            <!-- END VENDOR TABLE -->
            <!-- STORE TABLE -->
            <!-- ko with: fiscalStoreTables -->
            <div data-bind="visible: $root.currentDetailView() == 'store'">
                <!-- ko with: $root.fiscalStoreTables()[0] -->
                <div class="inp-group" style="margin-top:1.17em;">
                    <label>Filter Store: </label><input type="text" data-bind="value: filterValue, valueUpdate:'afterkeydown'" style="width:100px;"/>
                    <a href="#" class="detail_button" title="Clear filter" data-bind="click: function($data){ $data.filterValue(''); }"><span class="icon-remove"></span></a>
                </div>
                <!-- /ko -->
                <!-- ko foreach: $data -->
                <div data-bind="attr: { class: 'compare-tables-' + $root.fiscalStoreTables().length }">
                
                <div style="float:right; margin: 1em 0;">
                    <a class="warning_button" title="Remove" data-bind="visible: isCompareTable, click: $root.removeCompare"><span class="icon-remove"></span></a>
                </div>
                <h3 data-bind="visible: $root.currentPage() == 'fiscal_period'" style="float:left; display:none;"><span data-bind="text: 'FP' + FiscalPeriod() + ' FY' + FiscalYear()"></span></h3>
                <h3 data-bind="visible: $root.currentPage() == 'fiscal_week'" style="float:left; display:none;"><span data-bind="text: 'Week '+ FiscalWeek() +' FP' + FiscalPeriod() + ' FY' + FiscalYear()"></span></h3>
                <div style="clear:both;"></div>
                <div data-bind="visible: isLoading" style="width: 100%; height: 100%; min-height: 100%; position: relative; overflow: hidden; background: none repeat scroll 0px 0px rgb(255, 255, 255); border: 1px solid rgb(170, 170, 170);">
                    <img src="img/wfm_loading_circle2.gif" style="display:block; margin:1em auto;" alt="Loading"/>
                </div>
                <table class="fw_store_table" data-bind="visible: isLoading() === false, attr:{id: 'fiscal-store-table-' + ID() }">
                    <thead>                    
                        <tr>
                            <th data-bind="click: sort.bind($data, 'Store', $element)"><span class="sort_arrow"></span>Store</th>
                            <th data-bind="visible: $root.fiscalStoreTables().length == 1, click: sort.bind($data, 'TotalPO', $element)"><span class="sort_arrow"></span>Total PO Count</th>
                            <th data-bind="visible: $root.fiscalStoreTables().length == 1, click: sort.bind($data, 'SuspendedPO', $element)"><span class="sort_arrow"></span>Suspended PO Count</th>
                            <th data-bind="visible: $root.fiscalStoreTables().length < 3, click: sort.bind($data, 'PercentOfTotal', $element)"><span class="sort_arrow"></span>% of Total</th>
                            <th data-bind="click: sort.bind($data, 'SuspendedContribution', $element)"><span class="sort_arrow"></span>Suspended Contribution</th>
                            <th></th>
                        </tr>
                        <tr>
                            <th colspan="6" class="navbar">
                                <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: lastPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-fast-forward"></span></a>
                                <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: nextPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-forward" ></span></a>
                                <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: firstPage, css: {disabled: currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                                <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: previousPage, css: {disabled: currentPage() <= 1}"><span class="icon-backward"></span></a>
                                <span class="nav_label">Page <span data-bind="text: currentPage()"></span> of <span data-bind="text: totalPages()"></span></span>
                            </th>
                        </tr>                    
                    </thead>
                    <tbody data-bind="foreach: pagedRows">
                        <tr>
                            <td data-bind="text: Store" style="text-align:left;"></td>
                            <td data-bind="visible: $root.fiscalStoreTables().length == 1, text: numberWithCommas(TotalPO)">0</td>
                            <td data-bind="visible: $root.fiscalStoreTables().length == 1, text: numberWithCommas(SuspendedPO)">0</td>
                            <td data-bind="visible: $root.fiscalStoreTables().length < 3, text: PercentOfTotalLabel(), css: weekRatingClass()">0%</td>
                            <td data-bind="text: SuspendedContributionLabel(), css: contributionRatingClass()">0%</td>
                            <td><a data-bind="attr:{href:'#p=store_detail&region='+$root.currentRegion() + '&fy='+$parent.FiscalYear() + '&fp='+$parent.FiscalPeriod()+'&fw='+$parent.FiscalWeek()+'&store='+Store + '&dt=' + $root.currentPage(), title:'Goto ' + Store + ' detail'}"><span class="icon-circle-arrow-right" title="More"></span></a></td>
                        </tr>                    
                    </tbody>
                    <tfoot>
                        <tr>
                            <th colspan="6" class="navbar">
                                <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: lastPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-fast-forward"></span></a>
                                <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: nextPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-forward" ></span></a>
                                <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: firstPage, css: {disabled: currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                                <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: previousPage, css: {disabled: currentPage() <= 1}"><span class="icon-backward"></span></a>
                                <span class="nav_label">Page <span data-bind="text: currentPage()"></span> of <span data-bind="text: totalPages()"></span></span>
                            </th>
                        </tr>
                    </tfoot>
                </table>
                </div>
                <!-- /ko -->
                <div style="clear:both;"></div>
            </div>
            <!-- /ko -->
            <!-- END STORE TABLE -->
            <!-- RESOLUTION CODE TABLE -->
            <!-- ko with: fiscalRCTables -->
            <div data-bind="visible: $root.currentDetailView() == 'rc'">
                <!-- ko with: $root.fiscalRCTables()[0] -->
                <div class="inp-group" style="margin-top:1.17em;">
                    <label>Filter Store: </label><input type="text" data-bind="value: filterValue, valueUpdate:'afterkeydown'" style="width:100px;"/>
                    <a href="#" class="detail_button" title="Clear filter" data-bind="click: function($data){ $data.filterValue(''); }"><span class="icon-remove"></span></a>
                </div>
                <!-- /ko -->
                <!-- ko foreach: $data -->
                <div data-bind="attr: { class: 'compare-tables-' + $root.fiscalRCTables().length }">
                
                <div style="float:right; margin: 1em 0;">
                    <a class="warning_button" title="Remove" data-bind="visible: isCompareTable, click: $root.removeCompare"><span class="icon-remove"></span></a>
                </div>
                <h3 data-bind="visible: $root.currentPage() == 'fiscal_period'" style="float:left; display:none;"><span data-bind="text: 'FP' + FiscalPeriod() + ' FY' + FiscalYear()"></span></h3>
                <h3 data-bind="visible: $root.currentPage() == 'fiscal_week'" style="float:left; display:none;"><span data-bind="text: 'Week '+ FiscalWeek() +' FP' + FiscalPeriod() + ' FY' + FiscalYear()"></span></h3>
                <div style="clear:both;"></div>
                <div data-bind="visible: isLoading" style="width: 100%; height: 100%; min-height: 100%; position: relative; overflow: hidden; background: none repeat scroll 0px 0px rgb(255, 255, 255); border: 1px solid rgb(170, 170, 170);">
                    <img src="img/wfm_loading_circle2.gif" style="display:block; margin:1em auto;" alt="Loading"/>
                </div>
                <table class="fw_resolution_table" data-bind="visible: isLoading() === false, attr:{id: 'fiscal-rc-table-' + ID() }">
                    <thead>                    
                        <tr>
                            <th data-bind="click: sort.bind($data, 'ResolutionCode', $element)"><span class="sort_arrow"></span>Resolution Code</th>
                            <th data-bind="visible: $root.fiscalRCTables().length < 3, click: sort.bind($data, 'SuspendedPO', $element)"><span class="sort_arrow"></span>Suspended PO Count</th>
                            <th data-bind="click: sort.bind($data, 'SuspendedContribution', $element)"><span class="sort_arrow"></span>Suspended Contribution</th>
                            <th></th>
                        </tr> 
                        <tr>
                            <th colspan="6" class="navbar">
                                <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: lastPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-fast-forward"></span></a>
                                <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: nextPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-forward" ></span></a>
                                <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: firstPage, css: {disabled: currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                                <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: previousPage, css: {disabled: currentPage() <= 1}"><span class="icon-backward"></span></a>
                                <span class="nav_label">Page <span data-bind="text: currentPage()"></span> of <span data-bind="text: totalPages()"></span></span>
                            </th>
                        </tr>                   
                    </thead>
                    <tbody data-bind="foreach: pagedRows">
                        <tr>
                            <td data-bind="text: ResolutionCode" style="text-align:left;"></td>
                            <td data-bind="visible: $root.fiscalRCTables().length < 3, text: numberWithCommas(SuspendedPO)">0</td>
                            <td data-bind="text: SuspendedContributionLabel(), css: contributionRatingClass()">0%</td>
                            <td><a data-bind="attr:{href:'#p=rc_detail&region='+$root.currentRegion() + '&fy='+$parent.FiscalYear() + '&fp='+$parent.FiscalPeriod()+'&fw='+$parent.FiscalWeek()+'&rc='+ResolutionCode + '&dt=' + $root.currentPage(), title:'Goto ' + ResolutionCode + ' detail'}"><span class="icon-circle-arrow-right" title="More"></span></a></td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <th colspan="6" class="navbar">
                                <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: lastPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-fast-forward"></span></a>
                                <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: nextPage, css: {disabled: currentPage() >= totalPages()}"><span class="icon-forward" ></span></a>
                                <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: firstPage, css: {disabled: currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                                <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: previousPage, css: {disabled: currentPage() <= 1}"><span class="icon-backward"></span></a>
                                <span class="nav_label">Page <span data-bind="text: currentPage()"></span> of <span data-bind="text: totalPages()"></span></span>
                            </th>
                        </tr>
                    </tfoot>
                </table>
                </div>
                <!-- /ko -->
                <div style="clear:both;"></div>            
            </div>
            <!-- /ko -->
            <!-- END RESOLUTION CODE TABLE -->
            <!-- PAGE FOOTER -->
            <h5 style="margin: 0 0 0.5em; text-align: center;"><%=CurrentFiscalWeek %></h5>
            <a href="#" class="center" data-bind="click: function(){gotoTop()}">Top of page</a>
            </div>
        </div>
                
        <!-- VENDOR DETAIL PAGE -->
        <div class="content" id="vendor_detail" style="display:none; width:100%;" data-bind="visible: $root.currentPage() == 'vendor_detail'">
            <div class="wrapper">
                <div class="chart_wrapper">
                    <h3>Vendor Trending - Fiscal Year <span data-bind="text: selectedFY()"></span></h3>
                    <div style="width: 770px; height: 300px; background:white; text-align:center; display:inline-block;" data-bind="visible: $root.isLoadingVendorTrends">
                        <img src="img/wfm_loading_circle2.gif" alt="Loading Trends" style="display:block; margin:0 auto; padding-top:73px;" />
                    </div>
                    <div data-bind="visible: $root.isLoadingVendorTrends() === false">
                        <div id="vendor_chart" class="chart" style="width: 770px; height: 300px; display:inline-block;"></div>
                    </div>
                </div>
                <div class="resolution_totals">
                    <h3>Resolution Code Totals</h3>
                    <table class="vendor_resolution_table">
                    <thead>
                        <tr>
                            <th>Code Name</th>
                            <th>Count</th>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: resolutionTotals">
                        <tr>
                            <td style="text-align:left;" data-bind="text: Name"></td>
                            <td><span data-bind="text: Total"></td>
                        </tr>
                    </tbody>
                    </table>
                </div>
            <h3 data-bind="visible: $root.selectedDetailType() == 'fiscal_period'">Fiscal Period <span data-bind="text: selectedFP()"></span> FY<span data-bind="text: selectedFY()"></span> PO Detail for <span data-bind="text: selectedVendor()"></span></h3>
            <h3 data-bind="visible: $root.selectedDetailType() == 'fiscal_week'">Fiscal Week <span data-bind="text: selectedFW()"></span> FP<span data-bind="text: selectedFP()"></span> FY<span data-bind="text: selectedFY()"></span> PO Detail for <span data-bind="text: selectedVendor()"></span></h3>
            <table class="vendor_detail_table">
                <thead>                    
                    <tr>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONumber', $element)">PO #<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Suspended', $element)">Suspended<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'FormattedCloseDate', $element)">Close Date<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'ResolutionCode', $element)">Resolution Code<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'AdminNotes', $element)">PO Admin Notes<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Subteam', $element)">Subteam<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Store', $element)">Store<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'AdjustedCost', $element)">Adj. Cost<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'CreditPO', $element)">Credit PO<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'VendorType', $element)">Vendor Type<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'POCreator', $element)">PO Creator<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONuEInvoiceMatchedToPOmber', $element)">EInvoice<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONotes', $element)">PO Notes<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'ClosedBy', $element)">Closer<span class="sort_arrow_bottom"></span></th>
                    </tr>
                    <tr>
                        <th colspan="14" class="navbar">
                            <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: $root.PORowPages.lastPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-fast-forward"></span></a>
                            <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: $root.PORowPages.nextPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-forward" ></span></a>
                            <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: $root.PORowPages.firstPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                            <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: $root.PORowPages.previousPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-backward"></span></a>
                            <span class="nav_label">Page <span data-bind="text: $root.PORowPages.currentPage()"></span> of <span data-bind="text: $root.PORowPages.totalPages()"></span></span>
                        </th>
                    </tr>
                </thead>
                <tbody data-bind="foreach: PORowPages.pagedRows">
                    <tr>
                        <td data-bind="text: PONumber" style="text-align:left;"></td>
                        <td data-bind="text: Suspended" style="text-align:center;">N</td>
                        <td data-bind="text: FormattedCloseDate()">0</td>
                        <td data-bind="text: ResolutionCode">0</td>
                        <td data-bind="text: AdminNotes">0</td>
                        <td data-bind="text: Subteam">0</td>
                        <td data-bind="text: Store">0</td>
                        <td data-bind="text: AdjustedCost" style="text-align:center;">0</td>
                        <td data-bind="text: CreditPO" style="text-align:center;">0</td>
                        <td data-bind="text: VendorType">0</td>
                        <td data-bind="text: POCreator">0</td>
                        <td data-bind="text: EInvoiceMatchedToPO" style="text-align:center;">0</td>
                        <td data-bind="text: PONotes">0</td>
                        <td data-bind="text: ClosedBy">0</td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <th colspan="14" class="navbar">
                            <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: $root.PORowPages.lastPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-fast-forward"></span></a>
                            <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: $root.PORowPages.nextPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-forward" ></span></a>
                            <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: $root.PORowPages.firstPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                            <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: $root.PORowPages.previousPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-backward"></span></a>
                            <span class="nav_label">Page <span data-bind="text: $root.PORowPages.currentPage()"></span> of <span data-bind="text: $root.PORowPages.totalPages()"></span></span>
                        </th>
                    </tr>
                </tfoot>
            </table>
            <h5 style="margin: 0 0 0.5em; text-align: center;"><%=CurrentFiscalWeek %></h5>
            <a href="#" class="center" data-bind="click: function(){gotoTop()}">Top of page</a>
            </div>
        </div>
        <!-- STORE DETAIL PAGE -->
        <div class="content" id="store_detail" style="display:none; width:100%;" data-bind="visible: $root.currentPage() == 'store_detail'">
            <div class="wrapper">
                <div class="chart_wrapper">
                    <h3>Store Trending - Fiscal Year <span data-bind="text: selectedFY()"></span></h3>
                    <div style="width: 770px; height: 300px; background:white; text-align:center; display:inline-block;" data-bind="visible: $root.isLoadingStoreTrends">
                        <img src="img/wfm_loading_circle2.gif" alt="Loading Trends" style="display:block; margin:0 auto; padding-top:73px;" />
                    </div>
                    <div data-bind="visible: $root.isLoadingStoreTrends() === false">
                        <div id="store_chart" class="chart" style="width: 770px; height: 300px; display:inline-block;"></div>
                    </div>
                </div>
                <div class="resolution_totals">
                    <h3>Resolution Code Totals</h3>
                    <table class="vendor_resolution_table">
                    <thead>
                        <tr>
                            <th>Code Name</th>
                            <th>Count</th>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: resolutionTotals">
                        <tr>
                            <td style="text-align:left;" data-bind="text: Name"></td>
                            <td><span data-bind="text: Total"></td>
                        </tr>
                    </tbody>
                    </table>
                </div>
            <h3 data-bind="visible: $root.selectedDetailType() == 'fiscal_period'">Fiscal Period <span data-bind="text: selectedFP()"></span> FY<span data-bind="text: selectedFY()"></span> PO Detail for <span data-bind="text: selectedStore()"></span></h3>
            <h3 data-bind="visible: $root.selectedDetailType() == 'fiscal_week'">Fiscal Week <span data-bind="text: selectedFW()"></span> FP<span data-bind="text: selectedFP()"></span> FY<span data-bind="text: selectedFY()"></span> PO Detail for <span data-bind="text: selectedStore()"></span></h3>

            <table class="vendor_detail_table">
                <thead>                    
                    <tr>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONumber', $element)">PO #<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Suspended', $element)">Suspended<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Vendor', $element)">Vendor<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'FormattedCloseDate', $element)">Close Date<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'ResolutionCode', $element)">Resolution Code<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'AdminNotes', $element)">PO Admin Notes<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Subteam', $element)">Subteam<span class="sort_arrow_bottom"></span></th>                        
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'AdjustedCost', $element)">Adj. Cost<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'CreditPO', $element)">Credit PO<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'VendorType', $element)">Vendor Type<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'POCreator', $element)">PO Creator<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONuEInvoiceMatchedToPOmber', $element)">EInvoice<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONotes', $element)">PO Notes<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'ClosedBy', $element)">Closer<span class="sort_arrow_bottom"></span></th>
                    </tr>
                    <tr>
                        <th colspan="14" class="navbar">
                            <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: $root.PORowPages.lastPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-fast-forward"></span></a>
                            <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: $root.PORowPages.nextPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-forward" ></span></a>
                            <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: $root.PORowPages.firstPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                            <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: $root.PORowPages.previousPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-backward"></span></a>
                            <span class="nav_label">Page <span data-bind="text: $root.PORowPages.currentPage()"></span> of <span data-bind="text: $root.PORowPages.totalPages()"></span></span>
                        </th>
                    </tr>
                </thead>
                <tbody data-bind="foreach: PORowPages.pagedRows">
                    <tr>
                        <td data-bind="text: PONumber" style="text-align:left;"></td>
                        <td data-bind="text: Suspended" style="text-align:center;">N</td>
                        <td data-bind="text: Vendor">N</td>
                        <td data-bind="text: FormattedCloseDate()">0</td>
                        <td data-bind="text: ResolutionCode">0</td>
                        <td data-bind="text: AdminNotes">0</td>
                        <td data-bind="text: Subteam">0</td>
                        <td data-bind="text: AdjustedCost" style="text-align:center;">0</td>
                        <td data-bind="text: CreditPO" style="text-align:center;">0</td>
                        <td data-bind="text: VendorType">0</td>
                        <td data-bind="text: POCreator">0</td>
                        <td data-bind="text: EInvoiceMatchedToPO" style="text-align:center;">0</td>
                        <td data-bind="text: PONotes">0</td>
                        <td data-bind="text: ClosedBy">0</td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <th colspan="14" class="navbar">
                            <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: $root.PORowPages.lastPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-fast-forward"></span></a>
                            <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: $root.PORowPages.nextPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-forward" ></span></a>
                            <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: $root.PORowPages.firstPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                            <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: $root.PORowPages.previousPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-backward"></span></a>
                            <span class="nav_label">Page <span data-bind="text: $root.PORowPages.currentPage()"></span> of <span data-bind="text: $root.PORowPages.totalPages()"></span></span>
                        </th>
                    </tr>
                </tfoot>
            </table>
            <h5 style="margin: 0 0 0.5em; text-align: center;"><%=CurrentFiscalWeek %></h5>
            <a href="#" class="center" data-bind="click: function(){gotoTop()}">Top of page</a>
            </div>
        </div>
        <!-- RESOLUTION CODE DETAIL PAGE -->
        <div class="content" id="rc_detail" style="display:none; width:100%;" data-bind="visible: $root.currentPage() == 'rc_detail'">
            <div class="wrapper">
            <h3 data-bind="visible: $root.selectedDetailType() == 'fiscal_period'">Fiscal Period <span data-bind="text: selectedFP()"></span> FY<span data-bind="text: selectedFY()"></span> PO Detail for <span data-bind="text: selectedResolutionCode()"></span></h3>
            <h3 data-bind="visible: $root.selectedDetailType() == 'fiscal_week'">Fiscal Week <span data-bind="text: selectedFW()"></span> FP<span data-bind="text: selectedFP()"></span> FY<span data-bind="text: selectedFY()"></span> PO Detail for <span data-bind="text: selectedResolutionCode()"></span></h3>

            <table class="vendor_detail_table">
                <thead>                    
                    <tr>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONumber', $element)">PO #<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Suspended', $element)">Suspended<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Vendor', $element)">Vendor<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'FormattedCloseDate', $element)">Close Date<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'ResolutionCode', $element)">Resolution Code<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'AdminNotes', $element)">PO Admin Notes<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Subteam', $element)">Subteam<span class="sort_arrow_bottom"></span></th>     
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Store', $element)">Store<span class="sort_arrow_bottom"></span></th>                   
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'AdjustedCost', $element)">Adj. Cost<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'CreditPO', $element)">Credit PO<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'VendorType', $element)">Vendor Type<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'POCreator', $element)">PO Creator<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONuEInvoiceMatchedToPOmber', $element)">EInvoice<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONotes', $element)">PO Notes<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'ClosedBy', $element)">Closer<span class="sort_arrow_bottom"></span></th>
                    </tr>
                    <tr>
                        <th colspan="15" class="navbar">
                            <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: $root.PORowPages.lastPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-fast-forward"></span></a>
                            <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: $root.PORowPages.nextPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-forward" ></span></a>
                            <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: $root.PORowPages.firstPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                            <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: $root.PORowPages.previousPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-backward"></span></a>
                            <span class="nav_label">Page <span data-bind="text: $root.PORowPages.currentPage()"></span> of <span data-bind="text: $root.PORowPages.totalPages()"></span></span>
                        </th>
                    </tr>
                </thead>
                <tbody data-bind="foreach: PORowPages.pagedRows">
                    <tr>
                        <td data-bind="text: PONumber" style="text-align:left;"></td>
                        <td data-bind="text: Suspended" style="text-align:center;">N</td>
                        <td data-bind="text: Vendor">N</td>
                        <td data-bind="text: FormattedCloseDate()">0</td>
                        <td data-bind="text: ResolutionCode">0</td>
                        <td data-bind="text: AdminNotes">0</td>
                        <td data-bind="text: Subteam">0</td>
                        <td data-bind="text: Store">0</td>
                        <td data-bind="text: AdjustedCost" style="text-align:center;">0</td>
                        <td data-bind="text: CreditPO" style="text-align:center;">0</td>
                        <td data-bind="text: VendorType">0</td>
                        <td data-bind="text: POCreator">0</td>
                        <td data-bind="text: EInvoiceMatchedToPO" style="text-align:center;">0</td>
                        <td data-bind="text: PONotes">0</td>
                        <td data-bind="text: ClosedBy">0</td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <th colspan="15" class="navbar">
                            <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: $root.PORowPages.lastPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-fast-forward"></span></a>
                            <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: $root.PORowPages.nextPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-forward" ></span></a>
                            <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: $root.PORowPages.firstPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                            <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: $root.PORowPages.previousPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-backward"></span></a>
                            <span class="nav_label">Page <span data-bind="text: $root.PORowPages.currentPage()"></span> of <span data-bind="text: $root.PORowPages.totalPages()"></span></span>
                        </th>
                    </tr>
                </tfoot>
            </table>
            <h5 style="margin: 0 0 0.5em; text-align: center;"><%=CurrentFiscalWeek %></h5>
            <a href="#" class="center" data-bind="click: function(){gotoTop()}">Top of page</a>
            </div>
        </div>
        <!-- PAY BY AGREED COST VENDOR DETAIL PAGE -->
        <div class="content" id="pac_detail" style="display:none; width:100%;" data-bind="visible: $root.currentPage() == 'pac_detail'">
            <div class="wrapper">
                <div class="chart_wrapper">
                    <h3>Vendor Trending - Fiscal Year <span data-bind="text: selectedFY()"></span></h3>
                    <div id="Div2" class="chart" style="width: 770px; height: 300px; display:inline-block;"></div>
                </div>
                <div class="resolution_totals">
                    <h3>Resolution Code Totals</h3>
                    <table class="vendor_resolution_table">
                    <thead>
                        <tr>
                            <th>Code Name</span></th>
                            <th>Count</span></th>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: resolutionTotals">
                        <tr>
                            <td style="text-align:left;" data-bind="text: Name"></td>
                            <td><span data-bind="text: Total"></td>
                        </tr>
                    </tbody>
                    </table>
                </div>
            <h3>Fiscal Week <span data-bind="text: selectedFW()"></span> PO Detail for <span data-bind="text: selectedVendor()"></span></h3>
            <table class="vendor_detail_table">
                <thead>                    
                    <tr>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONumber', $element)">PO #<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Suspended', $element)">Suspended<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'FormattedCloseDate', $element)">Close Date<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'ResolutionCode', $element)">Resolution Code<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'AdminNotes', $element)">PO Admin Notes<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Subteam', $element)">Subteam<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'Store', $element)">Store<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'AdjustedCost', $element)">Adj. Cost<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'CreditPO', $element)">Credit PO<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'VendorType', $element)">Vendor Type<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'POCreator', $element)">PO Creator<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONuEInvoiceMatchedToPOmber', $element)">EInvoice<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'PONotes', $element)">PO Notes<span class="sort_arrow_bottom"></span></th>
                        <th data-bind="click: $root.PORowPages.sort.bind($data, 'ClosedBy', $element)">Closer<span class="sort_arrow_bottom"></span></th>
                    </tr>
                    <tr>
                        <th colspan="14" class="navbar">
                            <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: $root.PORowPages.lastPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-fast-forward"></span></a>
                            <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: $root.PORowPages.nextPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-forward" ></span></a>
                            <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: $root.PORowPages.firstPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                            <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: $root.PORowPages.previousPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-backward"></span></a>
                            <span class="nav_label">Page <span data-bind="text: $root.PORowPages.currentPage()"></span> of <span data-bind="text: $root.PORowPages.totalPages()"></span></span>
                        </th>
                    </tr>
                </thead>
                <tbody data-bind="foreach: PORowPages.pagedRows">
                    <tr>
                        <td data-bind="text: PONumber" style="text-align:left;"></td>
                        <td data-bind="text: Suspended" style="text-align:center;">N</td>
                        <td data-bind="text: FormattedCloseDate()">0</td>
                        <td data-bind="text: ResolutionCode">0</td>
                        <td data-bind="text: AdminNotes">0</td>
                        <td data-bind="text: Subteam">0</td>
                        <td data-bind="text: Store">0</td>
                        <td data-bind="text: AdjustedCost" style="text-align:center;">0</td>
                        <td data-bind="text: CreditPO" style="text-align:center;">0</td>
                        <td data-bind="text: VendorType">0</td>
                        <td data-bind="text: POCreator">0</td>
                        <td data-bind="text: EInvoiceMatchedToPO" style="text-align:center;">0</td>
                        <td data-bind="text: PONotes">0</td>
                        <td data-bind="text: ClosedBy">0</td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <th colspan="14" class="navbar">
                            <a href="#" class="nav_button nav_last" title="Last page" style="float:right;" data-bind="click: $root.PORowPages.lastPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-fast-forward"></span></a>
                            <a href="#" class="nav_button nav_next" title="Next page" style="float:right;" data-bind="click: $root.PORowPages.nextPage, css: {disabled: $root.PORowPages.currentPage() >= $root.PORowPages.totalPages()}"><span class="icon-forward" ></span></a>
                            <a href="#" class="nav_button nav_first" title="First page" style="float:left;" data-bind="click: $root.PORowPages.firstPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-fast-backward"></span></a>                            
                            <a href="#" class="nav_button nav_previous" title="Previous page" style="float:left;" data-bind="click: $root.PORowPages.previousPage, css: {disabled: $root.PORowPages.currentPage() <= 1}"><span class="icon-backward"></span></a>
                            <span class="nav_label">Page <span data-bind="text: $root.PORowPages.currentPage()"></span> of <span data-bind="text: $root.PORowPages.totalPages()"></span></span>
                        </th>
                    </tr>
                </tfoot>
            </table>
            <h5 style="margin: 0 0 0.5em; text-align: center;"><%=CurrentFiscalWeek %></h5>
            <a href="#" class="center" data-bind="click: function(){gotoTop()}">Top of page</a>
            </div>
        </div>

        <!-- FOOTER & COPYRIGHT -->
        <img src="img/poman.png" alt="" id="poman" />
        <img src="img/wfm_logo_white.png" alt="" style="display:block; margin:1em auto;" />
        <div class="copyright">Copyright &copy; 2013 Whole Foods Market Inc.<span>To report website issues please email <a href="mailto:<%=SUPPORT_EMAIL %>"><%=SUPPORT_EMAIL %></a></span></div>
        <div style="text-align:center; font-size:8pt; color: #999;">Server: <%=SERVER_NAME %></div>
        <!-- LOADING DIALOG -->
        <div data-bind="visible: isLoading" class="loader" style="display:none;">
        	<h2>Loading</h2>
        	<img src="img/wfm_loading_circle2.gif" alt="" style="display:block; margin:0 auto; text-align:center;"/>
        </div>
        <!-- MAIN DIALOG -->
        <div data-bind="visible: showDialog" class="dialog" style="display:none;">
            <div data-bind="visible: dialogView() == 'download'">
                <h2>Download Ready</h2>
                <a href="#" id="download_link"><img src="img/excel_64x64.png" alt="" /><span style="display:block;">Download File</span></a>
                <a href="#" class="dialog-button" data-bind="click: function(d,e){$root.showDialog(false);}">Close</a>
            </div>
            <div data-bind="visible: dialogView() == 'compare'">
                <h2>Compare to</h2>
                <label>Fiscal Year</label>
                <select data-bind="value: dialogFY" style="margin-right:1em;">
                    <%=YEAR_OPTIONS %>
                </select>
                <label>Fiscal Period</label>
                <select data-bind="value: dialogFP">
                    <%=FP_OPTIONS %>
                </select>
                <span data-bind="visible: $root.currentPage() == 'fiscal_week'"> 
                    <label>Fiscal Week</label>
                    <select data-bind="value: dialogFW">
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                    </select>
                </span>
                <div>
                    <div style="width:46%; margin:0 2%; float: left;">
                        <a href="#" class="dialog-button" data-bind="click: function(d,e){$root.showDialog(false);}">Cancel</a>
                    </div>
                    <div style="width:46%; margin:0 2%; float: left;">
                        <a href="#" class="dialog-button" data-bind="click: $root.addCompare">GO</a>
                    </div>
                </div>
            </div>
        </div>

        <!-- SESSION DIALOG -->
        <div id="session_dialog" title="Your session is about to expire!">
	        <p class="center notice">You will be logged off in <span id="dialog-countdown"></span> seconds.</p>
            <div style="margin:1em auto 0 auto; text-align:center;"><img src="img/poman_logoff.gif" alt="" /></div>
        </div>
        <!-- LOADING MASK -->
        <div data-bind="visible: isLoading() || showDialog()" class="mask" style="display:none;"></div>

        <!-- SCRIPTS -->
        <script type="text/javascript" src="js/date.js"></script>  
        <script type="text/javascript" src="js/vendor/jquery.js"></script>  
        <script src="js/vendor/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
        <script type="text/javascript" src="js/vendor/JSON2.js"></script>
        <script type="text/javascript" src="js/vendor/knockout.js"></script>  
        <script type="text/javascript" src="js/vendor/jqwidgets/jqxknockout.js"></script>  
        <script type="text/javascript" src="js/vendor/jqwidgets/jqxcore.js"></script>  
        <script type="text/javascript" src="js/vendor/jqwidgets/jqxbuttons.js"></script>  
        <script type="text/javascript" src="js/vendor/jqwidgets/jqxscrollbar.js"></script>
	    <script type="text/javascript" src="js/vendor/jqwidgets/jqxlistbox.js"></script>
	    <script type="text/javascript" src="js/vendor/jqwidgets/jqxdata.js"></script>
        <script type="text/javascript" src="js/vendor/jqwidgets/jqxchart.js"></script>
	    <script type="text/javascript" src="js/vendor/jqwidgets/jqxcheckbox.js"></script>
	    <script type="text/javascript" src="js/vendor/jqwidgets/jqxdropdownlist.js"></script>
        <script src="js/vendor/sjcl.js" type="text/javascript"></script>
        <script src="js/vendor/jquery.ba-bbq.min.js" type="text/javascript"></script>
        <script src="js/vendor/jquery.timers.js" type="text/javascript"></script>
        <script src="js/vendor/jquery.idletimer.js" type="text/javascript"></script>
        <script src="js/vendor/jquery.idletimeout.js" type="text/javascript"></script>
        <script type="text/javascript" src="js/functions.js?v=1.0.2"></script>   
        <script type="text/javascript" src="js/classes.js?v=1.0.3"></script>   
        <script type="text/javascript" src="js/main.js?v=1.0.3"></script>    
    </body>
</html>
