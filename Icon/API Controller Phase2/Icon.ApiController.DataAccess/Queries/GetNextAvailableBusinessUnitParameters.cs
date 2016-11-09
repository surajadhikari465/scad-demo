using Icon.Common.DataAccess;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetNextAvailableBusinessUnitParameters : IQuery<int?>
    {
        public int InstanceId { get; set; }
        public string MessageQueueName { get; set; }
    }
}
