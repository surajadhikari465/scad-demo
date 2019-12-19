using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAvailablePlusByCategoryParameters : IQuery<List<IRMAItem>>
    {
        public int PluCategoryId { get; set; }
        public int MaxPlus { get; set; }
    }
}