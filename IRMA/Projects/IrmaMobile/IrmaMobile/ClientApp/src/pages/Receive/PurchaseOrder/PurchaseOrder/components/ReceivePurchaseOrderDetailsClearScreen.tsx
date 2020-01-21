import React, { useEffect, useContext } from 'react'
import { types, AppContext } from '../../../../../store';
import { useHistory } from 'react-router-dom';

const ReceivePurchaseOrderDetailsClearScreen: React.FC = () => {
    // @ts-ignore
    const { dispatch } = useContext(AppContext);
    let history = useHistory();

    useEffect(() => {
        dispatch({ type: types.SETPURCHASEORDERUPC, purchaseOrderUpc: '' });
        dispatch({ type: types.SETORDERDETAILS, orderDetails: null });
        dispatch({ type: types.SETPURCHASEORDERNUMBER, purchaseOrderNumber: '' });

        history.goBack();
    }, [dispatch, history])
    
    return (
        <div>
            
        </div>
    )
}

export default ReceivePurchaseOrderDetailsClearScreen;