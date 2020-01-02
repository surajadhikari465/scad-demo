import { validateNumberOfDecimalsForGrid } from './shared.js'

let hasWriteAccess = false;

const searchOperators = {
    ContainsAny: 'ContainsAny',
    ContainsAll: 'ContainsAll',
    ExactlyMatchesAny: 'ExactlyMatchesAny',
    ExactlyMatchesAll: 'ExactlyMatchesAll',
    HasAttribute: 'HasAttribute',
    DoesNotHaveAttribute: 'DoesNotHaveAttribute'
}


window.addEventListener('load', function () {
    const columnsKey = 'columnsKey';
    let utilityFunctions = {

        getFormatters: () => {
            let formatters = [];

            formatters.push({
                key: "BrandsHierarchyClassId",
                formatter: val => utilityFunctions.formatHierarchyColumn(val, "Brands")
            });
            formatters.push({
                key: "MerchandiseHierarchyClassId",
                formatter: val => utilityFunctions.formatHierarchyColumn(val, "Merchandise")
            });
            formatters.push({
                key: "TaxHierarchyClassId",
                formatter: val => utilityFunctions.formatHierarchyColumn(val, "Tax")
            });
            formatters.push({
                key: "FinancialHierarchyClassId",
                formatter: val => utilityFunctions.formatHierarchyColumn(val, "Financial")
            });
            formatters.push({
                key: "NationalHierarchyClassId",
                formatter: val => utilityFunctions.formatHierarchyColumn(val, "National")
            });
            formatters.push({
                key: "ManufacturerHierarchyClassId",
                formatter: val => utilityFunctions.formatHierarchyColumn(val, "Manufacturer")
            });

            return formatters;
        },
        applyFormatters: (columnSettings) => {
            for (let formatterSettings of utilityFunctions.getFormatters()) {
                let column = columnSettings.find(c => c.key === formatterSettings.key);
                if (column) {
                    column.formatter = formatterSettings.formatter;
                }
            }
            return columnSettings;
        },
        getColumnDataType: function (attribute) {
            if (attribute.DataTypeName === 'Number') {
                return 'string';
            } else if (attribute.DataTypeName === 'Text') {
                return 'string';
            } else if (attribute.DataTypeName === 'Boolean') {
                return 'bool';
            } else if (attribute.DataTypeName === 'Date') {
                return 'date';
            } else {
                throw "Unable to create a Column Data Type for attribute: " + attribute.toString();
            }
        },
        getColumnEditorType: function (attribute) {
            if (attribute.IsPickList) {
                return 'combo';
            } else if (attribute.DataTypeName === 'Number') {
                return 'text';
            } else if (attribute.DataTypeName === 'Text') {
                return 'text';
            } else if (attribute.DataTypeName === 'Boolean') {
                return 'checkbox';
            } else if (attribute.DataTypeName === 'Date') {
                return 'date';
            } else {
                throw "Unable to create Grid Editor Type for attribute: " + attribute.toString();
            }
        },
        getColumnEditorOptions: function (attribute) {
            let editorOptions = {

            };
            if (attribute.IsPickList) {
                editorOptions.dataSource = searchViewModel.state.pickListData[attribute.AttributeId];
                editorOptions.textKey = 'PickListValue';
                editorOptions.valueKey = 'PickListValue';
                editorOptions.virtualization = true;
            } else if (attribute.DataTypeName === "Text") {
                editorOptions.maxLength = attribute.MaxLengthAllowed;
                editorOptions.validatorOptions = {
                    pattern: attribute.CharacterSetRegexPattern
                };
            } else if (attribute.DataTypeName === "Number") {
                editorOptions.validatorOptions = {
                    number: true,
                    custom: {
                        method: (value) => validateNumberOfDecimalsForGrid(value, attribute),
                        errorMessage: `A valid number with up to ${attribute.NumberOfDecimals} decimals should be entered`
                    }, valueRange: {
                        min: attribute.MinimumNumber,
                        max: attribute.MaximumNumber
                    }
                };
                if (attribute.MinimumNumber) {
                    editorOptions.minValue = Number.parseFloat(attribute.MinimumNumber);
                }
                if (attribute.MaximumNumber) {
                    editorOptions.maxValue = Number.parseFloat(attribute.MaximumNumber);
                }
                if (attribute.NumberOfDecimals) {
                    editorOptions.maxDecimals = Number.parseInt(attribute.NumberOfDecimals);
                }
            } else if (attribute.DataTypeName === "Date") {
                dataMode: "date"
            }
            return editorOptions;
        },
        formatHierarchyColumn: function (val, hierarchyName) {
            let hierarchyClasses = searchViewModel.state.hierarchyClasses[hierarchyName];
            let hierarchyClass = hierarchyClasses.find(hc => hc.HierarchyClassId === val);
            if (hierarchyClass) {
                return hierarchyClass.HierarchyClassLineage;
            } else {
                return "";
            }
        }
    };
    let searchViewModel = {
        state: {
            attributes: [],
            hierarchyClasses: [],
            merchandiseHierarchyTraits: [],
            pickListData: [],
            hierarchyClassesLoaded: [],
            pickListDataLoaded: [],
            searchParameters: [],
        },
        view: {
            attributesElement: null,
            attributesAddButton: null,
            searchButton: null,
            resetColumnsButton: null,
            igGrid: null
        },
        searchParameterViewModels: [],
        setCheckBoxEnabled: function (e) {
            var checkboxId = e.target.getAttribute('associatedPartialSearch');
            var checkbox = document.getElementById(checkboxId);

            var comboId = e.target.getAttribute('associatedAttributesComboBox');
            var comboBox = document.getElementById(comboId);

            if (comboBox.value === "Scan Code" && e.target.value.toString().includes(" ")) {
                $(checkbox).igCheckboxEditor('disable');
                $(checkbox).igCheckboxEditor('option', 'checked', false);
            }
            else {
                $(checkbox).igCheckboxEditor('enable');
            }
        },
        addAttribute: function (currentListItemNode) {
            let self = this;
            let scanCodeIndex = self.attributeComboBoxOptions.findIndex(o => o.value === "ScanCode");
            let scanCodeComboBoxOption = self.attributeComboBoxOptions.find(o => o.value === "ScanCode");

            let searchParameterViewModel = {
                comboBoxOption: null,
                value: null,
                listItemElement: null,
                attributesComboBoxElement: null,
                attributesSearchValueElement: null,
                searchOperatorValue: searchOperators.ExactlyMatchesAny
            };

            var idSuffix = Math.round(Math.random() * 100000).toString();

            let listItem = document.createElement('li');
            listItem.classList.add('input-group');
            listItem.id = "listItem" + idSuffix;

            let attributesComboBox = document.createElement('input');
            attributesComboBox.id = 'attributesComboBox' + idSuffix;

            let operatorComboBox = document.createElement('input');
            operatorComboBox.id = 'operatorComboBox' + idSuffix;

            let attributesSearchValue = document.createElement('input');
            attributesSearchValue.classList.add("attributes-search-value");
            attributesSearchValue.setAttribute('associatedAttributesComboBox', attributesComboBox.id);
            attributesSearchValue.addEventListener('input', this.setCheckBoxEnabled)
 
            let removeAttributeButton = document.createElement('button');
            removeAttributeButton.id = "removeAttributeButton" + idSuffix;
            removeAttributeButton.classList.add("btn");
            removeAttributeButton.style.backgroundColor = 'white';
            removeAttributeButton.style.padding = '0';
            removeAttributeButton.style.marginLeft = '3px';
            removeAttributeButton.style.color = 'red';
            removeAttributeButton.addEventListener('click', this.removeAttribute.bind(this, listItem.id, idSuffix));
            removeAttributeButton.innerHTML = '<i class="fa fa-minus-circle"></i>';

            let addAttributeButton = document.createElement('button');
            addAttributeButton.id = "addAttributeButton" + idSuffix;
            addAttributeButton.classList.add("btn");
            addAttributeButton.style.backgroundColor = 'white';
            addAttributeButton.style.padding = '0';
            addAttributeButton.style.marginLeft = '5px';
            addAttributeButton.style.color ='green';
            addAttributeButton.addEventListener('click', this.addAttribute.bind(this, listItem));
            addAttributeButton.innerHTML = '<i class="fa fa-plus-circle"></i>';


            listItem.appendChild(attributesComboBox);
            listItem.appendChild(operatorComboBox);
            listItem.appendChild(attributesSearchValue);
            listItem.appendChild(addAttributeButton);
            listItem.appendChild(removeAttributeButton);

            if (currentListItemNode === null) {
                this.view.attributesElement.appendChild(listItem);
            }
            else {
                currentListItemNode.parentNode.insertBefore(listItem, currentListItemNode.nextSibling);
            }

            let operatorComboBoxElement = $(operatorComboBox).igCombo({
                dataSource: self.operatorComboBoxOptions,
                textKey: "displayName",
                valueKey: "value",
                width: "300px",
                initialSelectedItems: [{ value: searchOperators.ExactlyMatchesAny }],
                virtualization: true,
                autoSelectFirstMatch: false,
                mode: "dropdown",
                selectionChanged: function (evt, ui) {
                    searchParameterViewModel.searchOperatorValue = ui.items[0].data.value;
                }
            });


            let attributesComboBoxElement = $(attributesComboBox).igCombo({
                dataSource: self.attributeComboBoxOptions,
                textKey: "displayName",
                valueKey: "value",
                width: "300px",
                initialSelectedItems: [{ index: scanCodeIndex }],
                virtualization: true,
                autoSelectFirstMatch: false,
                mode: "dropdown",
                selectionChanged: function (evt, ui) {
                    if (ui.items && ui.items.length > 0) {
                        let comboBoxOption = ui.items[0].data;
                        let attributesSearchValue = document.createElement('input');
                        attributesSearchValue.classList.add("attributes-search-value");
                        listItem.replaceChild(attributesSearchValue, listItem.children[2]);
                        let attributesSearchValueElement = self.setAttributesSearchValue(comboBoxOption, attributesSearchValue, searchParameterViewModel);
                        searchParameterViewModel.comboBoxOption = comboBoxOption;
                        searchParameterViewModel.attributesSearchValueElement = attributesSearchValueElement;
                        attributesSearchValue.setAttribute('associatedAttributesComboBox', evt.target.id);
                    }
                }
            });
            let attributesSearchValueElement = this.setAttributesSearchValue(scanCodeComboBoxOption, attributesSearchValue, searchParameterViewModel);

            searchParameterViewModel.comboBoxOption = scanCodeComboBoxOption;
            searchParameterViewModel.listItemElement = listItem;
            searchParameterViewModel.attributesComboBoxElement = attributesComboBoxElement;
            searchParameterViewModel.attributesSearchValueElement = attributesSearchValueElement;
            searchParameterViewModel.operatorComboBoxElement = operatorComboBoxElement;
            searchParameterViewModel.id = idSuffix;

            searchParameterViewModel.attributesSearchValueElement.focus();
            this.searchParameterViewModels.push(searchParameterViewModel);
        },
        setAttributesSearchValue: function (comboBoxOption, attributesSearchValue, searchParameterViewModel) {
            return  $(attributesSearchValue).igTextEditor({
                inputName: comboBoxOption.value,
                width: "300px",
                valueChanged: function (evt, ui) {
                    searchParameterViewModel.value = ui.newValue;
                }
            });
        },
        removeAttribute: function (controlId, searchParameterId) {
            let attributeElements = this.view.attributesElement.children;
            if (attributeElements.length > 1) {

                let element = document.getElementById(controlId);
                this.view.attributesElement.removeChild(element);
                this.searchParameterViewModels = this.searchParameterViewModels.filter(spvm => spvm.id != searchParameterId);
            }
        },
        search: function () {
            $('#searchError').hide();
            $('#resetColumnsButton').show();
            $('#searchError').html('');
            $("#selectedCount")[0].innerHTML = '';
            $("#ExportLink").css("display", "block");

            if ($("#grid").igGrid() != undefined) {
                $("#grid").igGrid('destroy');
            }

            let searchParameters = {
                GetItemsAttributesParameters: []
            };

            for (let searchParameter of this.searchParameterViewModels) {
                searchParameters.GetItemsAttributesParameters.push({
                    AttributeName: searchParameter.comboBoxOption.value,
                    AttributeValue: searchParameter.value,
                    SearchOperator: searchParameter.searchOperatorValue
                });
            }
            let self = this;
            let checkBoxProcessor = function () {
                var checkedRows = $("#grid").igGrid("selectedRows");

                if (checkedRows.length > 0) {
                    $("#selectedExportRowsAllColumns")[0].classList.remove("disabled");
                    $("#selectedExportRowsSelectedColumns")[0].classList.remove("disabled");
                    $("#selectedCount")[0].innerHTML = `${checkedRows.length} Row${checkedRows.length > 1 ? 's' : ''} Selected`;
                }
                else {
                    $("#selectedExportRowsAllColumns")[0].classList.add("disabled");
                    $("#selectedExportRowsSelectedColumns")[0].classList.add("disabled");
                    $("#selectedCount")[0].innerHTML = '';
                }
            }
            $.post(window.location.origin + '/Item/SaveGetItemsParameters', searchParameters)
                .done(function () {
                    let columnSettings = self.getColumns();
                    let igGrid = $("#grid").igGrid({
                        primaryKey: "ItemId",
                        autoGenerateColumns: false,
                        width: "100%",
                        dataSource: window.location.origin + '/Item/GridDataSource',
                        updateUrl: window.location.origin + '/Item/GridUpdate',
                        autoCommit: false,
                        aggregateTransactions: true,
                        fixedHeaders: false,
                        responseDataKey: "Records",
                        columns: columnSettings,
                        requestError: function (evt, ui) {
                            $('#searchError').show();
                            $('#searchError').html('An error occurred. Please try to narrow your search.');
                            if ($("#grid").igGrid() != undefined) {
                                $("#grid").igGrid('destroy');
                          
                            }
                            $('#resetColumnsButton').hide();
                            $('#ExportLink').hide();
                            
                        },
                        features: [
                            {
                                name: 'RowSelectors',
                                enableCheckBoxes: true,
                                enableSelectAllForPaging: false,
                                rowSelectorClicked: checkBoxProcessor,
                                checkBoxStateChanged: checkBoxProcessor
                            },
                            {
                                name: "Selection",
                                type: "row",
                                multipleCellSelectOnClick: false,
                                multipleSelection: true,
                                rowSelectionChanged: checkBoxProcessor
                            }, {
                                name: "Paging",
                                type: "remote",
                                pageSize: 10,
                                recordCountKey: "TotalRecordsCount"
                            }, {
                                name: "Sorting",
                                type: "remote"
                            }, {
                                name: "ColumnMoving",
                                movingDialogHeight: 600,
                                columnMovingDialogContainment: "window",
                                columnMoved: (evt, ui) => self.saveColumnSettings(ui.owner.grid.options.columns)
                            }, {
                                name: "Hiding",
                                columnChooserHeight: 600,
                                columnChooserContainment: "window",
                                columnHidden: (evt, ui) => self.saveColumnSettings(ui.owner.grid.options.columns),
                                columnShown: (evt, ui) => self.saveColumnSettings(ui.owner.grid.options.columns)
                            }, {
                                name: "Resizing",
                                deferredResizing: true,
                                columnResized: (evt, ui) => self.saveColumnSettings(ui.owner.grid.options.columns)
                            }, {
                                name: "Updating",
                                enableAddRow: false,
                                enableDeleteRow: false,
                                editMode: hasWriteAccess ? 'row' : 'none',
                                editRowEnding: function (evt, ui) {

                                    let merchandiseHierarchyClassId = ui.values["MerchandiseHierarchyClassId"];
                                    let hierarchyTraits = searchViewModel.merchandiseHierarchyTraits.find(x => x.HierarchyClassId == merchandiseHierarchyClassId);

                                    if (hierarchyTraits.ProhibitDiscount != null) {
                                        ui.values["ProhibitDiscount"] = hierarchyTraits.ProhibitDiscount;
                                    }

                                    if (hierarchyTraits.FinancialHierarchyClassId != null) {
                                        ui.values["FinancialHierarchyClassId"] = hierarchyTraits.FinancialHierarchyClassId;
                                    }

                                    if (hierarchyTraits.ItemType != null) {
                                        ui.values["ItemTypeDescription"] = hierarchyTraits.ItemType;
                                    }

                                    if (evt.key === "Enter" || $(evt.toElement).attr("data-localeid") === "doneLabel"
                                        || $(evt.toElement).attr("data-localeid") === "cancelLabel"
                                        || (evt.key === undefined && evt.toElement === undefined)) {
                                        return true;
                                    }
                                    return false;
                                },
                                editRowEnded: function (evt, ui) {
                                    if (ui.update) {
                                        self.view.igGrid.igGrid("saveChanges", function () {
                                            console.log("Updated item successfully.");
                                        }, function (jqXHR, textStatus, errorThrown) {
                                            console.log("Error occurred when updating the item.");
                                            console.log(jqXHR.responsJSON);
                                            console.log(textStatus);
                                            console.log(errorThrown);
                                            alert("Error occurred when updating the item.");
                                        });
                                    }
                                },
                                columnSettings: (function () {
                                    let columnSettings = [];

                                    columnSettings.push({
                                        columnKey: "Actions",
                                        editorType: "text",
                                        allowHidden: true,
                                        readOnly: true
                                    });


                                    columnSettings.push({
                                        columnKey: "ItemId",
                                        editorType: "number",
                                        required: true,
                                        readOnly: true
                                    });
                                    columnSettings.push({
                                        columnKey: "ItemTypeDescription",
                                        editorType: "text",
                                        required: true,
                                        readOnly: true
                                    });
                                    columnSettings.push({
                                        columnKey: "ScanCode",
                                        editorType: "text",
                                        required: true,
                                        readOnly: true
                                    });
                                    columnSettings.push({
                                        columnKey: "BarcodeType",
                                        editorType: "text",
                                        required: true,
                                        readOnly: true
                                    });
                                    columnSettings.push({
                                        columnKey: "MerchandiseHierarchyClassId",
                                        editorType: "combo",
                                        required: true,
                                        editorOptions: {
                                            dataSource: self.state.hierarchyClasses["Merchandise"],
                                            textKey: 'HierarchyClassLineage',
                                            valueKey: 'HierarchyClassId',
                                            virtualization: true
                                        }
                                    });
                                    columnSettings.push({
                                        columnKey: "BrandsHierarchyClassId",
                                        editorType: "combo",
                                        required: true,
                                        editorOptions: {
                                            dataSource: self.state.hierarchyClasses["Brands"],
                                            textKey: 'HierarchyClassLineage',
                                            valueKey: 'HierarchyClassId',
                                            virtualization: true
                                        }
                                    });
                                    columnSettings.push({
                                        columnKey: "NationalHierarchyClassId",
                                        editorType: "combo",
                                        required: true,
                                        editorOptions: {
                                            dataSource: self.state.hierarchyClasses["National"],
                                            textKey: 'HierarchyClassLineage',
                                            valueKey: 'HierarchyClassId',
                                            virtualization: true
                                        }
                                    });
                                    columnSettings.push({
                                        columnKey: "FinancialHierarchyClassId",
                                        editorType: "combo",
                                        required: true,
                                        readOnly: true,
                                        editorOptions: {
                                            dataSource: self.state.hierarchyClasses["Financial"],
                                            textKey: 'HierarchyClassLineage',
                                            valueKey: 'HierarchyClassId',
                                            virtualization: true
                                        }
                                    });
                                    columnSettings.push({
                                        columnKey: "TaxHierarchyClassId",
                                        editorType: "combo",
                                        required: true,
                                        editorOptions: {
                                            dataSource: self.state.hierarchyClasses["Tax"],
                                            textKey: 'HierarchyClassLineage',
                                            valueKey: 'HierarchyClassId',
                                            virtualization: true
                                        }
                                    });
                                    columnSettings.push({
                                        columnKey: "ManufacturerHierarchyClassId",
                                        editorType: "combo",
                                        editorOptions: {
                                            dataSource: self.state.hierarchyClasses["Manufacturer"],
                                            textKey: 'HierarchyClassLineage',
                                            valueKey: 'HierarchyClassId',
                                            virtualization: true
                                        }
                                    });
                                    self.state.attributes.forEach(function (attribute) {
                                        columnSettings.push({
                                            columnKey: attribute.AttributeName,
                                            editorType: utilityFunctions.getColumnEditorType(attribute),
                                            editorOptions: utilityFunctions.getColumnEditorOptions(attribute),
                                            required: attribute.DataTypeName === 'Boolean' ? false : attribute.IsRequired,
                                            readOnly: attribute.IsReadOnly
                                        });
                                    });
                                    return columnSettings;
                                })()
                            }
                        ]
                    });
                    self.view.igGrid = igGrid;
                    if (self.view.resetColumnsButton.hidden) {
                        self.view.resetColumnsButton.hidden = false;
                    }
                    self.saveColumnSettings($("#grid").igGrid("option", "columns"));
                })
                .fail(function (data) {
                    console.log(data);
                });
        },
        getColumns: function () {
            if (localStorage.getItem(columnsKey)) {
                let columnSettings = JSON.parse(localStorage.getItem(columnsKey));
                for (let attribute of this.state.attributes.sort(this.compareAttributesByDisplayOrder)) {
                    if (!columnSettings.some(c => c.key === attribute.AttributeName)) {
                            columnSettings.push({
                                headerText: attribute.DisplayName,
                                key: attribute.AttributeName,
                                dataType: utilityFunctions.getColumnDataType(attribute),
                                width: attribute.GridColumnWidth
                            });
                    }
                }
                return utilityFunctions.applyFormatters(columnSettings);
            } else {
                let columnSettings = [];

                if (hasWriteAccess) {
                    columnSettings.push({
                        headerText: "Actions",
                        key: "Actions",
                        width: "200px",
                        template: "<a href='/Item/Edit?scanCode=${ScanCode}'>Edit</a> | <a href='/Item/Detail?scanCode=${ScanCode}'>View</a>",
                        unbound: true,
                        allowHidden: true
                    });
                }
                else {
                    columnSettings.push({
                        headerText: "Actions",
                        key: "Actions",
                        width: "200px",
                        template: "<a href='/Item/Detail?scanCode=${ScanCode}'>View</a>",
                        unbound: true,
                        allowHidden: true
                    });
                }
                columnSettings.push({
                    headerText: "Item ID",
                    key: "ItemId",
                    dataType: "number",
                    width: "200px"
                });
                columnSettings.push({
                    headerText: "Item Type",
                    key: "ItemTypeDescription",
                    dataType: "text",
                    width: "200px"
                });
                columnSettings.push({
                    headerText: "Scan Code",
                    key: "ScanCode",
                    dataType: "text",
                    width: "200px",
                    template: "<a href='Item/Detail?scanCode=${ScanCode}'>${ScanCode}</a>"
                });
                columnSettings.push({
                    headerText: "Barcode Type",
                    key: "BarcodeType",
                    dataType: "text",
                    width: "200px"
                });
                columnSettings.push({
                    headerText: "Brands",
                    key: "BrandsHierarchyClassId",
                    dataType: "text",
                    width: "200px"
                });
                columnSettings.push({
                    headerText: "Merchandise",
                    key: "MerchandiseHierarchyClassId",
                    dataType: "text",
                    width: "200px"
                });
                columnSettings.push({
                    headerText: "Tax",
                    key: "TaxHierarchyClassId",
                    dataType: "number",
                    width: "200px"
                });
                columnSettings.push({
                    headerText: "Financial",
                    key: "FinancialHierarchyClassId",
                    dataType: "number",
                    width: "200px"
                });
                columnSettings.push({
                    headerText: "National",
                    key: "NationalHierarchyClassId",
                    dataType: "number",
                    width: "200px"
                });
                columnSettings.push({
                    headerText: "Manufacturer",
                    key: "ManufacturerHierarchyClassId",
                    dataType: "number",
                    width: "200px"
                });

            for(let attribute of this.state.attributes.sort(this.compareAttributesByDisplayOrder)) {
                if (attribute.DataTypeName === 'Date') {
                    columnSettings.push({
                        headerText: attribute.DisplayName,
                        key: attribute.AttributeName,
                        dataType: "date",
                        dateDisplayType: "local",
                        enableUTCDates: true,
                        format: "yyyy-MM-dd",
                        width: attribute.GridColumnWidth
                    });
                } else {
                    columnSettings.push({
                        headerText: attribute.DisplayName,
                        key: attribute.AttributeName,
                        dataType: utilityFunctions.getColumnDataType(attribute),
                        width: attribute.GridColumnWidth
                    });
                }
            }
                return utilityFunctions.applyFormatters(columnSettings);
            }
        },
        saveColumnSettings: function (columns) {
            let columnsJson = JSON.stringify(columns);
            window.localStorage.setItem(columnsKey, columnsJson);
            this.setSession();
        },
        setSession: () => {
            let selectedColumnNames;

            if (localStorage.getItem(columnsKey) != null) {
                let columnSettings = JSON.parse(localStorage.getItem(columnsKey));
                columnSettings = columnSettings.filter(a => a.hidden == false);
                selectedColumnNames = columnSettings.map(a => a.key);
            }
            else {
                selectedColumnNames = null;
            }

            $.post(window.location.origin + '/Item/ItemSearchExport', { selectedColumnNames: selectedColumnNames });
        },
        compareAttributesByName: function (a1, a2) {
            let a1DisplayName = a1.DisplayName.toLowerCase();
            let a2DisplayName = a2.DisplayName.toLowerCase();

            if (a1DisplayName > a2DisplayName) {
                return 1;
            }
            else if (a1DisplayName < a2DisplayName) {
                return -1;
            }
            else {
                return 0;
            }
        },
        exportSelectedRows: function (exportAllAttributes) {
            var checkedRows = $("#grid").igGrid("selectedRows");

            $.ajax({
                url: window.location.origin + '/Item/ExportSelectedItems',
                type: 'POST',
                data: { selectedIds: checkedRows.map(cr => cr.id) },
                success: function (response) { //return the download link
                    window.location.href = window.location.origin + '/Item/ExportSelectedItems?exportAllAttributes=' + exportAllAttributes;
                },
            });
        },
        selectedRowAllColumnsExportClick: function () {
            searchViewModel.exportSelectedRows(true);
        },
        selectedRowCurrentColumnsExportClick: function () {
            searchViewModel.exportSelectedRows(false);
        },
        compareAttributesByDisplayOrder: function (a1, a2) {
            if (a1.DisplayOrder > a2.DisplayOrder) {
                return 1;
            }
            else if (a1.DisplayOrder < a2.DisplayOrder) {
                return -1;
            }
            else {
                return 0;
            }
        },
    
        loadPickListData: function () {
            return new Promise((resolve, reject) => {
                let self = searchViewModel;
                self.state.attributes.forEach((attribute) => {
                    self.state.pickListData[attribute.AttributeId] = attribute.PickListData;
                    self.state.pickListDataLoaded[attribute.AttributeId] = true;
                });
                resolve();
            });
        },
        attributeComboBoxOptions:[],
        operatorComboBoxOptions: [{
            displayName: "Contains(ANY)",
            value: searchOperators.ContainsAny
        },
        {
            displayName: "Contains(ALL)",
            value: searchOperators.ContainsAll
        },
        {
            displayName: "Exactly Matches(ANY)",
            value: searchOperators.ExactlyMatchesAny
        },
        {
            displayName: "Exactly Matches(ALL)",
            value: searchOperators.ExactlyMatchesAll
        },
        {
            displayName: "Has Attribute",
            value: searchOperators.HasAttribute
        },
        {
            displayName: "Does Not Have Attribute",
            value: searchOperators.DoesNotHaveAttribute
        }],
        createAttributeComboBoxOptions: function () {
            this.attributeComboBoxOptions = [{
                displayName: "Edit",
                value: "Edit"
            }, {
                displayName: "Item ID",
                value: "ItemId"
            }, {
                displayName: "Item Type",
                value: "ItemTypeDescription"
            }, {
                displayName: "Scan Code",
                value: "ScanCode"
            }, {
                displayName: "Barcode Type",
                value: "BarcodeType"
            }, {
                displayName: "Merchandise",
                value: "Merchandise",
                isHierarchy: true,
                hierarchyName: "Merchandise"
            }, {
                displayName: "Brands",
                value: "Brands",
                isHierarchy: true,
                hierarchyName: "Brands"
            }, {
                displayName: "Tax",
                value: "Tax",
                isHierarchy: true,
                hierarchyName: "Tax"
            }, {
                displayName: "National",
                value: "National",
                isHierarchy: true,
                hierarchyName: "National"
            }, {
                displayName: "Financial",
                value: "Financial",
                isHierarchy: true,
                hierarchyName: "Financial"
            }, {
                displayName: "Manufacturer",
                value: "Manufacturer",
                isHierarchy: true,
                hierarchyName: "Manufacturer"
            }].concat(this.state.attributes.map(a => {
                return {
                    displayName: a.DisplayName,
                    value: a.AttributeName,
                    attribute: a
                }
            }));
            this.attributeComboBoxOptions.sort((o1, o2) => {
                let o1DisplayName = o1.displayName.toLowerCase();
                let o2DisplayName = o2.displayName.toLowerCase();

                if (o1DisplayName > o2DisplayName)
                    return 1;
                else if (o1DisplayName < o2DisplayName)
                    return -1;
                else
                    return 0;
            });
        },
        resetColumns: function (){
            this.view.igGrid.igGrid("destroy");
            localStorage.removeItem(columnsKey);
            this.search();
        },
        init: function (attributes) {
            try {
                $("#selectedExportRowsAllColumns")[0].addEventListener('click', this.selectedRowAllColumnsExportClick);
                $("#selectedExportRowsSelectedColumns")[0].addEventListener('click', this.selectedRowCurrentColumnsExportClick);
                this.state.attributes = attributes.sort(this.compareAttributesByName);
                this.createAttributeComboBoxOptions();
                this.setSession();
                this.loadPickListData(this.state.attributes)
                    .then(() => {
                        this.view.attributesElement = document.getElementById('attributes');
                        this.view.searchButton = document.getElementById('searchButton');
                        this.view.searchButton.addEventListener('click', this.search.bind(this));
                        this.view.resetColumnsButton = document.getElementById("resetColumnsButton");
                        this.view.resetColumnsButton.addEventListener("click", this.resetColumns.bind(this));
                        this.addAttribute(null);
                    });
            } catch (error) {
                $("#alerts").empty();
                $("#alerts").append('<div id="alert" class="alert alert-danger alert-dismissable">' +
                    '<button type="button" class="close" data-dismiss="alert">&times;</button >' +
                    '<strong>An unexpected error occurred. Please reach out to support team to resolve issue.</strong>' +
                    '</div >').show();
                console.log(error);
            }
        }
    }

    let loadHierarchies = ()  => {
        return new Promise((resolve, reject) => {
            let self = searchViewModel;

            $.ajax({
                url: window.location.origin + '/Hierarchy/All',
                dataType: 'json',
                async: true
            })
                .done(function (data) {
                    let promises = [];
                    data.forEach(hierarchy => {
                        if (hierarchy.HierarchyName === 'Certification Agency Management' ||
                            hierarchy.HierarchyName === 'Browsing') {
                            return;
                        }
                        self.state.hierarchyClassesLoaded[hierarchy.HierarchyName] = false;

                    });
                    data.forEach(hierarchy => {

                        if (hierarchy.HierarchyName === 'Certification Agency Management' ||
                            hierarchy.HierarchyName === 'Browsing') {
                            return;
                        }

                        let filters = "HierarchyClassId,HierarchyClassLineage,HierarchyClassName";

                        var promise = new Promise((resolve, reject) => {
                            $.ajax({
                                url: window.location.origin +
                                    '/HierarchyClass/ByHierarchyId?hierarchyId=' +
                                    hierarchy.HierarchyId +
                                    "&columnFilters=" +
                                    filters,
                                dataType: 'json',
                                async: true
                            })
                                .done(function (data) {
                                    self.state.hierarchyClasses[hierarchy.HierarchyName] = data;
                                    self.state.hierarchyClassesLoaded[hierarchy.HierarchyName] = true;
                                    resolve();
                                }).fail(function (data) {
                                    console.log(data);
                                    reject();
                                });
                        });

                        promises.push(promise);
                    });
                    Promise.all(promises).then(() => { resolve(); });
                });
        });
    }

    let enableSearchIfAllDataIsLoaded = () => {
        let loading = document.getElementById("loading");
        let searchTools = document.getElementById("searchtools");
        let searchButton = document.getElementById("searchButton");
        searchButton.disabled = false;
        loading.classList.add("d-none");
        searchTools.classList.remove("d-none");

        // when the page loads there should only be one element (scan code) on the page with this class. 
        let scanCodeElement = document.getElementsByClassName("attributes-search-value");
        if (scanCodeElement != null && scanCodeElement != undefined && scanCodeElement.length > 0) {
            scanCodeElement[0].focus();
        }
        console.log('enableSearchIfAllDataIsLoaded');
    }

    let loadWriteAccess = () => {
        $.getJSON(window.location.origin + '/Item/HasWriteAccess')
            .done(function (data) {
                hasWriteAccess = data;
            }).fail(function (data) {
                console.log('fail');
                console.log(data);
            });
    }

    let loadAttributes = () => {
        $.getJSON(window.location.origin + '/Attribute/All')
            .done(function (data) {
                searchViewModel.init(data.Attributes);
                window.searchViewModel = searchViewModel;
            }).fail(function (data) {
                console.log('fail');
                console.log(data);
            });
    }

    let loadMerchandiseHierarchyTraits = () => {
        $.getJSON(window.location.origin + '/HierarchyClass/MerchandiseHierarchyTraits')
            .done(function (data) {
                searchViewModel.merchandiseHierarchyTraits = data;
            }).fail(function (data) {
                console.log('fail');
                console.log(data);
            });
    }

    loadHierarchies()
        .then(() => loadAttributes())
        .then(() => loadMerchandiseHierarchyTraits())
        .then(() => loadWriteAccess())
        .then(() => enableSearchIfAllDataIsLoaded());

    $(document).keydown(function (e) {
        if (e.which == 13) {
            document.getElementById('searchButton').click();
        }
    });
});


