IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Conv_GetUnitId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_Conv_GetUnitId]
go

Create FUNCTION [dbo].[fn_Conv_GetUnitId]
	(@itemuom char(2), @casesize int)
	 RETURNS int
AS
BEGIN  
	DECLARE @return int

	select @return = (select Unit_Id
	 from   ItemUnit
	 where  Unit_Abbreviation = 
			(case 
				when @itemuom in ('LB', 'RW') then 'LB'
				when @itemuom not in ('LB', 'RW') and @casesize > 1 then 'CS'
				else 'EA' end))		
	RETURN @return
END
go
 