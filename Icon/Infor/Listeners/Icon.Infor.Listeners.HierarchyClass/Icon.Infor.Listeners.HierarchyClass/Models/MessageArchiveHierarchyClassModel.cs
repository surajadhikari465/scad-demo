using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Models
{
    public class MessageArchiveHierarchyClassModel
    {
        public int MessageArchiveId { get; set; }
        public int HierarchyClassId { get; set; } 
        public string HierarchyClassName { get; set; }
        public string HierarchyName { get; set; }
        public Guid InforMessageId { get; set; }
        public string Context { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }

        public MessageArchiveHierarchyClassModel(HierarchyClassModel model)
        {
            this.HierarchyClassId = model.HierarchyClassId;
            this.HierarchyClassName = model.HierarchyClassName;
            this.HierarchyName = model.HierarchyName;
            this.InforMessageId = Guid.Parse(model.InforMessageId);
            this.Context = JsonConvert.SerializeObject(model);
            this.ErrorCode = model.ErrorCode;
            this.ErrorDetails = model.ErrorDetails;
        }
    }
}
