using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Models.DataMonster;
using MoreLinq.Extensions;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetItemsBySearchCriteriaQueryHandler : IQueryHandler<GetItemsBySearchCriteriaQuery, IEnumerable<ItemDetail>>
    {
        private IDbProvider db;

        public GetItemsBySearchCriteriaQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<ItemDetail> Search(GetItemsBySearchCriteriaQuery parameters)
        {
            var values = parameters.IncludedStores.Select(v => new { Value = v }).ToDataTable();

            var multiResults = this.db.Connection.QueryMultiple(
                "[dbo].[GetItemsBySearchCriteria]",
                new
                {
                    BrandName = parameters.BrandName,
                    Subteam = parameters.Subteam,
                    Supplier = parameters.Supplier,
                    ItemDescription = parameters.ItemDescription,
                    Region = parameters.Region,
                    IncludeLocales = parameters.IncludeLocales,
                    IncludedStores = values
                },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction,commandTimeout:30000);


            var itemInformationList = multiResults.Read<ItemDetailInformation>().ToList();
            List<ItemDetailLocaleInformation> itemRegionalInformationList = new List<ItemDetailLocaleInformation>() ;

            if (parameters.IncludeLocales)
            {
                itemRegionalInformationList = multiResults.Read<ItemDetailLocaleInformation>().ToList();

            }    

            List<ItemDetail> itemDetailList = new List<ItemDetail>();
            foreach (ItemDetailInformation itemInformation in itemInformationList)
            {
                ItemDetail itemDetail = new ItemDetail();
                itemDetail.ItemInformation = itemInformation;
                if(parameters.IncludeLocales)
                {
                    itemDetail.ItemDetailLocaleInformation = itemRegionalInformationList.Where(i => i.ItemId.ToString() == itemInformation.InforItemID).ToList();
                }
             
                itemDetailList.Add(itemDetail);
            }
            return itemDetailList;
           
        }
    }
}