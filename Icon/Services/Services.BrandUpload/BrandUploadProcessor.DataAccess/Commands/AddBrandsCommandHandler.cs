using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.DataAccess.Commands
{
    public class AddBrandsCommandHandler : ICommandHandler<AddBrandsCommand>
    {
        private readonly IDbConnection dbConnection;

        public AddBrandsCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(AddBrandsCommand data)
        {
            foreach (var brand in data.Brands)
            {
                var singleBrandToAdd = new List<AddBrandModel> { brand };
                try
                {
                    if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                    using (var transactionScope = dbConnection.BeginTransaction())
                    {
                        // dapper maps the use defined table by ordinal, not by name.
                        // order of properties is important here.
                        var tvp = singleBrandToAdd.Select(s => new {
                            s.BrandId,
                            s.BrandName,
                            s.BrandAbbreviation,
                            s.Designation,
                            s.ParentCompany,
                            s.ZipCode,
                            s.Locality
                        }).ToDataTable().AsTableValuedParameter("dbo.AddUpdateBrandType");

                        var addedBrandId = dbConnection.Query<int>(
                            "dbo.AddBrand",
                            new
                            {
                                Brand = tvp
                            }, commandType: CommandType.StoredProcedure, transaction: transactionScope).First();

                        data.AddedBrandIds.Add(addedBrandId);
                        transactionScope.Commit();
                    }
                }
                catch (Exception ex)
                {
                    data.InvalidBrands.Add(new ErrorItem<AddBrandModel>(brand, ex.Message));
                }
            }
        }
    }
}