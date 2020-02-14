import React, { Fragment, useState, useContext, useEffect } from 'react'
import ReactTable from 'react-table'
import agent from '../../../../../api/agent';
import { AppContext } from '../../../../../store';
import { Modal, Button } from 'semantic-ui-react';

interface IProps {
    handleSelectRefuse: (code: string, id: number) => any;
}

const InvoiceDataRefuseCodes: React.FC<IProps> = ({ handleSelectRefuse }) => {
    const { state } = useContext(AppContext);
    const { region, orderDetails } = state;

    const [data, setData] = useState([]);
    const [open, setOpen] = useState<boolean>(false);

    let refuseDisabled = orderDetails?.OrderItems && orderDetails?.OrderItems.filter((item)=>item.QtyReceived !== 0).length > 0 ? true: false; 

    useEffect(() => {
        const getData = async () => {
            const rawData = await agent.InvoiceData.getRefuseCodes(region);

            setData(rawData && rawData.map((reasonCode: any) => { 
                return {
                    code: reasonCode.reasonCodeAbbreviation, 
                    description: reasonCode.reasonCodeDescription,
                    value: reasonCode.reasonCodeID
                }}));
        }

        getData();
    }, [region])

    const columns = React.useMemo(
        () => [
          {
            Header: 'Code',
            accessor: 'code',
            width: 50
          },
          {
            Header: 'Description',
            accessor: "description",
            style: { 'whiteSpace': 'unset' }
          }
        ],
        []
      )
    
    return (
        <Fragment>
            <Modal open={open} closeIcon onClose={() => setOpen(false)} trigger={<Button onClick={() => setOpen(true)} disabled={refuseDisabled} style={{margin: '10px', backgroundColor:'lightgreen'}}>Refuse Order</Button>}>
                <Modal.Header>Choose Reason Code</Modal.Header>
                <Modal.Content>
                    <ReactTable
                            data={data}
                            columns={columns}
                            showPagination={false}
                            style={{
                                height: "480px" 
                            }}
                            className="-striped -highlight"
                            minRows={0}
                            getTrProps={(state:any, rowInfo: any) => ({
                                onClick:() => {
                                    setOpen(false);
                                    handleSelectRefuse(rowInfo.original.code, rowInfo.original.value)
                                }
                              })}
                        />
                </Modal.Content>
            </Modal>
        </Fragment>
    )
}

export default InvoiceDataRefuseCodes;