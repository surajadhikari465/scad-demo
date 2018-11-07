SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PSIStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PSIStore]
GO


CREATE PROCEDURE [dbo].[PSIStore]
AS

--**************************************************************************
-- Procedure: PSIStore
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with 
--                   StoreItemVendor.DiscontinueItem
--**************************************************************************
-- 20071121 DaveStacey - Rewrote joins, added error handling
	-- Exclude deleted and discontinued items, join VCH to get correct cost and case size, changed price/multiple to reg
	BEGIN TRY

			declare @NOW datetime
			
			select @NOW = (getdate())


		SELECT SIV.Store_No AS Store, II.Identifier AS UPC, SIV.Vendor_ID, 
			P.POSPrice AS Retail_Price, 
			 P.Multiple AS Units, 
			VCH.unitcost AS Cost, 0 AS Unit_Case, VCH.Package_Desc1
		FROM dbo.StoreItemVendor SIV (NOLOCK) 
			JOIN dbo.StoreItem SI (NOLOCK) ON si.Store_no = SIV.Store_no AND SI.Item_Key = SIV.Item_Key
			JOIN dbo.Item I (NOLOCK) ON I.Item_Key = SIV.Item_Key 
			JOIN dbo.ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key 
			JOIN dbo.Store S (NOLOCK) ON S.Store_no = SIV.Store_no 
			JOIN dbo.Price P (NOLOCK) ON I.Item_Key = P.Item_Key and P.Store_no = SIV.Store_no  
			JOIN dbo.VendorCostHistory VCH (NOLOCK) ON VCH.StoreItemVendorID = SIV.StoreItemVendorID
		WHERE SI.Authorized = 1	
			AND II.Default_Identifier = 1 
			AND I.Deleted_Item = 0 
			AND SIV.DiscontinueItem = 0 
			AND VCH.VendorCostHistoryID = (Select Top 1 VCH2.VendorCostHistoryID 
					FROM dbo.VendorCostHistory VCH2 (nolock) 
					WHERE SIV.StoreItemVendorID = VCH2.StoreItemVendorID 
						AND @NOW BETWEEN VCH2.startdate AND VCH2.enddate
					ORDER BY VCH2.VendorCostHistoryID Desc) 
			AND S.WFM_Store = 1
		ORDER BY SIV.Store_No, II.Identifier

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('PSIStore failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


