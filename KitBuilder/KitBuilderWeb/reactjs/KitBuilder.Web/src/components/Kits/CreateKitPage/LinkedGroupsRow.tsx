import * as React from 'react'
import { Grid, Button, Checkbox } from '@material-ui/core';
import { LinkedGroup, LinkedGroupItem } from '../../../types/LinkGroup'

interface LinkedGroupsRowProps {
    linkedGroup: LinkedGroup,
    onItemSelected: (item: LinkedGroupItem) => void;
    onItemUnselected: (item: LinkedGroupItem) => void;
    onLinkedGroupDeleted: (linkedGroup: LinkedGroup) => void;
    selectedLinkedGroupItems: Array<LinkedGroupItem>;
}

export default function LinkedGroupsRow(props: LinkedGroupsRowProps) {

    const isItemSelected = (item: LinkedGroupItem) => {
        return props.selectedLinkedGroupItems.includes(item);
    }

    const handleSelectionChanged = (item: LinkedGroupItem, checked: boolean) => {
        if(checked) {
            props.onItemSelected(item);
        } else {
            props.onItemUnselected(item);
        }
    }

    const allItems = props.linkedGroup.linkGroupItemDto.map(item => 
    <Grid item xs={6} md={4} key = {item.linkGroupItemId}>
        <Checkbox className = "item-checkbox" checked={isItemSelected(item)} onChange={(e, checked) => handleSelectionChanged(item, checked)}/> 
         {/*
         //@ts-ignore TODO: fix uppercase json problem */}
         {item.item.productDesc} 
    </Grid>
    );

    return <React.Fragment>
        
        <Grid container justify="space-between">
            <Grid item>
                <h6>{props.linkedGroup.groupName}</h6>
            </Grid>
            <Grid>
                <Button color="secondary" onClick={() => props.onLinkedGroupDeleted(props.linkedGroup)}>
                    Remove
                </Button>
            </Grid>
        </Grid>
        <Grid container >
            {allItems}
        </Grid>
    </React.Fragment>
}