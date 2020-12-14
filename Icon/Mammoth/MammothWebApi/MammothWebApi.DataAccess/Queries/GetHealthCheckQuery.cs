using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetHealthCheckQuery : IQuery<int>
    {
        public int CheckId { get; set; }
    }
}
