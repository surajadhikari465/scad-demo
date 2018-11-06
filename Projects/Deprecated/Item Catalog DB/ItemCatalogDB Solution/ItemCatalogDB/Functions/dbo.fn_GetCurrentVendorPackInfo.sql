IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetCurrentVendorPackInfo]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetCurrentVendorPackInfo]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_GetCurrentVendorPackInfo]
(
	@Item_Key int
	,@Store_No int
	,@Vendor_ID int
	,@ReturnType int
)
RETURNS numeric(14,4)
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/20/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.
										Changed variable from @Now to @costDate.
    05/17/2011  Min Zhao       1996     Updated to check the IgnoreCasePack flag on the ItemVendor table. If the flag is on (true), 
	                                    return the Retail Case Pack; otherwise, return the Vendor Pack. 
	1/7/2013    Alex B		   9615		Fixed bug to return decimal value ,not to trim to  the whole number 
*/
BEGIN
	DECLARE @RetVal numeric(14,4)
	DECLARE @costDate DATETIME
	
	-- Build target cost date that includes any lead-time for the vendor.
	SELECT @costDate = GETDATE() + dbo.fn_GetLeadTimeDays(@Vendor_ID)
	
	IF @ReturnType = 1 --Vendor Pack
		BEGIN  
		  SELECT @RetVal = (
			  SELECT
				  TOP 1 (CASE WHEN ISNULL(IV.IgnoreCasePack,0) = 1 
							THEN IV.RetailCasePack 
							ELSE VCH.Package_Desc1 
							END) AS Package_Desc1
			  FROM 
				  VendorCostHistory VCH (nolock)
				  JOIN StoreItemVendor SIV (NoLock) ON VCH.StoreItemVendorID = SIV.StoreItemVendorID
				  LEFT OUTER JOIN ItemVendor IV (NoLock) ON SIV.Vendor_ID = IV.Vendor_ID
															AND SIV.Item_Key = IV.Item_Key
			  WHERE SIV.Item_Key = @Item_Key 
			    AND SIV.Store_No = @Store_No 
			    AND SIV.Vendor_ID = @Vendor_ID
				AND VCH.StartDate <= @costDate
				AND VCH.EndDate >= @costDate
			  ORDER BY
					VendorCostHistoryID DESC
		  )
		END
	ELSE IF @ReturnType = 2 --Vendor Pack UOM
		BEGIN
		  SELECT @RetVal = (
			  SELECT
				  TOP 1 CostUnit_ID
			  FROM 
				  VendorCostHistory VCH (nolock)
			  WHERE
					VCH.StoreItemVendorID = 
										    (SELECT StoreItemVendorID 
										     FROM StoreItemVendor 
										     WHERE Item_Key = @Item_Key AND Store_No = @Store_No AND Vendor_ID = @Vendor_ID)
						AND StartDate <= @costDate
						AND EndDate >= @costDate
			  ORDER BY
					VendorCostHistoryID DESC
		  )
		END
		
		RETURN @RetVal	
END
GO
