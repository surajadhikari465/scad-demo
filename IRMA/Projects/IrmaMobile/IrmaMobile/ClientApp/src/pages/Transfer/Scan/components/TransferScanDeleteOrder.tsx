import React, { useEffect, useContext } from 'react'
import { AppContext } from '../../../../store';
import { useHistory } from 'react-router-dom';

const TransferScanDeleteOrder: React.FC = () => {
    // @ts-ignore
    const { dispatch } = useContext(AppContext);
    let history = useHistory();

    useEffect(() => {
        localStorage.removeItem('transferData');
        history.push('/transfer/index/true');
    }, [dispatch, history])
    
    return (
        <div>
            
        </div>
    )
}

export default TransferScanDeleteOrder;