using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetNationalClassByClassCodeQuery : IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>>
    {
        private readonly IconContext context;

        public GetNationalClassByClassCodeQuery(IconContext context)
        {
            this.context = context;
        }

        public List<HierarchyClass> Search(GetNationalClassByClassCodeParameters parameters)
        {            
            SqlParameter nationalClassCode = new SqlParameter("NationalClassCode", SqlDbType.VarChar);
            nationalClassCode.Value = parameters.ClassCode == null ? (Object)DBNull.Value : parameters.ClassCode;


            string sql = "app.GetNationalClassForClassCode @NationalClassCode";

            SqlParameter[] sqlParameters = 
            {
                nationalClassCode
            };

            
                var dbResult = this.context.Database.SqlQuery<HierarchyClass>(sql, sqlParameters);
                return  dbResult.ToList() ;
            
        }

        private string SetSortColumnToMatchStoredProcedureColumn(string sortColumn)
        {
            switch(sortColumn)
            {
                case "ScanCode": return "sc.scanCode";
                case "BrandHierarchyClassId": return "brand.hierarchyClassName";
                case "ProductDescription": return "pd.traitValue";
                case "PosDescription": return "pos.traitValue";
                case "PackageUnit": return "pack.traitValue";
                case "FoodStampEligible": return "fs.traitValue";
                case "PosScaleTare": return "tare.traitValue";
                case "RetailSize": return "size.traitValue";
                case "RetailUom": return "uom.traitValue";
                case "MerchandiseHierarchyClassId": return "merch.hierarchyClassName";
                case "TaxHierarchyClassId": return "tax.hierarchyClassName";
                case "IsValidated": return "vld.traitValue";
                case "DepartmentSale": return "dept.traitValue";
                case "HiddenItem": return "hid.traitValue";
                case "Notes": return "note.traitValue";
                case "NationalHierarchyClassId": return "nat.hierarchyClassName";
                case "AnimalWelfareRatingId": return "isa.AnimalWelfareRatingId";
                case "CheeseMilkTypeId": return "isa.CheeseMilkTypeId";
                case "CheeseRaw": return "isa.CheeseRaw";
                case "EcoScaleRatingId": return "isa.EcoScaleRatingId";
                case "GlutenFreeAgency": return "isa.GlutenFreeAgencyName";
                case "KosherAgency": return "isa.KosherAgencyName";
                case "Msc": return "isa.Msc";
                case "NonGmoAgency": return "isa.NonGmoAgencyName";
                case "OrganicAgency": return "isa.OrganicAgencyName";
                case "PremiumBodyCare": return "isa.PremiumBodyCare";
                case "SeafoodFreshOrFrozenId": return "isa.SeafoodFreshOrFrozenId";
                case "SeafoodCatchTypeId": return "isa.SeafoodCatchTypeId";
                case "VeganAgency": return "isa.VeganAgencyName";
                case "Vegetarian": return "isa.Vegetarian";
                case "WholeTrade": return "isa.WholeTrade";
                case "GrassFed": return "isa.GrassFed";
                case "PastureRaised": return "isa.PastureRaised";
                case "FreeRange": return "isa.FreeRange";
                case "DryAged": return "isa.DryAged";
                case "AirChilled": return "isa.AirChilled";
                case "MadeInHouse": return "isa.MadeinHouse";
                case "CreatedDate": return "crdate.traitValue";
                case "LastModifiedDate": return "moddate.traitValue";
                case "LastModifiedUser": return "modusr.traitValue";
                default: throw new ArgumentException("Unable to sort by " + sortColumn + ". No column is registered to sort by that paramter.");
            }
        }

        private string SetSortOrderToMatchStoredProcedureOrder(string sortOrder)
        {
            if(sortOrder.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
            {
                return "ASC";
            }
            else if(sortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
            {
                return "DESC";
            }
            else
            {
                throw new ArgumentException("Unable to use sort order " + sortOrder + ". No order is registered to sort by that parameter.");
            }
        }
    }
}
