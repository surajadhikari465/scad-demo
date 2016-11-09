using Icon.Common.DataAccess;

namespace Icon.Esb.EwicAplListener.DataAccess.Queries
{
    public class AgencyExistsParameters : IQuery<bool>
    {
        public string AgencyId { get; set; }
    }
}
