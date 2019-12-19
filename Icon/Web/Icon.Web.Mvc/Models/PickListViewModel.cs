using Icon.Common.Models;


namespace Icon.Web.Mvc.Models
{
    public class PickListViewModel
    {
        public int PickListId { get; set; }
        public int? AttributeId { get; set; }
        public string PickListValue { get; set; }

        public PickListViewModel() { }

        public PickListViewModel(PickListModel pickListModel)
        {
            this.PickListId = pickListModel.PickListId;
            this.AttributeId = pickListModel.AttributeId;
            this.PickListValue = pickListModel.PickListValue;
        }
    }
}