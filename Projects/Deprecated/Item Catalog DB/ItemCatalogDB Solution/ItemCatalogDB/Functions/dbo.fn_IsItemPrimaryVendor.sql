 IF EXISTS (SELECT * 
	   FROM   sysobjects 
	   WHERE  name = N'fn_IsItemPrimaryVendor')
	DROP FUNCTION fn_IsItemPrimaryVendor
GO

-- This function checks to see if a a vendor is primary for a specified item and store
Create  FUNCTION [dbo].[fn_IsItemPrimaryVendor] (
	@Item_Key int,
	@Store_No int,
	@Vendor_ID int
)
RETURNS bit
AS

BEGIN  
	RETURN ISNULL((SELECT PrimaryVendor FROM StoreItemVendor WHERE Item_Key = @Item_Key AND Store_No = @Store_No AND Vendor_ID = @Vendor_ID), 0)
END

GO
