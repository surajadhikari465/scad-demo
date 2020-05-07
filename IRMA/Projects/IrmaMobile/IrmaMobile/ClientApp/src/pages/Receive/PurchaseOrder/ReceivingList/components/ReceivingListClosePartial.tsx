import React, { Fragment, useState, useContext } from 'react'
import ConfirmModal from '../../../../../layout/ConfirmModal'
import { useHistory, RouteComponentProps } from 'react-router-dom';
import LoadingComponent from '../../../../../layout/LoadingComponent';
import { toast } from 'react-toastify';
import agent from '../../../../../api/agent';
import Config from '../../../../../config';
import { AppContext, types } from '../../../../../store';

interface RouteParams {
    purchaseOrderNumber: string;
}

interface IProps extends RouteComponentProps<RouteParams> {}

const ReceivingListClosePartial: React.FC<IProps> = ({ match }) => {
    let history = useHistory();
    //@ts-ignore
    const { state, dispatch } = useContext(AppContext);
    const { region, orderDetails, user } = state;
    const [open, setOpen] = useState<boolean>(true);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const handleClosePartial = async () => {
        try {
            if(!orderDetails) {
                toast.error('orderDetails is not defined', { autoClose: false });
                history.goBack();
                return;
            }

            setIsLoading(true);

            
            var updateResult = await agent.InvoiceData.updateOrderBeforeClosing(
                region, 
                orderDetails.OrderId,
                '', 
                undefined, 
                0,
                '',
                undefined,
                orderDetails.SubteamNo,
                true);

            if((updateResult && !updateResult.status) || !updateResult) {
                toast.error(`Error when updating order before closing: ${(updateResult && updateResult.errorMessage) || 'No message given'}`, { autoClose: false })
                return;
            }

            var result = await agent.InvoiceData.closeOrder(region, orderDetails.OrderId, user!.userId)

            if(result && result.status) {
                toast.info('Order Partially Closed', { autoClose: false });

                dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
                dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });
                dispatch({ type: types.SETORDERDETAILS, orderDetails: null });

                history.push("/receive/PurchaseOrder");
            } else {
                toast.error(`Error when closing order: ${(result && result.errorMessage) || 'No message given'}`, { autoClose: false })
            }
        }
        finally {
            setIsLoading(false);
        }
    }

    const handleCancelClick = () => {
        history.goBack();
    }

    return (
        <Fragment>
            { isLoading ? <LoadingComponent content='Partially closing order...' /> :
            <ConfirmModal handleConfirmClose={handleClosePartial} setOpenExternal={setOpen} showTriggerButton={false} 
                                openExternal={open} headerText='Receiving List' cancelButtonText='No' confirmButtonText='Yes' 
                                lineOne={'Close this order as a Partial Shipment?'} handleCancelClose={handleCancelClick}/> 
            }
        </Fragment>
    )
}

export default ReceivingListClosePartial;