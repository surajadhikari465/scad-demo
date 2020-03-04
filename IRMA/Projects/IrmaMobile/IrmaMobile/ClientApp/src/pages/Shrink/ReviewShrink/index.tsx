import React, { Fragment, useContext, useState, useEffect, useCallback } from 'react';
import { AppContext, types, IMenuItem, IShrinkAdjustment } from "../../../store";
import { Modal, Confirm } from 'semantic-ui-react'
import CurrentLocation from "../../../layout/CurrentLocation";
import LoadingComponent from "../../../layout/LoadingComponent";
import agent from "../../../api/agent";
import {
  useHistory
} from "react-router-dom";
import ReactTable from 'react-table';
import './styles.scss';
import { toast } from 'react-toastify';

const ReviewShrink: React.FC = () => {
  // @ts-ignore
  const { state, dispatch } = useContext(AppContext);
  const [shrinkItems, setShrinkItems] = useState<[] | any>([]);
  const [selected, setSelected] = useState();
  const { isLoading, user, subteamSession, shrinkTypes } = state;
  const [alert, setAlert] = useState({ open: false, alertMessage: '' });
  const [confirm, setConfirm] = useState({ open: false, message: '', onConfirm: () => { } });
  let sessionIndex = subteamSession.findIndex((session:any) => session.sessionUser.userName === user?.userName);
  let history = useHistory();

  const setMenuItems = useCallback(() => {
    const newMenuItems = [
      { id: 1, order: 0, text: "Back", path: "/shrink", disabled: false } as IMenuItem
    ] as IMenuItem[];

    dispatch({ type: types.SETMENUITEMS, menuItems: newMenuItems });
  }, [dispatch]);

  useEffect(() => {
    setMenuItems();

    return () => {
      dispatch({ type: types.SETMENUITEMS, menuItems: [] });
    }
  }, [setMenuItems, dispatch]);

  useEffect(() => {
    if (subteamSession[sessionIndex].shrinkItems) {
      // @ts-ignore
      let localShrinkItems = subteamSession[sessionIndex].shrinkItems;
      setShrinkItems(localShrinkItems);
    }
    dispatch({ type: types.SHOWSHRINKHEADER, showShrinkHeader: false });
  }, [dispatch]);

  const update = () => {
    if (shrinkItems.length === 0) {
      setAlert({ open: true, alertMessage: 'There are no Shrink Items to Update' });
    } else {
      if (!selected) {
        setAlert({ open: true, alertMessage: 'Please Select an Item' });
      }
      else {
        setConfirm({ open: true, message: 'Do you want to update the Quantity/Subtype for the Selected UPC?', onConfirm: () => { history.push('/shrink/update') } });
      }
    }
  }

  const findShrinkAjustmentIdByAbbreviation = (shrinkAdjustments: IShrinkAdjustment[] | null,  abbreviation: string) :number => {
      if (shrinkAdjustments === null) return 0;
      var adjustment = shrinkAdjustments.find( a => a.abbreviation === abbreviation);
      if (adjustment === undefined) return -1;
      return adjustment.adjustmentID;
  }

  const setIsLoading = (result: boolean) => {
    dispatch({ type: types.SETISLOADING, isLoading: result })
  }
  const upload = async () => {
    if (shrinkItems.length === 0) {
      setAlert({ open: true, alertMessage: 'There are no Shrink Items to Upload' });
    } else {
      setIsLoading(true);
      let succeededItems = 0;
      for (let i = 0; i < shrinkItems.length; i++) {
        let weight = shrinkItems[i].costedByWeight ? shrinkItems[i].quantity : 0;
        let quantity = shrinkItems[i].costedByWeight ? 0 : shrinkItems[i].quantity;
        let selectedShrinkType : any = shrinkTypes.filter((shrinkType)=>shrinkType.shrinkSubTypeMember === shrinkItems[i].shrinkType)[0];
        try {
          var result = await agent.Shrink.submitShrinkItems(
            state.region,
            shrinkItems[i].itemKey,
            subteamSession[sessionIndex].sessionNumber,
            subteamSession[sessionIndex].sessionSubteam?.subteamNo,
            selectedShrinkType.shrinkSubTypeId,
            findShrinkAjustmentIdByAbbreviation(state.shrinkAdjustmentReasons, state.shrinkType.abbreviation),
            selectedShrinkType.shrinkType,
            user!.userId,
            user!.userName,
            selectedShrinkType.abbreviation,
            quantity,
            weight
          );
          if (!result || result === undefined) {
            toast.error(`Shrink Item ${shrinkItems[i].identifier} Failed to Upload`);
          } else {
            succeededItems += 1;
          }
        }
        finally {
          setIsLoading(false);
          localStorage.removeItem('sessionSubType');
          localStorage.removeItem('shrinkUser');
          localStorage.removeItem('shrinkSubteam');
          history.push('/shrink')
        }
      }
      if (succeededItems === shrinkItems.length) {
        toast.success("Shrink Items Uploaded");
        dispatch({ type: types.SHOWSHRINKHEADER, showShrinkHeader: false });
        subteamSession[sessionIndex] = { shrinkItems:[], isPrevSession: false, sessionShrinkType: '', sessionSubteam: undefined, sessionStore:'', sessionNumber: 0, sessionRegion:'', sessionUser: user, forceSubteamSelection: true };
        dispatch({ type: types.SETSUBTEAMSESSION, subteamSession });
      }
    }
  }
  const close = () => {
    setSelected(undefined);
    setConfirm({ open: false, message: '', onConfirm: () => { } });
  }

  const remove = () => {
    const newShrinkItems = shrinkItems.filter(function (shrinkItem: any) {
      return +shrinkItem.identifier !== +selected;
    });
    setShrinkItems(newShrinkItems);
    setSelected(undefined);
    setConfirm({ open: false, message: '', onConfirm: () => { } });
    const subteamSessionCopy = {...subteamSession[sessionIndex], shrinkItems: newShrinkItems };
    dispatch({ type: types.SETSUBTEAMSESSION, subteamSession: [...subteamSession.slice(0, sessionIndex), subteamSessionCopy, ...subteamSession.slice(sessionIndex + 1)]})
  }

  const removeCheck = () => {
    if (shrinkItems.length === 0) {
      setAlert({ open: true, alertMessage: 'There are no Shrink Items to Remove' });
    } else if (selected === undefined) {
      setAlert({ open: true, alertMessage: 'Please Select an Item' });
    }
    else {
      setConfirm({ open: true, message: 'Do you want to remove the selected UPC?', onConfirm: remove });
    }
  }

  const toggleAlert = (e: any) => {
    setAlert({ open: !alert.open, alertMessage: alert.alertMessage });
  }
  const select = (rowInfo: any) => {
    setSelected(rowInfo.original.identifier);
    dispatch({ type: types.SETSELECTEDSHRINKITEM, selectedShrinkItem: rowInfo.original });
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
  if (isLoading) {
    return (<LoadingComponent content="Uploading Items..." />)
  }
  return (
    <Fragment>
      <CurrentLocation />
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
            getTrProps={(state: any, rowInfo: any) => ({
              onClick: select.bind(null, rowInfo)
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