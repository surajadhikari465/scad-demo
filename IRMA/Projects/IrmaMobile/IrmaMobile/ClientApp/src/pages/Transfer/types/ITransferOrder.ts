import ITransferOrderItem from './ITransferOrderItem'

interface ITransferOrder {
    CreatedBy: number;
    ProductTypeId: number;
    OrderTypeId: number;
    VendorId: number;
    TransferSubTeamNo: number;
    ReceiveLocationId: number;
    PurchaseLocationId: number;
    TransferToSubTeam: number;
    SupplyTransferToSubTeam: number;
    FaxOrder: boolean;
    ExpectedDate: Date;
    ReturnOrder: boolean;
    FromQueue: boolean;
    DsdOrder: boolean;
    OrderItems: ITransferOrderItem[]
}

export default ITransferOrder;