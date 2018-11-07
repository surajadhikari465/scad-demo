SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'WFMM_GetVendorPackage')
	BEGIN
		DROP Procedure [dbo].WFMM_GetVendorPackage
	END
GO

CREATE PROCEDURE [dbo].[WFMM_GetVendorPackage]
	  @Store_No				int,
	  @Vendor_ID			int,
	  @Identifier			varchar(13)
AS
-- **************************************************************************
-- Procedure: WFMM_GetVendorPackage()
--    Author: Hui Kou
--      Date: 09.19.12
--
-- Description:
-- This procedure is called from the WFM Mobile app to return item vendor package size
-- order interface
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 09.19.12		Hk   	7427	Creation
-- **************************************************************************
BEGIN
    SET NOCOUNT ON  
	Declare @Item_Key    int
	
	SELECT @Item_Key = item_key 
		FROM dbo.ItemIdentifier (nolock)
		WHERE identifier = @Identifier and deleted_identifier = 0 and remove_identifier = 0 

	
	SELECT [VendorPackage]= IsNull((SELECT top 1 vp.Package_Desc1 
		FROM dbo.fn_VendorCostAllPackSizes(GETDATE()) vp 
		WHERE vp.store_no=@Store_No and vp.item_key=@Item_Key and vp.vendor_id=@Vendor_ID order by vp.vendorcosthistoryID desc),0.00)

    SET NOCOUNT OFF
END
GO