using Icon.Web.DataAccess.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{

    public class ItemHistoryModel
    {
        public int ItemId { get; set; }

        public int ItemTypeId { get; set; }

        public string ItemTypeCode { get; set; }

        public DateTime SysStartTimeUtc { get; set; }

        public DateTime SysEndTimeUtc { get; set; }

        public Dictionary<string, string> ItemAttributes { get; set; } = new Dictionary<string, string>();

        public ItemHistoryModel(ItemHistoryDbModel historyDbModel)
        {
            this.ItemId = historyDbModel.ItemId;
            this.ItemTypeId = historyDbModel.ItemTypeId;
            this.ItemTypeCode = historyDbModel.ItemTypeCode;
            this.SysStartTimeUtc = historyDbModel.SysStartTimeUtc;
            this.SysEndTimeUtc = historyDbModel.SysEndTimeUtc;
            this.ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(historyDbModel.ItemAttributesJson);
        }
        public ItemHistoryModel()
        {

        }
    }
}