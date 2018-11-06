CREATE PROCEDURE dbo.CheckIdentifierInItemVendor
    @Identifier varchar(20),
    @Vendor_ID int
AS 
SET NOCOUNT ON

declare @tmp table (CntId int, CntVend int)
insert into @tmp values(0,0)
update @tmp 
set CntVend = (select count(*)
               FROM ItemIdentifier
                   INNER JOIN 
                       Item 
                       on Item.Item_key = ItemIdentifier.Item_Key
                   INNER JOIN 
                       ItemVendor 
                       ON (Item.Item_Key = ItemVendor.Item_Key) 
               WHERE ItemIdentifier.Identifier = @Identifier AND ItemIdentifier.Deleted_Identifier = 0 and 
                     ItemVendor.Vendor_ID = @Vendor_ID and 
                    (ItemVendor.DeleteDate is null or ItemVendor.DeleteDate > CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)))),

   CntId = (SELECT COUNT(*) AS IdentifierCount
               FROM ItemIdentifier
               WHERE Identifier = @Identifier AND Deleted_Identifier = 0)

select * from @tmp

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIdentifierInItemVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIdentifierInItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIdentifierInItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIdentifierInItemVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIdentifierInItemVendor] TO [IRMAExcelRole]
    AS [dbo];

