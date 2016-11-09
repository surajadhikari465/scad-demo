namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateInProcessBusinessUnitCommand
    {
        public int InstanceId { get; set; }
        public int BusinessUnitId { get; set; }
        public int MessageTypeId { get; set; }
    }
}
