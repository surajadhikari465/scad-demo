CREATE PROCEDURE [dbo].[GetCategory_ByNameStartsWith] 
	@SubTeam_No as int,
	@Start varchar(52)
AS

BEGIN

SELECT @Start = @Start + '%'

select 
	rtrim(Category_Name) [Value],
	Category_ID [ID] 
from 
	ItemCategory 
where 
	Category_Name like @Start
	and 
	SubTeam_No = @SubTeam_No
order by 
	Category_Name

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategory_ByNameStartsWith] TO [IRMAClientRole]
    AS [dbo];

