$.fn.redrawIt = function () { return $(this).each(function () { var redraw = this.offsetHeight; }); };

var newSummaryGrid = (function () {

    function drawTable( data) {

        var table = $("#oTable");
        var source = $("#store-row-template").html();
        var base = $("#grid-base-template").html();

        resetGrid(table, base);

        var template = Handlebars.compile(source);
        data.forEach(function (e, i, a) {
            if (e.OosPercent < 0)
                console.log(e);
            e.formattedPercent = numeral(e.OosPercent).format('0.00%');
            table.append(template(e));
        });


        $("#btn-expandall").on("click", function() {
            showLoader(true);
            $(".clickable").each(function(i) {
                openElement($(this));
            });
            adjustHeights();
            showLoader(false);

        });

        $("#btn-collapseall").on("click", function() {
            showLoader(true);
            $(".clickable").each(function(i) {
                closeElement($(this));
            });
            adjustHeights();
            showLoader(false);
        });
        $(".clickable").on("click", function() {

            var element = $(this);
            if (element.hasClass("showingChildren")) {
                closeElement(element);
            } else {
                openElement(element);
            }

        });

        adjustHeights();
        showLoader(false);


    }


    function resetGrid(elem, base) {
            elem.html(base);
        }

        function showLoader(t) {
            var l = $("#loader");
            
            if (t)
                l.removeClass("collapsed");
            else
                l.addClass("collapsed");

            l.redrawIt();
        }

        function closeElement(elem) {
            var id = elem.data("id");
            var icon = $(elem).children(".oCell:first").children("span");
            icon.removeClass("minus").addClass("plus");

            elem.removeClass("showingChildren");
            $("*[data-parent=" + id + "]").each(function (i) {
                var t = $(this);
                t.addClass("collapsed");
                if (t.hasClass("showingChildren"))
                    closeElement(t);
            });
        }

        function openElement(elem) {
            var icon = $(elem).children(".oCell:first").children("span");
            icon.removeClass("plus").addClass("minus");
            elem.addClass("showingChildren");
            $("*[data-parent=" + $(elem).data("id") + "]").each(function (i) {
                var t = $(this);
                t.removeClass("collapsed").fadeIn(300);
            });
        }

        return {
            drawTable: function(d) {
                drawTable(d);
            },
            showLoader: function(t) {
                showLoader(t);
            }
        };

    
})();

function RunReport() {

    var startDate = $('#selectedStartDate').val();
    var endDate = $('#selectedEndDate').val();
    var region = ($('#ddlRegions').val());
    console.log(startDate);
    console.log(endDate);
    console.log(region);
    newSummaryGrid.showLoader(true);

    $.ajax({
        url: "/SummaryReport/GetSummaryPageData",
        data: { region: region, start: startDate, end: endDate },
        method: 'POST'
    }).done(function (data) {
        newSummaryGrid.drawTable(data);

    });

}