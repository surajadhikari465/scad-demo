using System.Linq;
using System.Collections.Generic;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Extensions;

namespace Icon.Web.DataAccess.Commands
{
    public class AddUpdateContactCommandHandler : ICommandHandler<AddUpdateContactCommand>
    {
        private IDbProvider db;

        public AddUpdateContactCommandHandler(IDbProvider dbProvider)
        {
            this.db = dbProvider;
        }

        public void Execute(AddUpdateContactCommand data)
        {
            var tvp = data.Contacts.Select(x => new
                    {
                        x.ContactId,
                        x.HierarchyClassId,
	                    x.ContactTypeId,
	                    x.ContactName,
	                    x.Email,
	                    x.Title,
	                    x.AddressLine1,
	                    x.AddressLine2,
	                    x.City,
	                    x.State,
	                    x.ZipCode,
	                    x.Country,
	                    x.PhoneNumber1,
	                    x.PhoneNumber2,
	                    x.WebsiteURL  
                    })
            .ToList()
            .ToDataTable(); //TYPE app.ContactInputType

            this.db.Connection.Query<int>(sql: "app.AddUpdateContact",
                                          param: new { contact = tvp },
                                          transaction: this.db.Transaction,
                                          commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}