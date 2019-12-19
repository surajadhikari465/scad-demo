using Dapper;
using System.Configuration;
using System.Data.SqlClient;

namespace BulkItemUploadProcessor.DataAccess.Tests
{
    public static class TestHelpers
    {
        public static SqlConnection Icon => new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);

        internal static int AddHierarchyClass(SqlConnection sqlConnection, int hierarchyId, string hierarchyClassName = "Test")
        {
            return sqlConnection.QueryFirst<int>(
                @"INSERT INTO dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel)
                VALUES(@HierarchyClassName, @HierarchyId, @HierarchyLevel)

                SELECT SCOPE_IDENTITY()",
                new
                {
                    HierarchyClassName = hierarchyClassName,
                    HierarchyId = hierarchyId,
                    HierarchyLevel = 1
                });
        }

        internal static int AddItemType(SqlConnection sqlConnection, string itemTypeCode = "TST", string itemTypeDesc = "Test")
        {
            return sqlConnection.QueryFirst<int>(
                @"INSERT INTO dbo.ItemType(itemTypeCode, itemTypeDesc)
                VALUES(@ItemTypeCode, @ItemTypeDesc)

                SELECT SCOPE_IDENTITY()",
                new
                {
                    ItemTypeCode = itemTypeCode,
                    ItemTypeDesc = itemTypeDesc
                });
        }

        internal static int AddBarcodeType(SqlConnection sqlConnection, string barcodeType = "TestBarcodeType")
        {
            return sqlConnection.QueryFirst<int>(
                @"INSERT INTO dbo.BarcodeType(BarcodeType)
                VALUES(@BarcodeType)

                SELECT SCOPE_IDENTITY()",
                new
                {
                    BarcodeType = barcodeType
                });
        }

        internal static int AddScanCodeToBarcodeTypeRangePool(SqlConnection sqlConnection, int barcodeTypeId, string scanCode, bool assigned = false)
        {
            return sqlConnection.Execute(
                @"INSERT INTO dbo.BarcodeTypeRangePool(BarcodeTypeId, ScanCode, Assigned)
                VALUES(@BarcodeTypeId, @ScanCode, @Assigned)",
                new
                {
                    BarcodeTypeId = barcodeTypeId,
                    ScanCode = scanCode,
                    Assigned = assigned
                });
        }
    }
}
