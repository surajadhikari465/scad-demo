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
    public class UpdateBrandsCommandHandler : ICommandHandler<UpdateBrandsCommand>
    {
        private readonly IDbConnection connection;

        public UpdateBrandsCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(UpdateBrandsCommand data)
        {
            foreach (var brand in data.Brands)
            {
                var singleBrandToUpdate = new List<UpdateBrandModel> {brand};
                try
                {
                    if (connection.State != ConnectionState.Open) connection.Open();
                    using (var transactionScope = connection.BeginTransaction())
                    {
                        // dapper maps the use defined table by ordinal, not by name.
                        // order of properties is important here.
                        var tvp = singleBrandToUpdate.Select(s => new {
                            s.BrandId,
                            s.BrandName,
                            s.BrandAbbreviation,
                            s.Designation,
                            s.ParentCompany,
                            s.ZipCode,
                            s.Locality
                        }).ToDataTable().AsTableValuedParameter("dbo.AddUpdateBrandType");

                        var updatedBrandId = connection.QueryFirst<int>(
                            "dbo.UpdateBrand",
                            new { Brand = tvp },
                            commandType: CommandType.StoredProcedure, transaction: transactionScope);
                 
                        data.UpdatedBrandIds.Add(updatedBrandId);
                        transactionScope.Commit();
                    }
                }
                catch (Exception ex)
                {
                    data.InvalidBrands.Add(new ErrorItem<UpdateBrandModel>(brand, ex.Message));
                }
            }
        }
    }
}