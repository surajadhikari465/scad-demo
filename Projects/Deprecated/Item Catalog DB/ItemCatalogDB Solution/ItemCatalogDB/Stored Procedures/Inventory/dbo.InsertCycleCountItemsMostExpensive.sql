SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCycleCountItemsMostExpensive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCycleCountItemsMostExpensive]
GO


CREATE PROCEDURE dbo.InsertCycleCountItemsMostExpensive
	@CycleCountID int
	,@SubTeam_No int
	,@Top varchar(5)
	,@InvLocID int = null

AS 

--**************************************************************************
-- Procedure: InsertCycleCountItemsMostExpensive
--
-- Revision:
-- 01/08/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
SET NOCOUNT ON

DECLARE
	@Error_No int
	,@SQL varchar(2500)

SELECT @Error_No = 0

BEGIN TRAN

	IF @InvLocID IS NOT NULL
	BEGIN    
        SELECT @SQL =        'INSERT INTO InventoryLocationItems (InvLocID, Item_Key) '
        SELECT @SQL = @SQL + 'SELECT TOP ' + @Top + ' WITH TIES ' + CAST(@InvLocID AS varchar(10)) + ', Item.Item_Key '
        SELECT @SQL = @SQL + 'FROM Item (nolock) '
        SELECT @SQL = @SQL + 'INNER JOIN '
        SELECT @SQL = @SQL + '    Price (nolock) '
        SELECT @SQL = @SQL + '    ON Price.Item_Key = Item.Item_Key '
        SELECT @SQL = @SQL + 'LEFT JOIN '
        SELECT @SQL = @SQL + '    InventoryLocationItems IL (nolock) '
        SELECT @SQL = @SQL + '    ON Item.Item_Key = IL.Item_Key AND IL.InvLocID = ' + CAST(@InvLocID AS varchar(10)) + ' '
        SELECT @SQL = @SQL + 'WHERE Item.SubTeam_No = ' + CAST(@SubTeam_No AS varchar(10)) + ' ' 
        SELECT @SQL = @SQL + '    AND Price.Store_No = (SELECT CycleCountMaster.Store_No FROM CycleCountMaster (nolock) INNER JOIN CycleCountHeader (nolock) ON CycleCountMaster.MasterCountID = CycleCountHeader.MasterCountID WHERE CycleCountHeader.CycleCountID =  ' + CAST(@CycleCountID AS varchar(10)) + ') '
        SELECT @SQL = @SQL + '    AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = 0 '
        SELECT @SQL = @SQL + '    AND IL.Item_Key IS NULL '
        SELECT @SQL = @SQL + '    AND EXISTS (SELECT * FROM StoreItemVendor SIV (nolock) WHERE SIV.Store_No = Price.Store_No AND SIV.Item_Key = Item.Item_Key AND DeleteDate IS NULL) '
        SELECT @SQL = @SQL + 'ORDER BY dbo.fn_AvgCostHistory(Item.Item_Key, Price.Store_No, Item.SubTeam_No, GETDATE()) DESC'

		EXECUTE(@SQL)
	
		SELECT @Error_No = @@Error

	END

	IF @Error_No = 0 
	BEGIN  
        SELECT @SQL =        'INSERT INTO CycleCountItems (CycleCountID, Item_Key) '
        SELECT @SQL = @SQL + 'SELECT TOP ' + @Top + ' WITH TIES ' + CAST(@CycleCountID AS varchar(10)) + ', Item.Item_Key '
        SELECT @SQL = @SQL + 'FROM Item (nolock) '
        SELECT @SQL = @SQL + 'INNER JOIN '
        SELECT @SQL = @SQL + '    Price (nolock) ON (Price.Item_Key = Item.Item_Key) '
        SELECT @SQL = @SQL + 'LEFT JOIN '
        SELECT @SQL = @SQL + '    CycleCountItems CI (nolock) '
        SELECT @SQL = @SQL + '    ON Item.Item_Key = CI.Item_Key AND CI.CycleCountID = ' + CAST(@CycleCountID AS varchar(10)) + ' '
        SELECT @SQL = @SQL + 'WHERE Item.SubTeam_No = ' + CAST(@SubTeam_No AS varchar(10)) + ' ' 
        SELECT @SQL = @SQL + '    AND Price.Store_No = (SELECT CycleCountMaster.Store_No FROM CycleCountMaster (nolock) INNER JOIN CycleCountHeader (nolock) ON CycleCountMaster.MasterCountID = CycleCountHeader.MasterCountID WHERE CycleCountHeader.CycleCountID =  ' + CAST(@CycleCountID AS varchar(10)) + ') '
        SELECT @SQL = @SQL + '    AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = 0 '
        SELECT @SQL = @SQL + '    AND CI.CycleCountItemID IS NULL '
        SELECT @SQL = @SQL + '    AND EXISTS (SELECT * FROM StoreItemVendor SIV (nolock) WHERE SIV.Store_No = Price.Store_No AND SIV.Item_Key = Item.Item_Key AND DeleteDate IS NULL) '
        SELECT @SQL = @SQL + 'ORDER BY dbo.fn_AvgCostHistory(Item.Item_Key, Price.Store_No, Item.SubTeam_No, GETDATE()) DESC'
    
		EXECUTE(@SQL)
	
		SELECT @Error_No = @@Error

	END

	IF @Error_No = 0
		COMMIT TRAN
	ELSE
	BEGIN
		ROLLBACK TRAN
		DECLARE @Severity smallint
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
		RAISERROR ('InsertCycleCountItemsMostExpensive failed with @@ERROR: %d', @Severity, 1, @Error_No)
	END

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

