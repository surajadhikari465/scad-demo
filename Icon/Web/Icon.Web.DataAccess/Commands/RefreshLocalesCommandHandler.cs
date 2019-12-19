using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class RefreshLocalesCommandHandler : ICommandHandler<RefreshLocalesCommand>
    {
        private IconContext context;

        public RefreshLocalesCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(RefreshLocalesCommand data)
        {
            var validLocales = context.Locale
                .Where(l => data.LocaleIds.Contains(l.localeID))
                .Select(sc => sc.localeID)
                .ToList()
                .ConvertAll(id => new
                {
                    I = id
                })
                .ToDataTable();

            AddMerchandiseMessages(validLocales);
        }

        private void AddMerchandiseMessages(DataTable validLocales)
        {
            SqlParameter inputType = new SqlParameter("ids", SqlDbType.Structured)
            {
                TypeName = "app.IntList",
                Value = validLocales
            };

            string sql = "EXEC app.RefreshLocales @ids";
            context.Database.ExecuteSqlCommand(sql, inputType);
        }
    }
}