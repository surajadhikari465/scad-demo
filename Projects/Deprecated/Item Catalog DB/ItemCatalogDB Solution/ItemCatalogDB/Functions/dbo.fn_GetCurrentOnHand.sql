 /****** Object:  UserDefinedFunction [dbo].[fn_GetCurrentOnHand]    Script Date: 12/07/2007 */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_GetCurrentOnHand')
    DROP FUNCTION fn_GetCurrentOnHand
GO
-- ==========================================================================
-- Author:		Sekhara
-- Create date: 12/07/2007
-- Function to fetch the Current OnHand Quantity for a specific item 
--  based on store and SubTeam.
-- ===========================================================================
CREATE FUNCTION [dbo].[fn_GetCurrentOnHand] 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int)
RETURNS decimal(10,4)
AS
BEGIN
	declare @CurrentOnHand decimal(10,4)
	select @CurrentOnHand = (SELECT Top 1 Quantity  from onHand
	where Item_Key=@Item_Key 
	and Store_No=@Store_no 
	and SubTeam_No=@SubTeam_No 
	order by LastReset Desc)

RETURN @CurrentOnHand
END
GO
 