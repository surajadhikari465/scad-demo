import React, { useContext, useState, useEffect } from 'react';
import { AppContext } from "../../../store";
import agent from '../../../api/agent';
import ReactTable from 'react-table';
import './styles.scss';


const ReviewShrink:React.FC = () => {
    const {state} = useContext(AppContext);
    const [shrinkItems, setShrinkItems] = useState<[] | any>([]);
    const [selected, setSelected] = useState();
    useEffect(() => {
        if(localStorage.getItem("shrinkItems") !== null){
            // @ts-ignore
           let localShrinkItems = JSON.parse(localStorage.getItem("shrinkItems"));
           setShrinkItems(localShrinkItems);
        }    
    }, []);
    const update = ()=>{
      if(shrinkItems.length === 0){
        alert('There are no Shrink Items to Update');
      }
    }
    const upload = ()=>{
      if(shrinkItems.length === 0){
        alert('There are no Shrink Items to Upload');
      }
      
      for(let i=0; i<shrinkItems.length;i++){
        let weight = shrinkItems[i].costedByWeight ? shrinkItems[i].quantity: 0;
        agent.Shrink.shrinkItemSubmit(
          state.region, 
          shrinkItems[i].itemKey, 
          state.storeNumber, 
          state.subteamNo, 
          state.shrinkType.shrinkSubTypeId,
          state.shrinkType.inventoryAdjustmentCodeId,
          state.shrinkType.abbreviation+','+state.shrinkType.shrinkType,
          0,
          null,
          state.shrinkType.abbreviation,
          shrinkItems[i].quantity,
          weight
          );
      }
    }
    const remove = ()=>{
      if(selected===undefined){
        alert('Please Select an Item');
      }
      else{
        if(window.confirm("Do you want to remove the selected UPC?")){
            const newShrinkItems  = shrinkItems.filter(function( shrinkItem:any ) {
              return +shrinkItem.identifier !== +selected;
            });
            setShrinkItems(newShrinkItems);
            localStorage.setItem("shrinkItems", JSON.stringify(newShrinkItems));
          }
        }
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
            accessor: "quantity"
          },
          {
            Header: 'SubType',
            accessor: "retailSubteamName"
          }
        ],
        [],

      )

    return (  
        <div className='review-shrink-page'>
            <section className='entry-section'>
                <ReactTable
                    data={data}
                    columns={columns}
                    defaultPageSize={20}
                    style={{
                        height: "480px" 
                    }}
                    className="-striped -highlight"
                    getTrProps={(state:any, rowInfo: any) => ({
                      onClick:select.bind(null, rowInfo)
                    })}
                />
            </section>    
            <section className='entry-section'>
                <div className='shrink-buttons'>
                  <button className="wfm-btn" onClick={update}>Update</button>
                  <button className="wfm-btn" onClick={upload}>Upload</button>
                  <button className="wfm-btn" onClick={remove}>Remove</button>
                </div>
              </section>
        </div>
    )
};

export default ReviewShrink;