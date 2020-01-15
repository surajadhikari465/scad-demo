import React, { Fragment, useEffect, useState, useContext } from 'react'
import CurrentLocation from '../../../layout/CurrentLocation'
import { Form } from 'semantic-ui-react'
import agent from '../../../api/agent';
import { AppContext } from "../../../store";
import LoadingComponent from '../../../layout/LoadingComponent';
import { WfmButton } from '@wfm/ui-react';
import { toast } from 'react-toastify';

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
                    
                    setVendors(vendorsRaw.map((vendor: any)  => { return { key: vendor.vendorID, text: vendor.vendorName, value: vendor.vendorID }}))
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
        { key: 0, text: 'Invoice', value: 0 },
        { key: 1, text: 'Credit', value: 1 }
    ]

    return (
        <Fragment>
            { isLoading ? <LoadingComponent content='Loading vendors...' /> :
            <Fragment>
                <CurrentLocation />
                <Form style={{marginTop: '15px', padding: '5px'}}>
                    <Form.Dropdown label='DSD Vendors' clearable fluid selection placeholder='Select DSD Vendor...' options={vendors} />
                    <Form.Input label='Invoice #' placeholder='Invoice #'></Form.Input>
                    <Form.Dropdown label='Invoice/Credit' clearable selection placeholder='Select Type...' options={invoiceCreditOptions} />
                    <span style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                        <WfmButton>Receive Item</WfmButton>
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