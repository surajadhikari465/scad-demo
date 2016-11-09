var hierarcyClassExportList = [];
var hierarchyClassRowsForExpansion = [];

function searchButtonClick() {

    $('#search-button').val('Loading...');
    $('#search-button').disable(true);

    // Prevent the search button from being clicked like a link.
    $('body').on('click', 'a.disabled', function (event) {
        event.preventDefault();
    });
}

function hierarchySearchSuccess() {

    $('#search-button').val('Display');
    $('#search-button').disable(false);
    $('#export').click(function () {
        hierarcyClassExportList = [];

      

        var hierarchyName = $('#hierarchyName').html();
        if (hierarchyName === 'Merchandise')
        {
            buildMerchandiseHierarchyClassExportList($('#hierarchyGrid').igGrid('option', 'dataSource').data(), '');
        }
        else
        {
            buildHierarchyClassExportList($('#hierarchyGrid').igGrid('option', 'dataSource').data(), '');
        }
        var hierarchyClassExportData = { hierarchyName: hierarchyName, hierarchyClasses: hierarcyClassExportList };

        $.fileDownload('Excel/HierarchyExport', {
            httpMethod: "POST",
            data: hierarchyClassExportData
        });
    });

    $('#filterButton').click(function () {
        var value = $('#filterText').val();
        if (value.length >= 3) {
            filterRows(value);
        }
    });

    $('#filterClear').click(function () {
        $('#filterText').val('');
        collapseRows();
    });

    $('#filterText').keyup(function (event) {
        if (event.keyCode == 13) {
            $('#filterButton').click();
        }
    });
}

function filterRows(searchValue) {
    findRowsToExpand(searchValue);
    expandRows();
    hierarchyClassRowsForExpansion.length = 0;
}

function findRowsToExpand(searchValue) {
    var dataSource = $('#hierarchyGrid').igGrid('option', 'dataSource');
    $(dataSource.data()).each(function (index, record) {
        searchDataSourceRecursively(searchValue, record);
    });
}

function searchDataSourceRecursively(searchValue, record) {
    var matched = false;
    if (record.HierarchyClassName.toLowerCase().indexOf(searchValue.toLowerCase()) > -1) {
        matched = true;
    }
    if (record.HierarchySubClasses !== null && record.HierarchySubClasses !== undefined) {
        $(record.HierarchySubClasses.Records).each(function (subIndex, subRecord) {
            if (searchDataSourceRecursively(searchValue, subRecord) === true) {
                matched = true;
            }
        });
    }
    if (matched) {
        hierarchyClassRowsForExpansion.push(record.HierarchyClassName);
    }
    return matched;
}

function expandRows() {
    collapseRows();

    // get column index of HierarchyClassID
    var hierarchyClassNameColumnIndex;
    var columns = $("#hierarchyGrid").igGrid("option", "columns");
    $(columns).each(function (index, column) {
        if (column.key === "HierarchyClassName") {
            hierarchyClassNameColumnIndex = index;
            return false;
        }
    });

    // loop through this 4 times since there are 4 levels that could potentially be expanded.
    for (var i = 0; i < 4; i++) {
        var gridRows = $('#hierarchyGrid').igHierarchicalGrid('rootWidget').allRows();
        $(gridRows).each(function (index, row) {
            for (var j = 0; j < hierarchyClassRowsForExpansion.length; j++) {
                if (row.cells[hierarchyClassNameColumnIndex] !== undefined) {
                    var gridRowId = row.cells[hierarchyClassNameColumnIndex].innerHTML;
                    if (gridRowId === hierarchyClassRowsForExpansion[j].toString()) {
                        if (!$("#hierarchyGrid").igHierarchicalGrid("expanded", row)) {
                            $("#hierarchyGrid").igHierarchicalGrid("expand", row);
                        }
                    }
                }
            }
        })
    }
}

function collapseRows() {
    var rowsToCollapse = $("#hierarchyGrid").igHierarchicalGrid("rootWidget").allRows();
    $(rowsToCollapse).each(function (index, element) {
        $("#hierarchyGrid").igHierarchicalGrid("collapse", element);
    });
}

function buildHierarchyClassExportList(igGridHierarchyClassData, parentHierarchyClassName) {
    var hierarchyName = $('#hierarchyName').text();

    for (var i = 0; i < igGridHierarchyClassData.length; i++) {
        var hierarchyClassExportModel = {
            HierarchyClassName: parentHierarchyClassName + igGridHierarchyClassData[i].HierarchyClassName
        };

        if (igGridHierarchyClassData[i].SubTeam !== undefined) {
            hierarchyClassExportModel.HierarchyClassName += ': ' + igGridHierarchyClassData[i].SubTeam;
        }

        hierarcyClassExportList.push(hierarchyClassExportModel);

        if (igGridHierarchyClassData[i].HierarchySubClasses !== undefined &&
            igGridHierarchyClassData[i].HierarchySubClasses !== null &&
            igGridHierarchyClassData[i].HierarchySubClasses.Records !== undefined &&
            igGridHierarchyClassData[i].HierarchySubClasses.Records !== null &&
            igGridHierarchyClassData[i].HierarchySubClasses.Records.length !== 0) {
            buildHierarchyClassExportList(igGridHierarchyClassData[i].HierarchySubClasses.Records, hierarchyClassExportModel.HierarchyClassName + '|');
        }
        else {
            if (hierarchyName === 'Merchandise') {

                // include subbrick code and hierarchyClassId if it's the lowest level of the Merchandising hierarchy.
                hierarchyClassExportModel.HierarchyClassName += '|' + igGridHierarchyClassData[i].SubBrickCode + '|' + igGridHierarchyClassData[i].HierarchyClassId;
            }
            else {

                // only include the HierarchyClassId if it's the lowest level of other hierarchies.
                hierarchyClassExportModel.HierarchyClassName += '|' + igGridHierarchyClassData[i].HierarchyClassId;
            }
        }
    }
}


function buildMerchandiseHierarchyClassExportList(igGridHierarchyClassData, parentHierarchyClassName) {
    var hierarchyName = $('#hierarchyName').text();

    for (var i = 0; i < igGridHierarchyClassData.length; i++) {
        var hierarchyClassExportModel = {
            HierarchyClassName: parentHierarchyClassName + igGridHierarchyClassData[i].HierarchyClassName
        };
        
        

        if (igGridHierarchyClassData[i].HierarchySubClasses !== undefined &&
            igGridHierarchyClassData[i].HierarchySubClasses !== null &&
            igGridHierarchyClassData[i].HierarchySubClasses.Records !== undefined &&
            igGridHierarchyClassData[i].HierarchySubClasses.Records !== null &&
            igGridHierarchyClassData[i].HierarchySubClasses.Records.length !== 0)
        {
            buildMerchandiseHierarchyClassExportList(igGridHierarchyClassData[i].HierarchySubClasses.Records, hierarchyClassExportModel.HierarchyClassName + '|');
        }
        else
        {
            if (hierarchyName === 'Merchandise')
            {
                if (igGridHierarchyClassData[i].SubTeam !== undefined) {
                    hierarchyClassExportModel.HierarchyClassName += ': ' + igGridHierarchyClassData[i].SubTeam;
                }

                // include subbrick code and hierarchyClassId if it's the lowest level of the Merchandising hierarchy.
                hierarchyClassExportModel.HierarchyClassName += '|' + igGridHierarchyClassData[i].SubBrickCode + '|' + igGridHierarchyClassData[i].HierarchyClassId;
                hierarcyClassExportList.push(hierarchyClassExportModel);
            }
           
        }
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
