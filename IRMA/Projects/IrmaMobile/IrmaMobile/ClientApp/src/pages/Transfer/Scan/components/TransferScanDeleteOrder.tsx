import React, { useEffect, useState, Fragment } from 'react'
import { useHistory } from 'react-router-dom';
import BasicModal from '../../../../layout/BasicModal';

const TransferScanDeleteOrder: React.FC = () => {
    // @ts-ignore
    const [alert, setAlert] = useState<any>({ open: false, alertMessage: '', type: 'confirm', header: 'IRMA Mobile' });
    let history = useHistory();

    useEffect(() => {
        setAlert({
            ...alert,
            open: true,
            alertMessage: 'Would you like to delete the current order?',
            confirmAction: handleConfirmClick,
            cancelAction: handleCancelClick
        });
    }, [])

    const handleConfirmClick = () => {
        setAlert({
            ...alert,
            open: false,
        });
        localStorage.removeItem('transferData');
        history.push('/transfer/index/true');
    };
    const handleCancelClick = () => {
        setAlert({
            ...alert,
            open: false,
        });
        history.goBack();
    };

    return (
        <Fragment>
            <BasicModal alert={alert} setAlert={setAlert}></BasicModal>
        </Fragment>
    )
}

export default TransferScanDeleteOrder;