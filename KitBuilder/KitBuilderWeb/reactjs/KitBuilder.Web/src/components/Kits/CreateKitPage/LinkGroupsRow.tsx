import * as React from 'react'
import { Grid, Button, FormHelperText, Divider } from '@material-ui/core';
import { Delete } from '@material-ui/icons'
import { LinkGroup, LinkGroupItem } from '../../../types/LinkGroup'
import CollapseBar from 'src/components/PageStyle/CollapseBar';
import { AgGridReact } from '@ag-grid-community/react';
import { AllCommunityModules } from '@ag-grid-community/all-modules';
import '@ag-grid-community/all-modules/dist/styles/ag-grid.css';
import '@ag-grid-community/all-modules/dist/styles/ag-theme-balham.css';
import Tooltip from '@material-ui/core/Tooltip';

interface LinkGroupsRowProps {
    error?: any,
    linkedGroup: LinkGroup,
    onItemSelected: (item: LinkGroupItem) => void;
    onItemUnselected: (item: LinkGroupItem) => void;
    onLinkedGroupDeleted: (linkedGroup: LinkGroup) => void;
    selectedLinkedGroupItems: Array<LinkGroupItem>;
    disabledLinkGroups: Array<any>;
}

export default function LinkedGroupsRow(props: LinkGroupsRowProps) {
    var columnDefs = [
        {
            headerName: "",
            headerCheckboxSelection: true,
            headerCheckboxSelectionFilteredOnly: true,
            checkboxSelection: true,
            width: 50,
            valueGetter: function (params: any) {
                if (props.selectedLinkedGroupItems.some(i => i.linkGroupItemId === params.node.data.linkGroupItemId)) {
                    params.node.setSelected(true);
                }
            }
        },
        {
            headerName: "Scan Code", field: "item.scanCode", sortable: true, filter: true,
            rowDrag: true, cellStyle: { textAlign: 'center' }, tooltipField: 'item.scanCode'
        },
        {
            headerName: "Product Desc", field: "item.productDesc", sortable: true, filter: true, editable: false, cellStyle: { textAlign: 'center' },
            tooltipField: 'item.productDesc', width: 300
        },
        {
            headerName: "Item Size", field: "item.retailSize", sortable: true, filter: true, editable: false, cellStyle: { textAlign: 'center' },
            tooltipField: 'item.retailSize'
        },
        {
            headerName: "Item UOM", field: "item.retailUOM", sortable: true, filter: true, editable: false, cellStyle: { textAlign: 'center' },
            tooltipField: 'item.retailUOM'
        }
    ]

    var isRowSelectable = function (rowNode: any) {
        if (props.disabledLinkGroups.includes(rowNode.data.linkGroupId)) {
            return false;
        }
        else {
            return true;
        }
    }

    var rowClassRules = function (params: any) {
        if (params.node.rowIndex % 2 === 0) {
            return { background: 'white' }
        }
        else
            return { background: '#f8f9fa' }
    }

    var onGridReady = (params: any) => {
        params.api.sizeColumnsToFit();
    }

    var onRowSelected = (params: any) => {
        if (params.node.selected) {
            props.onItemSelected(params.node.data);
        } else {
            props.onItemUnselected(params.node.data);
        }
    }

    const [isCollapsed, setIsCollapsed] = React.useState<boolean>(false);

    // @ts-ignore
    const allItems = <React.Fragment>
        <Grid item xs={12}>
            <div style={{ width: "100%", height: "100%" }}>
                <div style={{ display: "flex", flexDirection: "row" }}>
                    <div style={{ flexGrow: 1 }}>
                        <div
                            className="ag-theme-balham"
                            style={{
                                height: '200px',
                                width: '100%'
                            }}
                        >
                            <AgGridReact
                                columnDefs={columnDefs}
                                rowData={props.linkedGroup.linkGroupItemDto}
                                multiSortKey="ctrl"
                                onGridReady={onGridReady}
                                rowDragManaged={true}
                                getRowStyle={rowClassRules}
                                isRowSelectable={isRowSelectable}
                                rowSelection='multiple'
                                colResizeDefault='shift'
                                suppressRowClickSelection = {true}
                                onRowSelected={onRowSelected}
                                modules={AllCommunityModules}>

                            </AgGridReact>
                        </div>
                    </div> </div>
            </div>
        </Grid>
    </React.Fragment>;

    return <React.Fragment>
        <Grid container justify="space-between" className="pb-3">
            <Grid item xs={12}>
                <h6>{props.linkedGroup.groupName}</h6>
            </Grid>
            <Grid item>

                <Tooltip title={props.selectedLinkedGroupItems.filter(f => f.linkGroupId == props.linkedGroup.linkGroupId).map(function (element: any) { if (element.item != null) { return <div style={{ color: 'white' }}>{element.item.scanCode + '(' + element.item.productDesc + ')'} </div> } else { return '' }; })} placement="right">
                    <Button> Number of modifiers selected: {' ' + props.selectedLinkedGroupItems.filter(f => f.linkGroupId == props.linkedGroup.linkGroupId).length}</Button>
                </Tooltip>
            </Grid>
            <Grid container justify="flex-end">
                <Button disabled={props.disabledLinkGroups.includes(props.linkedGroup.linkGroupId)} color="secondary" onClick={() => props.onLinkedGroupDeleted(props.linkedGroup)}>
                    <Delete />
                </Button>
            </Grid>
            <Divider style={{ width: "100%" }} />
            <Grid item xs={12}>
                <CollapseBar collapsed={isCollapsed} onOpen={() => setIsCollapsed(false)} onClose={() => setIsCollapsed(true)} />
            </Grid>
        </Grid>
        {!isCollapsed && <Grid container >
            {allItems}
            {props.error ? <Grid item xs={12}><FormHelperText className="px-0" error={true}>{props.error.error}</FormHelperText></Grid> : null}
        </Grid>}
    </React.Fragment>
}