var brandExportList = [];

$(document).ready(function () {
    $('#export').click(function () {
        hierarcyClassExportList = [];

        buildBrandExportList($('#brandGrid').data('igGrid').dataSource.data());

        $.fileDownload('/Excel/BrandExport', {
            httpMethod: 'POST',
            data: { "brands": JSON.stringify(brandExportList) },
            datatype: 'json',
        });
    });
});

function buildBrandExportList(igGridBrandData) {
    for (var i = 0; i < igGridBrandData.length; i++) {

        var brandExportModel = {
            BrandName: igGridBrandData[i].BrandName,
            BrandAbbreviation: igGridBrandData[i].BrandAbbreviation
        };

        brandExportList.push(brandExportModel);
        brandExportModel.BrandName += '|' + igGridBrandData[i].HierarchyClassId;
    }
}

// Helper function to allow toggling of the 'disabled' CSS class.
jQuery.fn.extend({
    disable: function (state) {
        return this.each(function () {
            var $this = $(this);
            $this.toggleClass('disabled', state);
        });
    }
});