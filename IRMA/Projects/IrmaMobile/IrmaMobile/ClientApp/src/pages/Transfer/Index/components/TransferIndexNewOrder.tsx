import React, { useEffect, useContext } from 'react'
import { AppContext } from '../../../../store';
import { useHistory } from 'react-router-dom';

const TransferIndexNewOrder: React.FC = () => {
    // @ts-ignore
    const { dispatch } = useContext(AppContext);
    let history = useHistory();

    useEffect(() => {
        history.goBack();
    }, [dispatch, history])
    
    return (
        <div>
            
        </div>
    )
}

export default TransferIndexNewOrder;