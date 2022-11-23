using GPMService.Producer.Model.DBModel;
using Mammoth.Framework;
using System.Collections.Generic;

namespace GPMService.Producer.DataAccess
{
    internal interface IExpiringTprProcessorDAL
    {
        IEnumerable<GetExpiringTprsQueryModel> GetExpiringTprs(MammothContext mammothContext, string region);
    }
}
