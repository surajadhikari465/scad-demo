using System;

namespace PushController.Common
{
    public class ItemLinkModel
    {
        public int IrmaPushId { get; set; }
        public int ParentItemId { get; set; }
        public int ChildItemId { get; set; }
        public int LocaleId { get; set; }
        public bool IsDelete { get; set; }
    }
}
