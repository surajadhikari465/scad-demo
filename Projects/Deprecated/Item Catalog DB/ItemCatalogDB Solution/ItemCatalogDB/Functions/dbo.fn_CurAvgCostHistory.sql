 /****** Object:  UserDefinedFunction [dbo].[fn_CurAvgCostHistory]    Script Date: 11/30/2007 */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_CurAvgCostHistory')
    DROP FUNCTION fn_CurAvgCostHistory
GO
-- ==========================================================================
-- Author:		Sekhara
-- Create date: 11/30/2007
-- Function to fetch the Current Average Cost for the specific item 
--  based on store and SubTeam.
-- ===========================================================================
CREATE FUNCTION [dbo].[fn_CurAvgCostHistory] 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int)
RETURNS VARCHAR(60)
AS
BEGIN
	DECLARE @AvgCost smallmoney
    SELECT @AvgCost =   (SELECT TOP 1 AvgCost
                         FROM AvgCostHistory (nolock)
                         WHERE Item_Key = ISNULL(@Item_Key,Item_Key)
						 AND Store_No = ISNULL(@Store_No,store_No)
						 AND SubTeam_No = ISNULL(@SubTeam_No,SubTeam_No)
                         AND Effective_Date <= GETDATE()
                         ORDER BY Effective_Date DESC)   
    RETURN @AvgCost
END
GO
