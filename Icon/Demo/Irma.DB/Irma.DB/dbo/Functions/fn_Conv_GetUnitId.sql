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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Conv_GetUnitId] TO [DataMigration]
    AS [dbo];

