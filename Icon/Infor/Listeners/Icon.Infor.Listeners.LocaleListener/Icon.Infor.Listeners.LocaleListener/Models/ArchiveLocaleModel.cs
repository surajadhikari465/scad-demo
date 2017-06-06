using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.LocaleListener.Models
{
    public class MessageArchiveLocaleModel
    {
        public MessageArchiveLocaleModel() { }

        public MessageArchiveLocaleModel(LocaleModel locale, Guid inforMessageId)
        {
            LocaleId = locale.LocaleId == 0 ? (int?)null : locale.LocaleId;
            BusinessUnitId = locale.BusinessUnitId == 0 ? (int?)null : locale.BusinessUnitId;
            LocaleName = locale.Name;
            LocaleTypeCode = locale.TypeCode;
            Action = locale.Action.ToString();
            InforMessageId = inforMessageId;
            Context = JsonConvert.SerializeObject(locale);
            ErrorCode = locale.ErrorCode;
            ErrorDetails = locale.ErrorDetails;
        }

        public int MessageArchiveId { get; set; }
        public int? LocaleId { get; set; }
        public int? BusinessUnitId { get; set; }
        public string LocaleName { get; set; }
        public string LocaleTypeCode { get; set; }
        public string Action { get; set; }
        public Guid InforMessageId { get; set; }
        public string Context { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
