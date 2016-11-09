using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Models
{
    public class MessageArchiveProductModel
    {
        public MessageArchiveProductModel(ItemModel itemModel)
        {
            ItemId = itemModel.ItemId;
            ScanCode = itemModel.ScanCode;
            InforMessageId = itemModel.InforMessageId;
            Context = JsonConvert.SerializeObject(itemModel);
            ErrorCode = itemModel.ErrorCode;
            ErrorDetails = itemModel.ErrorDetails;
        }

        public int MessageArchiveId { get; set; }
        public int ItemId { get; set; }
        public string ScanCode { get; set; }
        public Guid InforMessageId { get; set; }
        public string Context { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
