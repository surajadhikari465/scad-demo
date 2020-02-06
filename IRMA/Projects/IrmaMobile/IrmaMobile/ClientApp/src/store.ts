import React from "react";
import { OrderDetails } from "./pages/Receive/PurchaseOrder/types/OrderDetails";
import { ReasonCode } from "./pages/Receive/PurchaseOrder/types/ReasonCode";
import { ListedOrder } from "./pages/Receive/PurchaseOrder/types/ListedOrder";
import createPersistedReducer from "./scripts/use-persisted-reducer";
import ITransferItem from "./pages/Transfer/types/ITransferItem";
import ITransferData from "./pages/Transfer/types/ITransferData";
import { stat } from "fs";

export const usePersistedReducer = createPersistedReducer('state');

interface IIntitialState {
    region: string;
    stores: IStore[];
    store: string;
    storeNumber: string;
    subteams: ITeam[];
    subteam: ITeam;
    subteamNo: string;
    subteamSession: ISubteamSession;
    shrinkType: IShrinkType;
    shrinkTypes: any[],
    selectedShrinkItem: ISelectedShrink,
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
    settingsItems: IMenuItem[];
    showShrinkHeader: boolean;
    Title: string;
    transferData: ITransferData | null;
    transferLineItem: ITransferItem | null;
    showCog: boolean;
}

export interface IMenuItem {
    id: number;
    order: number;
    text: string;
    path: string;
    disabled: boolean;
    onClick: any;
}

export interface ISubteamSession {
    isPrevSession: boolean;
    sessionShrinkType: string;
    sessionSubteam: string;
    forceSubteamSelection?: boolean;
}

export interface ITeam {
    subteamNo: string;
    subteamName: string;
}

export interface ISelectedShrink {
    identifier: any;
    quantity: number;
    signDescription: string;
    packageDesc1: string;
    packageDesc2: string;
    packageUnitAbbreviation: string;
    shrinkType: string;
    costedByWeight: boolean;
}

export interface IShrinkType {
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

export const initialState = {
    region: "",
    stores: [],
    store: "",
    storeNumber: "",
    subteams: [],
    subteam: { subteamName: '', subteamNo: '' },
    subteamSession: { isPrevSession: false, sessionShrinkType: '', sessionSubteam: '', forceSubteamSelection: true },
    subteamNo: '',
    shrinkType: { shrinkType: '', abbreviation: '', shrinkSubTypeMember: '', shrinkSubTypeId: null, inventoryAdjustmentCodeId: null, reasonCode: null },
    selectedShrinkItem: { identifier: '', quantity: 0, signDescription: '', packageDesc1: '', packageDesc2: '', packageUnitAbbreviation: '', shrinkType: '', costedByWeight: false },
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
    Title: 'IRMA Mobile',
    transferData: null,
    transferLineItem: null,
    showCog: false
} as IIntitialState;

export const types = {
    SETREGION: "SETREGION",
    SETSTORES: "SETSTORES",
    SETSTORE: "SETSTORE",
    SETSTORENUMBER: "SETSTORENUMBER",
    SETSUBTEAMS: "SETSUBTEAMS",
    SETSUBTEAM: "SETSUBTEAM",
    SETSUBTEAMSESSION: "SETSUBTEAMSESSION",
    SETSUBTEAMNO: "SETSUBTEAMNO",
    SETSHRINKTYPE: "SETSHRINKTYPE",
    SETSHRINKTYPES: "SETSHRINKTYPES",
    SETSHRINKITEMS: "SETSHRINKITEMS",
    SETSELECTEDSHRINKITEM: "SETSELECTEDSHRINKITEM",
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
    SETTITLE: "SETTITLE",
    SETTRANSFERDATA: "SETTRANSFERDATA",
    SETTRANSFERLINEITEM: "SETTRANSFERLINEITEM",
    TOGGLECOG: "TOGGLECOG"
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
        case types.SETSUBTEAMSESSION: {
            return { ...state, subteamSession: action.subteamSession };
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
        case types.SETSELECTEDSHRINKITEM: {
            return { ...state, selectedShrinkItem: action.selectedShrinkItem };
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
        case types.SETTITLE: {
            return { ...state, Title: action.Title };
        }
        default:
            return state;
    }
};