using Icon.Infor.Listeners.HierarchyClass.Extensions;
using System;

namespace Icon.Infor.Listeners.HierarchyClass.Models
{
    public class VimMessageArchiveHierarchy
    {
        public int MessageArchiveId { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public string HierarchyName { get; set; }
        public Guid EsbMessageId { get; set; }
        public string Context { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }

        public VimMessageArchiveHierarchy() { }

        public VimMessageArchiveHierarchy(VimHierarchyClassModel model)
        {
            this.HierarchyClassId = model.HierarchyClassId;
            this.HierarchyClassName = model.HierarchyClassName;
            this.HierarchyName = model.HierarchyName;
            this.EsbMessageId = Guid.Parse(model.MessageId);
            this.Context = model.ToJson();
            this.ErrorCode = model.ErrorCode;
            this.ErrorDetails = model.ErrorDetails;
        }
    }
}
