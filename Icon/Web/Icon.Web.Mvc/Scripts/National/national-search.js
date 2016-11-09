var hierarcyClassExportList = [];
var hierarchyClassRowsForExpansion = [];
var filledHiearchyList = false;

function filterButtonClick()
{
    if (!filledHiearchyList)
    {
        hierarcyClassExportList = [];

        buildHierarchyClassExportList($('#nationalClassGrid').igGrid('option', 'dataSource').data(), '');
        filledHiearchyList = true;
    }
    var value = $('#filterText').val();
    if (value.length >= 3) {
        filterRows(value);
    }
}
function clearButtonClikc()
{
    $('#filterText').val('');
    collapseRows();
}

function filterRows(searchValue) {
    findRowsToExpand(searchValue);
    expandRows();
    hierarchyClassRowsForExpansion.length = 0;
}

function findRowsToExpand(searchValue) {
    var dataSource = $('#nationalClassGrid').igGrid('option', 'dataSource');
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
    var columns = $("#nationalClassGrid").igGrid("option", "columns");
    $(columns).each(function (index, column) {
        if (column.key === "HierarchyClassName") {
            hierarchyClassNameColumnIndex = index;
            return false;
        }
    });

    // loop through this 4 times since there are 4 levels that could potentially be expanded.
    for (var i = 0; i < 4; i++) {
        var gridRows = $('#nationalClassGrid').igHierarchicalGrid('rootWidget').allRows();
        $(gridRows).each(function (index, row) {
            for (var j = 0; j < hierarchyClassRowsForExpansion.length; j++) {
                if (row.cells[hierarchyClassNameColumnIndex] !== undefined) {
                    var gridRowId = row.cells[hierarchyClassNameColumnIndex].innerHTML;
                    if (gridRowId === hierarchyClassRowsForExpansion[j].toString()) {
                        if (!$("#nationalClassGrid").igHierarchicalGrid("expanded", row)) {
                            $("#nationalClassGrid").igHierarchicalGrid("expand", row);
                        }
                    }
                }
            }
        })
    }
}

function collapseRows() {
    var rowsToCollapse = $("#nationalClassGrid").igHierarchicalGrid("rootWidget").allRows();
    $(rowsToCollapse).each(function (index, element) {
        $("#nationalClassGrid").igHierarchicalGrid("collapse", element);
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
 
                // only include the HierarchyClassId if it's the lowest level of other hierarchies.
                hierarchyClassExportModel.HierarchyClassName += '|' + igGridHierarchyClassData[i].HierarchyClassId;
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
