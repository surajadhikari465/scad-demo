import React, { Fragment, useState, useContext, useEffect } from "react";
import { AppContext, types } from "../../../store";
import { Grid } from "semantic-ui-react";
import dateFormat from "dateformat";
import { toast } from "react-toastify";
import agent from "../../../api/agent";
import ITransferData from "../types/ITransferData";
import { useHistory } from "react-router-dom";
import ReactTable from "react-table";
import CurrentLocation from "../../../layout/CurrentLocation";
import ITransferItem from "../types/ITransferItem";
import ITransferOrderItem from "../types/ITransferOrderItem";
import LoadingComponent from "../../../layout/LoadingComponent";
import BasicModal from "../../../layout/BasicModal";
import ITransferOrder from "../types/ITransferOrder";
import '../styles.scss';
import { WfmButton } from "@wfm/ui-react";

const TransferReview: React.FC = () => {
  //@ts-ignore
  const { state, dispatch } = useContext(AppContext);
  const { region, user } = state;
  const [transferData, setTransferData] = useState<ITransferData>(JSON.parse(localStorage.getItem("transferData")!));
  const [selectedTransferItem, setSelectedTransferItem] = useState<ITransferItem>();
  const [data, setData] = useState<ITransferItem[]>(transferData.Items);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [alert, setAlert] = useState<any>({ open: false, alertMessage: '', type: 'default', header: 'IRMA Mobile', confirmAction: () => { }, cancelAction: () => { } });
  let history = useHistory();

  useEffect(() => {
    dispatch({ type: types.SETTITLE, Title: "Review Transfer" });
    return () => {
      dispatch({ type: types.SETTITLE, Title: "IRMA Mobile" });
    };
  }, [dispatch]);

  const select = (rowInfo: ITransferItem) => {
    setSelectedTransferItem(rowInfo);
  };

  const handleViewDetailsClick = () => {
    if (selectedTransferItem) {
      dispatch({ type: types.SETTRANSFERLINEITEM, transferLineItem: selectedTransferItem });
      history.push("/transfer/viewDetails");
    }
  };

  const handleUpdateClick = () => {
    if (selectedTransferItem) {
      const unit = selectedTransferItem.SoldByWeight ? 'weight' : 'quantity';
      setAlert({
        ...alert,
        open: true,
        alertMessage: `Do you want to update the ${unit} for the selected UPC?`,
        type: 'confirm',
        confirmAction: () => {
          setAlert({ ...alert, open: false });
          dispatch({ type: types.SETTRANSFERDATA, transferData: transferData });
          dispatch({ type: types.SETTRANSFERLINEITEM, transferLineItem: selectedTransferItem });
          history.push("/transfer/update");
        },
        cancelAction: () => { setAlert({ ...alert, open: false }); }
      });
    }
  };

  const handleRemoveClick = () => {
    if (selectedTransferItem && transferData) {
      setAlert({
        ...alert,
        open: true,
        alertMessage: 'Do you want to delete the selected UPC?',
        type: 'confirm',
        confirmAction: () => {
          const removeItem = transferData.Items.find(i => i.Upc === selectedTransferItem?.Upc);
          const newTransferData = { ...transferData, Items: transferData.Items.filter(i => i.Upc !== removeItem?.Upc) };
          setTransferData(newTransferData);
          setSelectedTransferItem(undefined);

          setData(newTransferData.Items);
          localStorage.setItem("transferData", JSON.stringify(newTransferData));
          setAlert({ ...alert, open: false });
        },
        cancelAction: () => { setAlert({ ...alert, open: false }); }
      });
    }
  };

  const handleAddLineItemClick = () => {
    history.push("/transfer/scan");
  };

  const handleSendClick = async () => {
    setIsLoading(true);
    let transferOrder = {
      CreatedBy: user!.userId,
      ProductTypeId: transferData.ProductType,
      OrderTypeId: 3,
      VendorId: transferData.FromStoreVendorId,
      TransferSubTeamNo: transferData.FromSubteamNo,
      ReceiveLocationId: transferData.ToStoreNo,
      PurchaseLocationId: transferData.ToStoreNo,
      FaxOrder: false,
      ExpectedDate: transferData.ExpectedDate,
      ReturnOrder: false,
      FromQueue: false,
      DsdOrder: false,
      OrderItems: transferData.Items.map<ITransferOrderItem>(i => ({
        QuantityOrdered: i.Quantity,
        ItemKey: parseFloat(i.ItemKey),
        QuantityUnit: i.RetailUomId,
        AdjustedCost: i.AdjustedCost,
        ReasonCodeDetailId: i.AdjustedReason
      }))
    } as ITransferOrder

    if (transferData.ProductType === 3) {
      transferOrder.TransferToSubTeam = transferData.SupplyType;
      transferOrder.SupplyTransferToSubTeam = transferData.ToSubteamNo;
    } else {
      transferOrder.TransferToSubTeam = transferData.ToSubteamNo;
      transferOrder.SupplyTransferToSubTeam = 0;
    }

    try {
      console.log(transferOrder);
      let result = await agent.Transfer.createTransferOrder(region, transferOrder);
      setIsLoading(false);
      
      let po = result.irmaPoNumber;      
      toast.success(`PO# ${po} created successfully.`, { autoClose: false });
      localStorage.removeItem('transferData');
      history.push('/transfer/index/false');
    } catch (error) {
      setIsLoading(false);
      toast.error('Error sending transfer order.', { autoClose: false });
    }
  };

  const columns = React.useMemo(
    () => [
      {
        Header: "UPC",
        accessor: "Upc"
      },
      {
        Header: "QTY",
        accessor: "Quantity",
        width: 40
      },
      {
        Header: "DESC",
        accessor: "Description"
      },
      {
        Header: "TOT_COST",
        accessor: "TotalCost"
      }
    ],
    []
  );

  if (isLoading) {
    return (
      <Fragment><LoadingComponent content="Sending Transfer Order" /></Fragment>
    );
  } else {
    return (
      <Fragment>
        <CurrentLocation />
        <Grid style={{ marginTop: "20px", marginLeft: "5px", marginRight: "5px" }} >
          <Grid.Row style={{ paddingTop: "0px" }}>
            <Grid columns={1}>
              <Grid.Row style={{ padding: "0px" }}>From: {transferData?.FromStoreName}</Grid.Row>
              <Grid.Row style={{ padding: "0px" }}>{transferData?.FromSubteamName}</Grid.Row>
              <Grid.Row style={{ padding: "0px" }}>To: {transferData?.ToStoreName}</Grid.Row>
              <Grid.Row style={{ padding: "0px" }}>{transferData?.ToSubteamName}</Grid.Row>
              <Grid.Row style={{ padding: "0px" }}>Expected Date: {dateFormat(transferData?.ExpectedDate, "UTC:yyyy-mm-dd")}</Grid.Row>
            </Grid>
          </Grid.Row>
          <Grid.Row style={{ paddingTop: "0px", paddingBottom: "0px" }}>
            <ReactTable
              data={data}
              columns={columns}
              style={{
                height: "280px"
              }}
              showPagination={false}
              className="-striped -highlight"
              getTrProps={(state: any, rowInfo: any) => ({
                onClick: select.bind(null, rowInfo?.original as ITransferItem)
              })}
            />
          </Grid.Row>
          <Grid.Row columns={"equal"} style={{ paddingTop: "0px", paddingBottom: "0px" }}>
            <Grid.Column style={{ padding: "1px" }}>
              <WfmButton flex='true' style={{ width: "100%" }} disabled={selectedTransferItem === null || selectedTransferItem === undefined} onClick={handleViewDetailsClick}>View Details</WfmButton>
            </Grid.Column>
            <Grid.Column style={{ padding: "1px" }}>
              <WfmButton flex='true' style={{ width: "100%" }} disabled={selectedTransferItem === null || selectedTransferItem === undefined} onClick={handleUpdateClick}>Update</WfmButton>
            </Grid.Column>
            <Grid.Column style={{ padding: "1px" }}>
              <WfmButton flex='true' style={{ width: "100%" }} disabled={selectedTransferItem === null || selectedTransferItem === undefined} onClick={handleRemoveClick}>Remove</WfmButton>
            </Grid.Column>
          </Grid.Row>
          <Grid.Row columns={"equal"} style={{ paddingTop: "1px", paddingBottom: "0px" }}>
            <Grid.Column style={{ padding: "1px" }}>
              <WfmButton flex='true' style={{ width: "100%" }} onClick={handleAddLineItemClick}>Add Line Item</WfmButton>
            </Grid.Column>
            <Grid.Column style={{ padding: "1px" }}>
              <WfmButton  flex='true' style={{ width: "100%" }} disabled={transferData.Items.length === 0} onClick={handleSendClick}>Send</WfmButton>
            </Grid.Column>
          </Grid.Row>
        </Grid>
        <BasicModal alert={alert} setAlert={setAlert} />
      </Fragment>
    );
  }
};

export default TransferReview;
