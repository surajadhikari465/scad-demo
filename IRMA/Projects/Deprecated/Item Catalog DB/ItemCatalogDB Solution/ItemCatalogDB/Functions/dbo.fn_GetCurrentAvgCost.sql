 IF EXISTS (SELECT * 
	   FROM   sysobjects 
	   WHERE  name = N'fn_GetCurrentAvgCost')
	DROP FUNCTION fn_GetCurrentAvgCost
GO

CREATE FUNCTION dbo.fn_GetCurrentAvgCost 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int,
     @Effective_Date datetime)
RETURNS smallmoney
AS
BEGIN
    DECLARE @AvgCost smallmoney

    SELECT @AvgCost =   (SELECT TOP 1 AvgCost
                         FROM AvgCostHistory (nolock)
                         WHERE Item_Key = @Item_Key
                             AND Store_No = @Store_No
                             AND SubTeam_No = @SubTeam_No
                             AND Effective_Date <= @Effective_Date
                         ORDER BY Effective_Date DESC)

    RETURN @AvgCost
END
GO