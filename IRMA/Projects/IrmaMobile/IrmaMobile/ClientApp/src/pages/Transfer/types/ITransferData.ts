import ITransferItem from "./ITransferItem";

interface ITransferData {
    FromStoreVendorId: number;
    FromStoreNo: number;
    FromStoreName: string;
    FromSubteamNo: number;
    FromSubteamName: string;
    
    ProductType: number;
    SupplyType: number;
    
    ToStoreNo: number;
    ToStoreName: string;
    ToSubteamNo: number;
    ToSubteamName: string;

    CreatedBy: number;
    ExpectedDate: Date;
    
    Items: ITransferItem[];
  }

export default ITransferData;