import React, { useState, Fragment, useContext } from 'react'
import { AppContext, } from '../../../store'
import { Grid, Input, Button, Segment } from 'semantic-ui-react';
import ITransferItem from '../types/ITransferItem';
import ReasonCodeModal from '../../../layout/ReasonCodeModal';
import { useHistory } from "react-router-dom";

const TransferLineItemDetails: React.FC = () => {
    //@ts-ignore
    const { state } = useContext(AppContext);
    const [transferLineItem] = useState<ITransferItem>(state.transferLineItem!);
    const [unitCost] = useState<number>(transferLineItem.VendorCost !== 0 ? transferLineItem.VendorCost : transferLineItem.AdjustedCost);
    let history = useHistory();
    const handleCancelClick = () => {
        history.push('/transfer/review');
    };

    return (
        <Fragment>
            <Fragment>
                <ReasonCodeModal />
                <Grid style={{ marginTop: '5px' }}>
                    <Grid.Row>
                        <Grid.Column width={4} verticalAlign='middle' textAlign='right'>
                            UPC:
                        </Grid.Column>
                        <Grid.Column width={10} style={{ padding: '0px' }}>
                            <Input disabled placeholder='UPC' value={transferLineItem.Upc} fluid />
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                            Desc:
                        </Grid.Column>
                        <Grid.Column width={12} style={{ padding: '0px' }}>
                            {transferLineItem.Description}
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                            Quantity:
                        </Grid.Column>
                        <Grid.Column width={3} style={{ padding: '0px' }}>
                            <Input disabled type='number' placeholder='Qty' value={transferLineItem.Quantity} fluid />
                        </Grid.Column>
                        <Grid.Column width={2} verticalAlign='middle' textAlign='right'>
                            Retail:
                        </Grid.Column>
                        <Grid.Column width={2} verticalAlign='middle' textAlign='left' style={{ fontWeight: 'bold' }}>
                            {transferLineItem.RetailUom}
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                            Vendor Pack:
                        </Grid.Column>
                        <Grid.Column width={10} verticalAlign='middle' textAlign='left' style={{ fontWeight: 'bold', padding: '0px' }}>
                            {transferLineItem.VendorPack}
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column verticalAlign='middle' textAlign='right' width={4}>
                            Vendor Cost:
                        </Grid.Column>
                        <Grid.Column width={4} verticalAlign='middle' textAlign='left' style={{ fontWeight: 'bold', paddingLeft: '0px' }}>
                            <Input disabled fluid value={unitCost} />
                        </Grid.Column>
                    </Grid.Row>
                    <Grid.Row>
                        <Grid.Column width={8} verticalAlign='middle' textAlign='left' style={{ fontWeight: 'bold' }}>
                            <Segment color='grey' inverted>Queued: {transferLineItem.Quantity}</Segment>
                        </Grid.Column>
                    </Grid.Row>
                </Grid>
                <span style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                    <Button className='wfmButton' style={{ width: '100%', marginTop: '50px' }} onClick={handleCancelClick}>Cancel</Button>
                </span>
            </Fragment>
        </Fragment>
    )
}

export default TransferLineItemDetails;