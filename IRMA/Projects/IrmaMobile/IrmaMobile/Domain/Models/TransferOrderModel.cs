using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrmaMobile.Domain.Models
{
    public class TransferOrderModel
    {
        public int CreatedBy { get; set; }
        public int ProductTypeId { get; set; }
        public int OrderTypeId { get; set; }
        public int VendorId { get; set; }
        public int TransferSubTeamNo { get; set; }
        public int ReceiveLocationId { get; set; }
        public int PurchaseLocationId { get; set; }
        public int TransferToSubTeam { get; set; }
        public int SupplyTransferToSubTeam { get; set; }
        public bool FaxOrder { get; set; }
        public DateTime ExpectedDate { get; set; }
        public bool ReturnOrder { get; set; }
        public bool FromQueue { get; set; }
        public bool DsdOrder { get; set; }
        public List<TransferOrderItemModel> OrderItems { get; set; }
    }
}
