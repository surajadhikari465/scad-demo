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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemChains_ByDescriptionStartsWith] TO [IRMAClientRole]
    AS [dbo];

