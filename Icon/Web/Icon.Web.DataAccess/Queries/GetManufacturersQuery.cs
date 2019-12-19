using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetManufacturersQuery : IQueryHandler<GetManufacturersParameters, List<ManufacturerModel>>
    {
        private readonly IconContext context;

        public GetManufacturersQuery(IconContext context)
        {
            this.context = context;
        }

        public List<ManufacturerModel> Search(GetManufacturersParameters parameters)
        {
            return context.Database.SqlQuery<ManufacturerModel>("EXEC app.GetManufacturers").ToList();
        }
    }
}
