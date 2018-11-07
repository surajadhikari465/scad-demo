namespace KitBuilderWebApi.QueryParameters
{
    public class InstructionListsParameters: BaseParameters
    {
        public string Name { get; set; }
        public string SearchNameQuery { get; set; }
    }

    public class KitParameters : BaseParameters
    {
        public string KitName { get; set; }
        public string LinkGroupName { get; set; }
        public string ScanCode { get; set; }
        public string SearchKitNameQuery { get; set; }
        public string SearchLinkGroupNameQuery { get; set; }
    }
}