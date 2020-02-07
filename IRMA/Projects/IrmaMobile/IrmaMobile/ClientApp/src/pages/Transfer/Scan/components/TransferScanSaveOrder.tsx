import React, { useEffect } from 'react'
import { useHistory } from 'react-router-dom';

const TransferScanSaveOrder: React.FC = () => {
    // @ts-ignore
    let history = useHistory();

    useEffect(() => {
        history.push('/transfer/index/true');
    }, [history])
    
    return (
        <div>
            
        </div>
    )
}

export default TransferScanSaveOrder;