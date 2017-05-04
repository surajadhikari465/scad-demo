-- ===================================================================
-- Author:		Sekhara
-- Create date: 11/30/2007
-- Function to fetch the Previous AverageCost for for the specific item 
--  based on store and SubTeam.
-- =======================================================================
CREATE FUNCTION [dbo].[fn_PrvAvgCostHistory] 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int)
RETURNS smallmoney
AS
BEGIN
	 -- Declare the variables to store the values returned by FETCH.
    DECLARE @AvgCost smallmoney
    DECLARE @Count Int

    select @Count=0
    
    -- Declare the Cursor to fetch top 2 records from the Query
    DECLARE PrvAvgCost_cursor CURSOR FOR
    SELECT TOP 2 AvgCost
    FROM AvgCostHistory (nolock)
    WHERE Item_Key = ISNULL(@Item_Key,Item_Key)
    AND Store_No = ISNULL(@Store_No,store_No)
    AND SubTeam_No = ISNULL(@SubTeam_No,SubTeam_No)
    AND Effective_Date <= GETDATE()
    ORDER BY Effective_Date DESC

    -- Opening the Cursor.
	OPEN PrvAvgCost_cursor

	-- Perform the first fetch.
	FETCH NEXT FROM PrvAvgCost_cursor INTO @AvgCost

	-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
	WHILE @@FETCH_STATUS = 0
	BEGIN
       select @Count=@Count+1
       -- This is executed as long as the previous fetch succeeds.
        FETCH NEXT FROM PrvAvgCost_cursor INTO @AvgCost
	END

	CLOSE PrvAvgCost_cursor
	DEALLOCATE PrvAvgCost_cursor
    if @Count=1 
	    Select @AvgCost=Null
	  
	RETURN @AvgCost
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PrvAvgCostHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PrvAvgCostHistory] TO [IRMAReportsRole]
    AS [dbo];

