import React, { Fragment, useState, useEffect, useContext, FormEvent, SyntheticEvent, ChangeEvent, useCallback } from 'react'
import { Modal, Dropdown, Form, CheckboxProps, DropdownProps, Divider, Input, Container, InputOnChangeData, Button, Label } from 'semantic-ui-react'
import agent from '../../../../../api/agent'
import { AppContext } from '../../../../../store'
import LoadingComponent from '../../../../../layout/LoadingComponent'
import useNumberInput from '../../../../../hooks/useNumberInput'

interface IProps {
    orderId: number;
    handleAddCharge: (sacType: number, description: string, subteamGLAccount: number, amount: number, isAllowance: boolean) => any;
    disabled: boolean;
}

const InvoiceDataAddCharge: React.FC<IProps> = ({ orderId, handleAddCharge, disabled }) => {
    enum radioOptions {
        Allocated,
        Nonallocated,
        None
    }

    const [open, setOpen] = useState<boolean>(false);
    const [options, setOptions] = useState([]);
    const [allocatedOptions, setAllocatedOptions] = useState([]);
    const [allocatedCharges, setAllocatedCharges] = useState<any>([]);
    const [nonAllocatedOptions, setNonallocatedOptions] = useState([]);
    const [nonAllocatedCharges, setNonallocatedCharges] = useState<any>([]);
    const [radioSelection, setRadioSelection] = useState<radioOptions>(radioOptions.None);
    const [isLoading, setIsLoading] = useState<boolean>(true);

    const [charge, setCharge] = useState<string>("");

    const { state } = useContext(AppContext);
    const { region } = state;

    const { value, setValue, valueInput, setValueInput, allowDecimals, setAllowDecimals, allowNegativeNumbers, setAllowNegativeNumbers, errorMessage, setErrorMessage } =
        useNumberInput(0, '', true, false, '');

    const loadCharges = useCallback(async () => {
        try {
            setIsLoading(true);
            const allocatedCharges = await agent.InvoiceData.getAllocatedInvoiceCharges(region);
            if (allocatedCharges) {
                setAllocatedCharges(allocatedCharges);
                setAllocatedOptions(allocatedCharges.map((charge: any) => {
                    return { key: charge.elementName, value: charge.elementName, text: charge.elementName }
                }))
            }

            const nonAllocatedCharges = await agent.InvoiceData.getNonallocatedInvoiceCharges(region, orderId);
            if (nonAllocatedCharges) {
                setNonallocatedCharges(nonAllocatedCharges);
                setNonallocatedOptions(nonAllocatedCharges.map((charge: any) => {
                    return { key: charge.subteamNo, value: charge.subteamName, text: charge.subteamName }
                }))
            }
        }
        finally {
            setIsLoading(false);
        }
    }, [orderId, region]);

    useEffect(() => {
        loadCharges();
    }, [loadCharges])

    const loadDropdown = (val: radioOptions) => {
        switch (val) {
            case radioOptions.Allocated:
                setOptions(allocatedOptions);
                break;
            case radioOptions.Nonallocated:
                setOptions(nonAllocatedOptions);
                break;
            case radioOptions.None:
                setOptions([]);
                break
            default:
                console.error("Unknown radio option: " + val);
        }
    }

    const handleChange = (event: FormEvent<HTMLInputElement>, data: CheckboxProps) => {
        let val: radioOptions;
        switch (data.value) {
            case 0:
                val = radioOptions.Allocated;
                break;
            case 1:
                val = radioOptions.Nonallocated;
                break;
            case 2:
                val = radioOptions.None;
                break;
            default:
                console.error("Unknown radio option: " + data.value);
                return;
        }

        setRadioSelection(val);
        loadDropdown(val);
        setAllowNegativeNumbers(val === radioOptions.Nonallocated);
        setCharge('');
    };

    const handleClose = () => {
        setOpen(false);
        setRadioSelection(radioOptions.None)
        setValue(0);
        setValueInput('');
        setCharge('');
        setErrorMessage('');
    }

    const handleChargeChange = (event: SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        setCharge(data.value ? data.value.toString() : '');
    }

    const handleOkClick = () => {
        const sacType = radioSelection === radioOptions.Allocated ? 1 : 2
        const description = charge;
        const subteamGLAccount = radioSelection === radioOptions.Nonallocated ? nonAllocatedCharges.filter((ch: any) => ch.subteamName === charge)[0].subteamNo : -1
        var allowance = false;

        if (radioSelection === radioOptions.Allocated && allocatedCharges) {
            const allowanceIndicator = allocatedCharges.filter((ch: any) => ch.elementName === charge)[0].isAllowance;

            if (allowanceIndicator === '-1') {
                allowance = true;
            }
        }

        handleAddCharge(sacType, description, subteamGLAccount, (allowance ? value * -1 : value), allowance);
        handleClose();
    }

    const handleAmountChanged = (e: React.ChangeEvent<HTMLInputElement>, data: InputOnChangeData) => {
        setValueInput(e.currentTarget.value);
    }

    return (
        <Fragment>
            {open && isLoading ?
                <LoadingComponent content="Loading types..." /> :
                <Modal closeOnDimmerClick={false} onClose={handleClose} open={open} trigger={<button className="irma-btn" disabled={disabled} onClick={() => { setOpen(true) }}>Add</button>}>
                    <Modal.Header>Add Invoice Charge</Modal.Header>
                    <Modal.Content>
                        <Form style={{ position: 'relative', left: '15%' }}>
                            <Form.Group inline style={{ marginBottom: '10px' }}>
                                <Form.Radio
                                    label="Allocated"
                                    value={radioOptions.Allocated}
                                    checked={radioSelection === radioOptions.Allocated}
                                    onChange={handleChange}
                                />
                                <Form.Radio
                                    label="Non-Allocated"
                                    value={radioOptions.Nonallocated}
                                    checked={radioSelection === radioOptions.Nonallocated}
                                    onChange={handleChange}
                                />
                            </Form.Group>
                        </Form>
                        <Divider />
                        <Dropdown disabled={radioSelection === radioOptions.None} clearable fluid selection options={options} value={charge} onChange={handleChargeChange} placeholder='Select Charge' />
                        <Divider />
                        <Container textAlign='right'>
                            <Form onSubmit={handleOkClick}>
                                <Input type='number'
                                    step="any"
                                    placeholder='Value'
                                    value={valueInput}
                                    onChange={handleAmountChanged}
                                    disabled={radioSelection === radioOptions.None || charge === ''} />
                                <div className='error-message'>{errorMessage !== '' ? errorMessage : ''}</div>
                            </Form>
                        </Container>
                    </Modal.Content>
                    <Modal.Actions>
                        <Button className='wfmButton' style={{ marginRight: '10px', width: '84px' }} disabled={radioSelection === radioOptions.None || charge === '' || value === 0 || errorMessage !== ''} onClick={handleOkClick}>
                            OK
                        </Button>
                        <Button className='wfmButton' style={{ width: '84px' }} onClick={handleClose}>
                            Cancel
                        </Button>
                    </Modal.Actions>
                </Modal>
            }
        </Fragment>
    )
}

export default InvoiceDataAddCharge;