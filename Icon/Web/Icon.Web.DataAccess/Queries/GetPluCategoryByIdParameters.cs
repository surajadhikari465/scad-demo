using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluCategoryByIdParameters : IQuery<PLUCategory> 
    {
        public int PluCategoryID { get; set; } 
    }
}
