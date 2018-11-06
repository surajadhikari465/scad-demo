SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PSIMovement]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PSIMovement]
GO

CREATE PROCEDURE [dbo].[PSIMovement] 
AS
--**************************************************************************
-- Procedure: PSIMovement
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
-- 20071121 DaveStacey - Rewrote joins, added error handling
	-- Change Biz Unit to Store_No
	BEGIN TRY
		DECLARE @Start as datetime, @End as DateTime, @EndText VARCHAR(10)
		SET DateFirst 1

		SELECT @Start = CONVERT(VARCHAR(10),DATEADD(d,1-(DATEPART(dw, GETDATE())+7),GETDATE()),101),
			   @End = CONVERT(VARCHAR(10),DATEADD(d,-1 * (DATEPART(dw, GETDATE())),GETDATE()),101)
		SELECT @EndText = CONVERT(VARCHAR(10), @End, 101)

		SELECT SBI.Store_No,       
			   II.Identifier AS UPC,
			   SUM(dbo.Fn_ItemSalesQty(II.Identifier, IU.Weight_Unit, SBI.Price_Level, 
								   SBI.Sales_Quantity, SBI.Return_Quantity, I.Package_Desc1, SBI.Weight))  AS 'MVTWK', 
			   @EndText AS WeekendingDate,
			   CAST(dbo.fn_GetDiscontinueStatus(I.Item_Key, S.Store_No, NULL) AS VARCHAR(1)) AS Discontinue_Item, 
			   I.Item_Key
		FROM dbo.Sales_SumByItem SBI (nolock)   
			JOIN dbo.Item I (nolock) ON I.Item_Key = SBI.Item_Key
			JOIN dbo.ItemIdentifier II (nolock) ON II.Item_Key = I.Item_Key
			JOIN dbo.ItemUnit IU (nolock) ON I.Retail_Unit_ID = IU.Unit_ID
			JOIN dbo.Store S (nolock) ON S.Store_No = SBI.Store_No
		WHERE S.WFM_Store = 1 
			AND II.Default_Identifier = 1 
			AND SBI.Date_Key BETWEEN @Start AND @End
			AND I.Deleted_Item = 0 		
		GROUP BY SBI.Store_No, dbo.fn_GetDiscontinueStatus(I.Item_Key, S.Store_No, NULL), I.Item_Key, II.Identifier
		ORDER BY SBI.Store_No, I.Item_Key

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('PSIMovement failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

