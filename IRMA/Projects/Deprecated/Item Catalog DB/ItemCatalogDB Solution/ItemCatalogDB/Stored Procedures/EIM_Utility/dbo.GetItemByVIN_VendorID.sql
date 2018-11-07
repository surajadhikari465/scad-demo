-- This script was created using WinSQL Professional
-- Timestamp: 6/25/2009 6:45:53 AM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetItemByVIN_VendorID;1 - Script Date: 6/25/2009 6:45:53 AM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemByVIN_VendorID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemByVIN_VendorID]
GO
CREATE PROCEDURE dbo.GetItemByVIN_VendorID
	
	(
	  @Item_ID varchar(12),
      @Vendor_ID int
	)
	
AS
	/* SET NOCOUNT ON */ 
	
	SELECT
        ItemVendor.Item_Key, ItemIdentifier.Identifier
        
FROM
        ItemVendor (nolock)
        inner join ItemIdentifier (nolock)
        on ItemVendor.item_key = ItemIdentifier.item_key
     
WHERE
        ItemVendor.Item_ID = @Item_ID
        AND ItemVendor.Vendor_ID = @Vendor_ID
        AND ItemVendor.DeleteDate is Null
        AND ItemIdentifier.Default_Identifier = 1
        AND ItemIdentifier.Deleted_Identifier = 0
        
	
	RETURN


GO
