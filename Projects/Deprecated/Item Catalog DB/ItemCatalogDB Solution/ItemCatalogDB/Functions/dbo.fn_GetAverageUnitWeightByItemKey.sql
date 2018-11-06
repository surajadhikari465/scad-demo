
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GetAverageUnitWeightByItemKey]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_GetAverageUnitWeightByItemKey]
GO

CREATE function [dbo].[fn_GetAverageUnitWeightByItemKey]
	(@Item_Key int)
returns decimal(9,4)
as
begin
	DECLARE @AverageUnitWeight decimal(9,4)
	
	SELECT 
		@AverageUnitWeight = i.Average_Unit_Weight
	FROM item					(nolock) i
	INNER JOIN ItemIdentifier	(nolock) ii ON i.Item_Key = ii.Item_Key 
											AND ii.Default_Identifier = 1
	WHERE i.Item_Key = @Item_Key 
	RETURN @AverageUnitWeight
end
GO
