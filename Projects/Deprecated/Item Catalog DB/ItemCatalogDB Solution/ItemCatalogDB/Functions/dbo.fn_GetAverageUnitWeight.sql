GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GetAverageUnitWeight]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_GetAverageUnitWeight]
GO

CREATE function [dbo].[fn_GetAverageUnitWeight]
	(@Identifier varchar(13))
returns decimal(9,4)
as
begin
	DECLARE @AverageUnitWeight decimal(9,4)
	
	SELECT 
		@AverageUnitWeight = ISNULL(i.Average_Unit_Weight,0)
	FROM item					(nolock) i
	INNER JOIN ItemIdentifier	(nolock) ii ON i.Item_Key = ii.Item_Key
	WHERE ii.Identifier = @Identifier AND ii.Default_Identifier = 1

	RETURN @AverageUnitWeight
end
GO