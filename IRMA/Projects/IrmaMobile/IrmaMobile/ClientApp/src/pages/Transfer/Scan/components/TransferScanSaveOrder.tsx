import React, { useEffect } from 'react'
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify'

const TransferScanSaveOrder: React.FC = () => {
    // @ts-ignore
    let history = useHistory();

    useEffect(() => {
        history.push('/transfer/index/true');
        toast.success('Order saved.');
    }, [history])

    return (
        <div></div>
    )
}

export default TransferScanSaveOrder;