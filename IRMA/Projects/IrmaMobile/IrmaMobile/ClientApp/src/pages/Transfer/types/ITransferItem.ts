interface ITransferItem {
    Upc: string;
    ItemKey: string;
    Quantity: number;
    TotalCost: number;
    RetailUomId: number;
    RetailUom: string;
    VendorPack: string;
    VendorCost: number;
    AdjustedCost: number;
    AdjustedReason: number;
    Description: string;
    SoldByWeight: boolean;
}

export default ITransferItem;