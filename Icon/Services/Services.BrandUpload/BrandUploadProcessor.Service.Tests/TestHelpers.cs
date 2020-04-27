using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using BrandUploadProcessor.Common.Models;
using Dapper;

namespace BrandUploadProcessor.Service.Tests
{
    public static class TestHelpers
    {

        public static IDbConnection Icon => new SqlConnection(ConfigurationManager.ConnectionStrings["IconConnectionString"].ConnectionString);

        public static List<ColumnHeader> GetHeaders()
        {
            return new List<ColumnHeader>
            {
                new ColumnHeader {Address = "A", ColumnIndex = 1, Name = "Brand ID"},
                new ColumnHeader {Address = "B", ColumnIndex = 2, Name = "Brand Name"},
                new ColumnHeader {Address = "C", ColumnIndex = 3, Name = "Brand Abbreviation"},
                new ColumnHeader {Address = "D", ColumnIndex = 4, Name = "Parent Company"},
                new ColumnHeader {Address = "E", ColumnIndex = 5, Name = "Zip Code"},
                new ColumnHeader {Address = "F", ColumnIndex = 6, Name = "Locality"},
                new ColumnHeader {Address = "G", ColumnIndex = 7, Name = "Designation"}
            };
        }

        public static List<BrandModel> GetBrands()
        {
            return new List<BrandModel>
            {
                new BrandModel { BrandId = 1, BrandName = "test1", BrandAbbreviation = "t1"},
                new BrandModel { BrandId = 2, BrandName = "test2", BrandAbbreviation = "t2"},
                new BrandModel { BrandId = 3, BrandName = "test3", BrandAbbreviation = "t3"}
            };
        }

        public static List<BrandAttributeModel> GetBrandAttributeModels()
        {
            return new List<BrandAttributeModel>
            {
                GetBrandAttributeModel(66, "Brand Abbreviation", "BA", "^[a-zA-Z0-9 &]{1,10}$", true,false),
                GetBrandAttributeModel(188,"Designation", "GRD", "(Global|Regional)$",false,false),
                GetBrandAttributeModel(189,"Parent Company", "PCO", "^[0-9]{1,10}$", false, false),
                GetBrandAttributeModel(213, "Zip Code", "ZIP", "^[0-9]{5}(?:-[0-9]{4})?$", false,false),
                GetBrandAttributeModel(214, "Locality", "LCL", "^.{1,35}$", false,false),
                GetBrandAttributeModel(0, "Brand Name", "BN", "^[a-zA-Z0-9 &]{1,255}$", true,false),
            };
        }

        public static BrandAttributeModel GetBrandAttributeModel(int traitId, string traitDesc, string traitCode,
            string traitPattern, bool isRequired, bool isReadonly)
        {
            return new BrandAttributeModel()
            {
                IsRequired = isRequired, IsReadOnly = isReadonly, TraitDesc = traitDesc, TraitPattern = traitPattern,
                TraitCode = traitCode, TraitId = traitId
            };
        }

        public static ParsedCell CreateParsedCell(string columnName,  string cellValue)
        {

            var address = GetAddressByColumnName(columnName);
            var columnIndex = GetIndexByColumnName(columnName);
            return new ParsedCell
            {
                Address = address,
                CellValue = cellValue,
                Column = new ColumnHeader
                {
                    Address = address,
                    ColumnIndex = columnIndex,
                    Name = columnName
                }
            };
        }

        public static RowObject CreateRowObject(int rowId, List<ParsedCell> cells)
        {
            return new RowObject
            {
                Row = rowId,
                Cells = cells
            };
        }

        public static T GetBrandAndTraitsByHierarchyClassId<T>(IDbConnection connection, int hierarchyClassId)
        {

            var sql = @"

                    declare @BrandAbbreviationTraitId int = (select TraitId from Trait where Traitcode = 'BA')
					declare @DesignationTraitId int = (select TraitId from Trait where Traitcode = 'GRD')
					declare @ParentCompanyTraitId int = (select TraitId from Trait where Traitcode = 'PCO')
					declare @ZipCodeTraitId int = (select TraitId from Trait where Traitcode = 'ZIP')
					declare @LocalityTraitId int = (select TraitId from Trait where Traitcode = 'LCL')

                    select  hc.hierarchyClassId BrandId, 
                            hc.HierarchyClassName BrandName,
                            hctBA.traitvalue BrandAbbreviation,
                            hctGRD.traitvalue Designation,
                            hctPCO.traitvalue ParentCompany,
                            hctZIP.traitvalue ZipCode,
                            hctLCL.traitvalue Locality
                    from HierarchyClass hc inner join Hierarchy h on hc.HierarchyId = h.HierarchyId 
                    inner join HierarchyClassTrait hctBA on hc.hierarchyClassId = hctBA.hierarchyClassId and hctBA.traitid = @BrandAbbreviationTraitId
                    left join HierarchyClassTrait hctGRD on hc.hierarchyClassId = hctGRD.hierarchyClassId and hctGRD.traitid = @DesignationTraitId
                    left join HierarchyClassTrait hctPCO on hc.hierarchyClassId = hctPCO.hierarchyClassId and hctPCO.traitid = @ParentCompanyTraitId
                    left join HierarchyClassTrait hctZIP on hc.hierarchyClassId = hctZIP.hierarchyClassId and hctZIP.traitid = @ZipCodeTraitId
                    left join HierarchyClassTrait hctLCL on hc.hierarchyClassId = hctLCL.hierarchyClassId and hctLCL.traitid = @LocalityTraitId
                    where h.HierarchyName = 'Brands' and hc.HierarchyClassId = @hierarchyClassid;
            ";

            T data=  connection.QueryFirstOrDefault<T>(sql, new { hierarchyClassId });
            return data;

        }

        public static string GetAddressByColumnName(string columnName)
        {
            var header = GetHeaders().FirstOrDefault(h => h.Name == columnName);
            return header?.Address;
        }
        public static int GetIndexByColumnName(string columnName)
        {
            var header = GetHeaders().FirstOrDefault(h => h.Name == columnName);
            return header?.ColumnIndex ?? -1;
            
        }
    }
}