if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemAuthorization]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemAuthorization]
GO

CREATE PROCEDURE dbo.GetItemAuthorization
(
        @Store_No int,
        @ItemKey int
)
AS
-- ****************************************************************************************************************
-- Procedure: GetItemAuthorization()
--    Author: unknown
--      Date: unknown
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-09-10   FA		13661	Add transaction isolation level
-- ****************************************************************************************************************

BEGIN
    SET NOCOUNT ON

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	SELECT     ItemVendor.Item_Key, ItemVendor.Vendor_ID, Vendor.CompanyName, ItemVendor.Item_ID AS Warehouse, Store.Store_Name,
					  StoreItemVendor.StoreItemVendorID, CASE WHEN StoreItemVendor.StoreItemVendorID IS NULL THEN 'N' ELSE 'Y' END AS Authorized,
					  case when StoreItemVendor.PrimaryVendor = 0 then 'N' else 'Y' end as PrimVen,
						  case when StoreItemVendor.StoreItemVendorID IS NULL or StoreItemVendor.PrimaryVendor = 0 then 'Authorize' else 'De-Authorize' end as Action1,
						  case when StoreItemVendor.PrimaryVendor = 0 then 'Set Primary' else 'Delete Primary' end as Action2
	FROM         ItemVendor INNER JOIN
						  Vendor ON ItemVendor.Vendor_ID = Vendor.Vendor_ID LEFT OUTER JOIN
						  StoreItemVendor ON ItemVendor.Item_Key = StoreItemVendor.Item_Key AND ItemVendor.Vendor_ID = StoreItemVendor.Vendor_ID AND
						  StoreItemVendor.Store_No = @Store_No LEFT OUTER JOIN
						  Store ON StoreItemVendor.Store_No = Store.Store_No
	WHERE     (ItemVendor.Item_Key = @ItemKey)
	
	COMMIT TRAN
END

GO

