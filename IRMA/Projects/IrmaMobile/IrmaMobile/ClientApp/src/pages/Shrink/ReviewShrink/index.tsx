import React, { Fragment, useContext, useState, useEffect } from 'react';
import { AppContext, types } from "../../../store";
import { Modal, Confirm } from 'semantic-ui-react'
import CurrentLocation from "../../../layout/CurrentLocation";
import agent from '../../../api/agent';
import {
  useHistory
} from "react-router-dom";
import ReactTable from 'react-table';
import './styles.scss';
import { toast } from 'react-toastify';


const ReviewShrink:React.FC = () => {
    // @ts-ignore
    const {state, dispatch} = useContext(AppContext);
    const [shrinkItems, setShrinkItems] = useState<[] | any>([]);
    const [selected, setSelected] = useState();
    const [alert, setAlert] = useState({open:false, alertMessage:''});
    const [confirm, setConfirm] = useState({open:false, message:'', onConfirm: ()=>{}});
    let history = useHistory();
    useEffect(() => {
        if(localStorage.getItem("shrinkItems") !== null){
            // @ts-ignore
           let localShrinkItems = JSON.parse(localStorage.getItem("shrinkItems"));
           setShrinkItems(localShrinkItems);
        }   
        dispatch({ type: types.SHOWSHRINKHEADER, showShrinkHeader: false }); 
    }, []);
    const update = ()=>{
      if(shrinkItems.length === 0){
        setAlert({open:true, alertMessage: 'There are no Shrink Items to Update'});
      } else{
        if(!selected){
          setAlert({open:true, alertMessage: 'Please Select an Item'});}
        else{
        setConfirm({open:true, message: 'Do you want to update the Quantity/Subtype for the Selected UPC?', onConfirm: ()=>{history.push('/shrink/update')}});
        }
        
      }
    }
    const setIsLoading = (result: boolean) => {
        dispatch({ type: types.SETISLOADING, isLoading: result })
    }
    const upload = async()=> {
      if(shrinkItems.length === 0){
        setAlert({open:true, alertMessage: 'There are no Shrink Items to Upload'});
      }
      setIsLoading(true);
      for(let i=0; i<shrinkItems.length;i++){
        let weight = shrinkItems[i].costedByWeight ? shrinkItems[i].quantity: 0;
        try {
          var result = await agent.Shrink.shrinkItemSubmit(
            state.region, 
            shrinkItems[i].itemKey, 
            state.storeNumber, 
            state.subteamNo, 
            state.shrinkType.shrinkSubTypeId,
            state.shrinkType.inventoryAdjustmentCodeId,
            state.shrinkType.abbreviation+','+state.shrinkType.shrinkType,
            1,
            null,
            state.shrinkType.abbreviation,
            shrinkItems[i].quantity,
            weight
            );
          if(!result){
            toast.error(`Shrink Item ${shrinkItems[i].identifier} Failed to Upload`);
          }
        }
        finally {
          if(i  === shrinkItems.length-1){
            toast.success("Shrink Items Uploaded");
          }
          setIsLoading(false);
          localStorage.removeItem("shrinkItems");
          history.push('/shrink')
        }
      }
    }
    const close = () => {
      setSelected(undefined);
      setConfirm({open:false, message: '', onConfirm: ()=>{}});
    }

    const remove = ()=>{
      const newShrinkItems  = shrinkItems.filter(function( shrinkItem:any ) {
        return +shrinkItem.identifier !== +selected;
      });
      setShrinkItems(newShrinkItems);
      setSelected(undefined);
      setConfirm({open:false, message: '', onConfirm: ()=>{}});
      localStorage.setItem("shrinkItems", JSON.stringify(newShrinkItems));
    }

    const removeCheck = ()=>{
      if(shrinkItems.length === 0){
        setAlert({open:true, alertMessage: 'There are no Shrink Items to Remove'});
      } else if(selected===undefined){
        setAlert({open:true, alertMessage: 'Please Select an Item'});
      }
      else{
        setConfirm({open:true, message: 'Do you want to remove the selected UPC?', onConfirm: remove});
      }
    }
  
    const toggleAlert = (e:any) =>{
        setAlert({open:!alert.open, alertMessage: alert.alertMessage});
    }
    const select = (rowInfo:any) =>{
        setSelected(rowInfo.original.identifier)
    }
    const data = shrinkItems;
    const columns = React.useMemo(
        () => [
          {
            Header: 'UPC',
            accessor: "identifier"
          },
          {
            Header: 'Desc',
            accessor: "itemDescription"
          },
          {
            Header: 'Qty',
            accessor: "quantity",
            width: 40
          },
          {
            Header: 'SubType',
            accessor: "shrinkType"
          }
        ],
        [],
      )
    return (  
      <Fragment>
        <CurrentLocation/>
        <div className='review-shrink-page'>
            <section className='entry-section'>
                <ReactTable
                    data={data}
                    columns={columns}
                    style={{
                        height: "400px" 
                    }}
                    showPagination={false}
                    className="-striped -highlight"
                    getTrProps={(state:any, rowInfo: any) => ({
                      onClick:select.bind(null, rowInfo)
                    })}
                />
            </section>    
            <section className='entry-section'>
                <div className='shrink-buttons'>
                  <button className="wfm-btn" onClick={update}>Update</button>
                  <button className="wfm-btn" onClick={removeCheck}>Remove</button>
                  <button className="wfm-btn upload-btn" onClick={upload}>Upload</button>
                </div>
            </section>
            <Modal
              open={alert.open}
              header='IRMA Mobile'
              content={alert.alertMessage}
              actions={['OK']}
              onActionClick={toggleAlert}
            />
            <Confirm
              open={confirm.open}
              onCancel={close}
              header='IRMA Mobile'
              content={confirm.message}
              onConfirm={confirm.onConfirm}
            /> 
        </div>
        </Fragment>
    )
};

export default ReviewShrink;