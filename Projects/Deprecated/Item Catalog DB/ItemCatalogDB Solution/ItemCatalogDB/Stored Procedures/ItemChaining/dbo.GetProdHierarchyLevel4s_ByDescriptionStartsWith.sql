if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetProdHierarchyLevel4s_ByDescriptionStartsWith]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetProdHierarchyLevel4s_ByDescriptionStartsWith]
GO

CREATE PROCEDURE [dbo].[GetProdHierarchyLevel4s_ByDescriptionStartsWith] 
	@ProdHierarchyLevel3_ID as int,
	@Start varchar(52)
AS

BEGIN

SELECT @Start = @Start + '%'

select 
	rtrim(Description) [Value],
	ProdHierarchyLevel4_ID [ID] 
from 
	ProdHierarchyLevel4 
where 
	Description like @Start
	and 
	ProdHierarchyLevel3_ID = @ProdHierarchyLevel3_ID
order by 
	Description

END
go