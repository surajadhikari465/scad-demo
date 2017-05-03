   /****** Object:  UserDefinedFunction [dbo].[fn_PrvOnHoldQtyAvgCostHistory]    Script Date: 11/30/2007 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_GetPreviousAvgCost')
    DROP FUNCTION fn_GetPreviousAvgCost
GO

CREATE FUNCTION [dbo].[fn_GetPreviousAvgCost] 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int,
     @EffectiveDate datetime)
RETURNS smallmoney
AS
BEGIN
	 -- Declare the variables to store the values returned by FETCH.
    DECLARE @PrvAvgCost smallmoney
    DECLARE @Count Int

    -- Assign 0 to the Counter Variable.
    select @Count=0

    -- Declare the Cursor to fetch top 2 records from the Query
    DECLARE PrvAvgCost_cursor CURSOR FOR
    SELECT TOP 2 AvgCost
    FROM AvgCostHistory (nolock)
    WHERE Item_Key = ISNULL(@Item_Key,Item_Key)
    AND Store_No = ISNULL(@Store_No,store_No)
    AND SubTeam_No = ISNULL(@SubTeam_No,SubTeam_No)
    AND Effective_Date <= @EffectiveDate
    ORDER BY Effective_Date DESC


    -- Opening the Cursor.
	OPEN PrvAvgCost_cursor

	-- Perform the first fetch.
	FETCH NEXT FROM PrvAvgCost_cursor INTO @PrvAvgCost

	-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
	WHILE @@FETCH_STATUS = 0
	BEGIN
       select @Count=@Count+1
       -- This is executed as long as the previous fetch succeeds.
        FETCH NEXT FROM PrvAvgCost_cursor INTO @PrvAvgCost
	END

	CLOSE PrvAvgCost_cursor
	DEALLOCATE PrvAvgCost_cursor
    if @Count=1 
	Select @PrvAvgCost=Null
	  
RETURN @PrvAvgCost
END
GO 