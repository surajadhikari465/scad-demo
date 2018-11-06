 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GLAcctIncludesTeamSubteam]') and xtype in (N'FN', N'IF', N'TF'))
    drop function [dbo].[fn_GLAcctIncludesTeamSubteam]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION dbo.fn_GLAcctIncludesTeamSubteam 
(
	@SubTeamType_ID tinyint,
	@ProductType_ID int,
	@OrderHeader_ID int
)
RETURNS bit
AS
BEGIN
    DECLARE @Result bit
    DECLARE @StorePurchase bit
    
	SELECT @StorePurchase = COUNT(*) 
							FROM 
								OrderHeader 
							WHERE
								OrderHeader_ID = @OrderHeader_ID
							AND 
								Transfer_To_SubTeam = SupplyTransferToSubTeam -- if these two are equal, then the purchase is for the store and not a specific team

	IF @ProductType_ID = 3 AND @StorePurchase = 1 -- if other supplies and a store purchase
		SELECT @Result = 1
	ELSE
		SELECT @Result = CASE WHEN @SubTeamType_ID IN (1,2,3,6,7) THEN 1 ELSE 0 END
    
	RETURN @Result
END
GO