if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetProdHierarchyLevel3s_ByDescriptionStartsWith]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetProdHierarchyLevel3s_ByDescriptionStartsWith]
GO

CREATE PROCEDURE [dbo].[GetProdHierarchyLevel3s_ByDescriptionStartsWith] 
	@Category_ID as int,
	@Start varchar(52)
AS

BEGIN

SELECT @Start = @Start + '%'

select 
	rtrim(Description) [Value],
	ProdHierarchyLevel3_ID [ID] 
from 
	ProdHierarchyLevel3 
where 
	Description like @Start
	and 
	Category_ID = @Category_ID
order by 
	Description

END 

go