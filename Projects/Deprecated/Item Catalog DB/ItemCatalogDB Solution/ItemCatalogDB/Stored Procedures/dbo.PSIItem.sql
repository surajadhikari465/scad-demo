SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PSIItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PSIItem]
GO


CREATE PROCEDURE [dbo].[PSIItem]
AS
--**************************************************************************
-- Procedure: PSIItem
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
-- 20071121 DaveStacey - Rewrote joins, added error handling
	BEGIN TRY

		SELECT II.Identifier AS UPC, 
			   I.Item_Key,
			   REPLACE(I.Item_Description,',',' ') AS Item_Full_Desc,
			   REPLACE(I.POS_Description,',',' ') AS Item_Desc,
			   I.Package_Desc2,
			   IU.Unit_Name,
			   I.Package_Desc1,
			   I.SubTeam_No,
			   (CASE WHEN I.Deleted_Item = 1 THEN 'DELETED' WHEN dbo.fn_GetDiscontinueStatus(I.Item_Key, NULL, NULL) = 1 THEN 'DISCONTINUED' ELSE 'ACTIVE' END) AS Status
		FROM dbo.Item AS I (nolock) 
			JOIN dbo.ItemIdentifier AS II (nolock) ON II.Item_Key = I.Item_Key
			JOIN dbo.ItemUnit AS IU (nolock) ON I.Package_Unit_ID = IU.Unit_ID
		WHERE II.Default_Identifier = 1

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('PSIItem failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

