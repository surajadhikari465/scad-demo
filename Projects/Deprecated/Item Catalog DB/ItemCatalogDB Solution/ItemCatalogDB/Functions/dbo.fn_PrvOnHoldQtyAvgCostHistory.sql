  /****** Object:  UserDefinedFunction [dbo].[fn_PrvOnHoldQtyAvgCostHistory]    Script Date: 11/30/2007 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_PrvOnHoldQtyAvgCostHistory')
    DROP FUNCTION fn_PrvOnHoldQtyAvgCostHistory
GO
-- =============================================
-- Author:		Sekhara
-- Create date: 11/30/2007
-- Function to fetch the Previous OnHand Quantity for the specific item 
--  based on store and SubTeam.
-- =============================================
CREATE FUNCTION [dbo].[fn_PrvOnHoldQtyAvgCostHistory] 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int)
RETURNS smallmoney
AS
BEGIN
	 -- Declare the variables to store the values returned by FETCH.
    DECLARE @PrvOnHoldQty smallmoney
    DECLARE @Count Int

    -- Assign 0 to the Counter Variable.
    select @Count=0

    -- Declare the Cursor to fetch top 2 records from the Query
    DECLARE PrvOnHoldQty_cursor CURSOR FOR
    SELECT TOP 2 Quantity
    FROM AvgCostHistory (nolock)
    WHERE Item_Key = ISNULL(@Item_Key,Item_Key)
    AND Store_No = ISNULL(@Store_No,store_No)
    AND SubTeam_No = ISNULL(@SubTeam_No,SubTeam_No)
    AND Effective_Date <= GETDATE()
    ORDER BY Effective_Date DESC


    -- Opening the Cursor.
	OPEN PrvOnHoldQty_cursor

	-- Perform the first fetch.
	FETCH NEXT FROM PrvOnHoldQty_cursor INTO @PrvOnHoldQty

	-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
	WHILE @@FETCH_STATUS = 0
	BEGIN
       select @Count=@Count+1
       -- This is executed as long as the previous fetch succeeds.
        FETCH NEXT FROM PrvOnHoldQty_cursor INTO @PrvOnHoldQty
	END

	CLOSE PrvOnHoldQty_cursor
	DEALLOCATE PrvOnHoldQty_cursor
    if @Count=1 
	Select @PrvOnHoldQty=Null
	  
RETURN @PrvOnHoldQty
END
GO 