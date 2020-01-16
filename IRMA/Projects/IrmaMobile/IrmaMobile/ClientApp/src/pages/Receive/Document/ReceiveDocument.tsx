import React, { Fragment, useEffect, useState, useContext } from 'react'
import CurrentLocation from '../../../layout/CurrentLocation'
import { Form, DropdownProps, InputOnChangeData } from 'semantic-ui-react'
import agent from '../../../api/agent';
import { AppContext } from "../../../store";
import LoadingComponent from '../../../layout/LoadingComponent';
import { WfmButton } from '@wfm/ui-react';
import { toast } from 'react-toastify';
// import { useHistory } from 'react-router-dom';

const ReceiveDocument: React.FC = () => {
    interface IMappedVendor {
        key: number;
        text: string;
        value: string;
    }

    // @ts-ignore
    const { state } = useContext(AppContext);
    const { storeNumber, region } = state;
    const [vendors, setVendors] = useState<IMappedVendor[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [selectedVendor, setSelectedVendor] = useState<string>();
    const [invoiceNumber, setInvoiceNumber] = useState<string>('');
    const [creditChoice, setCreditChoice] = useState<string>();
    
    // let history = useHistory();

    useEffect(() => {
        const loadVendors = async () => {
            try {
                setIsLoading(true);
                const vendorsRaw = await agent.Document.getVendors(region, parseInt(storeNumber));

                if(vendorsRaw) {
                    if(vendorsRaw.length === 0) {
                        toast.error('This store does not have any Receiving Document vendors.', { autoClose: false });
                        return;
                    }
                    
                    setVendors(vendorsRaw.map((vendor: any)  => { return { key: vendor.vendorID, text: vendor.vendorName, value: vendor.vendorID.toString() }}))
                }
            }
            finally {
                setIsLoading(false);
            }
        }

        if(vendors.length === 0) {
            loadVendors();
        }
    }, [vendors, region, storeNumber])

    const invoiceCreditOptions = [
        { key: 0, text: 'Invoice', value: '0' },
        { key: 1, text: 'Credit', value: '1' }
    ]

    const receiveItemClick = async () => {
        if(!selectedVendor) {
            toast.error('Please select a vendor before creating the Receiving Document.', { autoClose: false });
            return;
        } else if ( !creditChoice ) {
            toast.error('Please choose invoice or credit before continuing.', { autoClose: false });
            return;
        } else if ( invoiceNumber === '' ) {
            toast.error('Please enter in an invoice number.', { autoClose: false });
            return;
        }

        const result = await agent.Document.isDuplicateReceivingDocumentInvoiceNumber(region, invoiceNumber, parseInt(selectedVendor));

        if(result) {
            toast.error(`Invoice number ${invoiceNumber} has already been used for this vendor.  Please update the invoice number and try again.`, { autoClose: false });
            return;
        }

        // history.push("/receive/Document/scan");
    }

    const handleVendorChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data.value === undefined) {
            return;
        }
        
        setSelectedVendor(data.value.toString());
    }

    const handleInvoiceNumberChange = (event: React.ChangeEvent<HTMLInputElement>, data: InputOnChangeData) => {
        setInvoiceNumber(data.value.toString());
    }

    const handleCreditChange = (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
        if(data.value === undefined) {
            return;
        }
        
        setCreditChoice(data.value.toString()); 
    }

    return (
        <Fragment>
            { isLoading ? <LoadingComponent content='Loading vendors...' /> :
            <Fragment>
                <CurrentLocation />
                <Form style={{marginTop: '15px', padding: '5px'}}>
                    <Form.Dropdown value={selectedVendor} label='DSD Vendors' clearable fluid selection placeholder='Select DSD Vendor...' options={vendors} onChange={handleVendorChange} />
                    <Form.Input value={invoiceNumber} label='Invoice #' placeholder='Invoice #' onChange={handleInvoiceNumberChange}></Form.Input>
                    <Form.Dropdown value={creditChoice} label='Invoice/Credit' clearable selection placeholder='Select Type...' options={invoiceCreditOptions} onChange={handleCreditChange} />
                    <span style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                        <WfmButton onClick={receiveItemClick}>Receive Item</WfmButton>
                    </span>
                    <span style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                        <WfmButton style={{marginTop: '20px'}}>View Session</WfmButton>
                    </span>
                </Form>
            </Fragment>
            }
        </Fragment>
    )
}

export default ReceiveDocument