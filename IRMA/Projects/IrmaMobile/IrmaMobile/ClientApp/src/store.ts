import React from "react";
import { OrderDetails } from "./pages/Receive/PurchaseOrder/types/OrderDetails";
import { ReasonCode } from "./pages/Receive/PurchaseOrder/types/ReasonCode";
import { ListedOrder } from "./pages/Receive/PurchaseOrder/types/ListedOrder";
import { DropdownItemProps } from "semantic-ui-react";

interface IIntitialState {
  region: string;
  stores: IStore[];
  store: string;
  storeNumber: string;
  subteams: ITeam[];
  subteam: ITeam;
  subteamNo: string;
  shrinkType: IShrinkType;
  shrinkTypes: any[],
  shrinkItems: any[],
  isLoading: boolean;
  orderDetails: OrderDetails | null;
  mappedReasonCodes: IMappedReasonCode[];
  reasonCodes: ReasonCode[];
  modalOpen: boolean;
  listedOrders: ListedOrder[];
  purchaseOrderUpc: string;
  purchaseOrderNumber: string;
  menuItems: IMenuItem[];
  showShrinkHeader: boolean;
  TransferData: ITransferData | undefined;
  Title: string;
}

export interface IMenuItem {
  id: number;
  order: number;
  text: string;
  path: string;
  disabled: boolean;
  onClick: (event: React.MouseEvent<HTMLDivElement, MouseEvent>, data: DropdownItemProps) => void;
}

export interface ITeam {
  subteamNo: string;
  subteamName: string;
}

export interface IShrinkType{
  shrinkType: string;  
  abbreviation: string;
  shrinkSubTypeId: null | number;
  shrinkSubTypeMember: string;
  inventoryAdjustmentCodeId: null | number;
  reasonCode: null | number;
}

export interface IStore {
  storeNo: string;
  name: string;
}

export interface IMappedReasonCode {
  key: number;
  text: string;
  value: string;
}

export interface ITransferData {
  FromStoreNo: number;
  FromSubteamNo: number;
  ToStoreNo: number;
  ToSubteamNo: number;
  ProductType: number;
  ExpectedDate: Date;
}

export const initialState = {
    region: "",
    stores: [],
    store: "",
    storeNumber: "",
    subteams: [],
    subteam: {subteamName:'', subteamNo:''},
    subteamNo:'',
    shrinkType: {shrinkType:'', abbreviation:'', shrinkSubTypeMember: '', shrinkSubTypeId:null, inventoryAdjustmentCodeId: null, reasonCode: null},
    shrinkTypes: [],
    shrinkItems: [],
    isLoading: false,
    orderDetails: null,
    mappedReasonCodes: [],
    reasonCodes: [],
    modalOpen: false,
    listedOrders: [],
    purchaseOrderUpc: "",
    purchaseOrderNumber: "",
    menuItems: [],
    showShrinkHeader: true,
    TransferData: undefined,
    Title: ''
} as IIntitialState;

export const types = {
    SETREGION: "SETREGION",
    SETSTORES: "SETSTORES",
    SETSTORE: "SETSTORE",
    SETSTORENUMBER: "SETSTORENUMBER",
    SETSUBTEAMS: "SETSUBTEAMS",
    SETSUBTEAM: "SETSUBTEAM",
    SETSUBTEAMNO: "SETSUBTEAMNO",
    SETSHRINKTYPE: "SETSHRINKTYPE",
    SETSHRINKTYPES: "SETSHRINKTYPES",
    SETSHRINKITEMS: "SETSHRINKITEMS",
    SETISLOADING: "SETISLOADING",
    SETORDERDETAILS: "SETORDERDETAILS",
    SETMAPPEDREASONCODES: "SETMAPPEDREASONCODES",
    SETREASONCODES: "SETREASONCODES",
    SETMODALOPEN: "SETMODALOPEN",
    SETLISTEDORDERS: "SETLISTEDORDERS",
    SETPURCHASEORDERUPC: "SETPURCHASEORDERUPC",
    SETPURCHASEORDERNUMBER: "SETPURCHASEORDERNUMBER",
    SETMENUITEMS: "SETMENUITEMS",
    SHOWSHRINKHEADER: "SHOWSHRINKHEADER",
    SETTRANSFERDATA: "SETTRANSFERDATA",
    SETTITLE: "SETTITLE"
};

export const AppContext = React.createContext({ state: initialState });

export const reducer = (state: any, action: any) => {
    switch (action.type) {
        case types.SETREGION: {
            return { ...state, region: action.region };
        }
        case types.SETSTORES: {
            return { ...state, stores: action.stores };
        }
        case types.SETSTORE: {
            return { ...state, store: action.store };
        }
        case types.SETSTORENUMBER: {
            return { ...state, storeNumber: action.storeNumber };
        }
        case types.SETSUBTEAMS: {
            return { ...state, subteams: action.subteams };
        }
        case types.SETSUBTEAM: {
            return { ...state, subteam: action.subteam };
        }
        case types.SETSUBTEAMNO: {
          return { ...state, subteamNo: action.subteamNo };
        }
        case types.SETSHRINKTYPE: {
            return { ...state, shrinkType: action.shrinkType };
        }
        case types.SETSHRINKTYPES: {
          return { ...state, shrinkTypes: action.shrinkTypes };
        }
        case types.SETSHRINKITEMS: {
          return { ...state, shrinkItems: action.shrinkItems };
        }
        case types.SETISLOADING: {
            return { ...state, isLoading: action.isLoading };
        }
        case types.SETORDERDETAILS: {
            return { ...state, orderDetails: action.orderDetails };
        }
        case types.SETMAPPEDREASONCODES: {
          return { ...state, mappedReasonCodes: action.mappedReasonCodes };
        }
        case types.SETREASONCODES: {
          return { ...state, reasonCodes: action.reasonCodes };
        }
        case types.SETMODALOPEN: {
          return { ...state, modalOpen: action.modalOpen };
        }
        case types.SETLISTEDORDERS: {
          return { ...state, listedOrders: action.listedOrders };
        }
        case types.SETPURCHASEORDERUPC: {
          return { ...state, purchaseOrderUpc: action.purchaseOrderUpc };
        }
        case types.SETPURCHASEORDERNUMBER: {
          return { ...state, purchaseOrderNumber: action.purchaseOrderNumber };
        }
        case types.SETMENUITEMS: {
          return { ...state, menuItems: action.menuItems }; 
        }
        case types.SHOWSHRINKHEADER: {
          return { ...state, showShrinkHeader: action.showShrinkHeader }; 
        }
        case types.SETTRANSFERDATA: {
          return { ...state, TransferData: action.TransferData };
        }
        case types.SETTITLE: {
          return { ...state, Title: action.Title };
        }
        default:
            return state;
    }
};