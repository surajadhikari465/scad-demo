import { WfmButton } from '@wfm/ui-react';
import React, { Fragment, SyntheticEvent, useEffect, useState } from 'react';
import { Dropdown, DropdownProps, Modal } from 'semantic-ui-react';
import Charge from '../../types/Charge';

interface IProps {
    charges: Charge[];
    disabled: boolean;
    handleRemoveCharge: (chargeId: number) => any;
}

const InvoiceDataRemoveCharge: React.FC<IProps> = ({ charges, disabled, handleRemoveCharge }) => {
    const [open, setOpen] = useState<boolean>(false);
    const [options, setOptions] = useState<any>([]);
    const [selectedCharge, setSelectedCharge] = useState(0);

    useEffect(() => {
        if(charges && charges.length > 0) {
            setOptions(charges.map(ch => {return {key: ch.ChargeId, value: ch.ChargeId, text: `${ch.Description}: ${ch.ChargeValue}`}}))
        }
    }, [charges])

    const handleChargeChange = (event: SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        setSelectedCharge((data && data.value && parseInt(data.value.toString())) || 0);
    }

    const handleClose = () => {
        setOpen(false);
        setSelectedCharge(0);
    }

    const handleOkClick = () => {
        handleRemoveCharge(selectedCharge);
        setOpen(false);
    }

    return (
        <Fragment>
            <Modal open={open} trigger={<WfmButton disabled={disabled} onClick={() => {setOpen(true)}}>Remove</WfmButton>}>
                <Modal.Header>Remove Invoice Charge</Modal.Header>
                <Modal.Content>
                    <Dropdown clearable fluid selection options={options} value={selectedCharge} onChange={handleChargeChange} placeholder='Select Charge'/>
                </Modal.Content>
                <Modal.Actions>
                    <WfmButton disabled={selectedCharge === 0} style={{marginRight: '20px'}} onClick={handleOkClick}>OK</WfmButton>
                    <WfmButton onClick={handleClose}>Cancel</WfmButton>
                </Modal.Actions>
            </Modal>
        </Fragment>
    )
}

export default InvoiceDataRemoveCharge;