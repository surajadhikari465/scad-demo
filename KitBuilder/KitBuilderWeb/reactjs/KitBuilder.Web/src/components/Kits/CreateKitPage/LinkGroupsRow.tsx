import * as React from 'react'
import { Grid, Button, Checkbox, FormHelperText, Divider } from '@material-ui/core';
import { Delete } from '@material-ui/icons'
import { LinkGroup, LinkGroupItem } from '../../../types/LinkGroup'
import CollapseBar from 'src/components/PageStyle/CollapseBar';

interface LinkGroupsRowProps {
    error?: any,
    linkedGroup: LinkGroup,
    onItemSelected: (item: LinkGroupItem) => void;
    onItemUnselected: (item: LinkGroupItem) => void;
    onLinkedGroupDeleted: (linkedGroup: LinkGroup) => void;
    selectedLinkedGroupItems: Array<LinkGroupItem>;
}

export default function LinkedGroupsRow(props: LinkGroupsRowProps) {

    const [ isCollapsed, setIsCollapsed ] = React.useState<boolean>(false);

    const isItemSelected = (item: LinkGroupItem) => {
        return props.selectedLinkedGroupItems.some(i => i.linkGroupItemId === item.linkGroupItemId);
    }

    const handleSelectionChanged = (item: LinkGroupItem, checked: boolean) => {
        if(checked) {
                props.onItemSelected(item);
            } else {
                props.onItemUnselected(item);
            }
    }
// @ts-ignore
    const allItems = props.linkedGroup.linkGroupItemDto.map(item => 
    <Grid item xs={6} md={4} key = {item.linkGroupItemId}>
        <Checkbox className = "item-checkbox" checked={isItemSelected(item)} onChange={(e, checked) => handleSelectionChanged(item, checked)}/> 
         {item.item.productDesc} 
    </Grid>
);

    return <React.Fragment>
        
        <Grid container justify="space-between" className="pb-3">
            <Grid item>
                <h6>{props.linkedGroup.groupName}</h6>
            </Grid>
            <Grid>
                <Button color="secondary" onClick={() => props.onLinkedGroupDeleted(props.linkedGroup)}>
                    <Delete/>
                </Button>
            </Grid>
            <Divider style={{width: "100%"}}/>
            <Grid item xs={12}>
            <CollapseBar collapsed = {isCollapsed} onOpen={() => setIsCollapsed(false)} onClose = {() => setIsCollapsed(true)}/>
            </Grid>
        </Grid>
        {!isCollapsed && <Grid container >
            {allItems}
            {props.error ? <Grid item xs={12}><FormHelperText className="px-0" error={true}>{props.error.error}</FormHelperText></Grid> : null}
        </Grid>}
    </React.Fragment>
}