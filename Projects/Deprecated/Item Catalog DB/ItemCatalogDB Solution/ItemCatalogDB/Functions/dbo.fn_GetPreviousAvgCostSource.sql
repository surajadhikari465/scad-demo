 SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_GetPreviousAvgCostSource')
    DROP FUNCTION fn_GetPreviousAvgCostSource
GO

CREATE FUNCTION [dbo].[fn_GetPreviousAvgCostSource] 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int,
     @EffectiveDate datetime)
RETURNS varchar(25)
AS
BEGIN
	 -- Declare the variables to store the values returned by FETCH.
    DECLARE @PrvAvgCostSource varchar(25)
    DECLARE @Count Int

    -- Assign 0 to the Counter Variable.
    select @Count=0

    -- Declare the Cursor to fetch top 2 records from the Query
    DECLARE PrvAvgCost_cursor CURSOR FOR
    SELECT TOP 2 u.UserName
    FROM AvgCostHistory (nolock)
    LEFT JOIN Users u ON u.User_ID = AvgCostHistory.User_ID
    WHERE Item_Key = ISNULL(@Item_Key,Item_Key)
    AND Store_No = ISNULL(@Store_No,store_No)
    AND SubTeam_No = ISNULL(@SubTeam_No,SubTeam_No)
    AND Effective_Date <= @EffectiveDate
    ORDER BY Effective_Date DESC


    -- Opening the Cursor.
	OPEN PrvAvgCost_cursor

	-- Perform the first fetch.
	FETCH NEXT FROM PrvAvgCost_cursor INTO @PrvAvgCostSource

	-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
	WHILE @@FETCH_STATUS = 0
	BEGIN
       select @Count=@Count+1
       -- This is executed as long as the previous fetch succeeds.
        FETCH NEXT FROM PrvAvgCost_cursor INTO @PrvAvgCostSource
	END

	CLOSE PrvAvgCost_cursor
	DEALLOCATE PrvAvgCost_cursor
    if @Count=1 
	Select @PrvAvgCostSource=Null
	  
RETURN @PrvAvgCostSource
END
GO 