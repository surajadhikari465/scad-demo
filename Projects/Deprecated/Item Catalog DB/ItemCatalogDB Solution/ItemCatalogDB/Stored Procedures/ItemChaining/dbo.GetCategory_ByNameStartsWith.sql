if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCategory_ByNameStartsWith]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCategory_ByNameStartsWith]
GO

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

go