if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemChains_ByDescriptionStartsWith]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemChains_ByDescriptionStartsWith]
GO

CREATE PROCEDURE [dbo].[GetItemChains_ByDescriptionStartsWith] 
	@Start varchar(52)
AS

BEGIN

	SELECT @Start = @Start + '%'

	select 
		rtrim(ItemChainDesc) [Value],
		ItemChainId [ID] 
	from 
		ItemChain 
	where 
		ItemChainDesc like @Start
	order by 
		ItemChainDesc
END 
go