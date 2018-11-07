create procedure dbo.GetStatesWithStores as

	select distinct 
		State 
	from StoresVW
	order by 
		State
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStatesWithStores] TO [IRMAClientRole]
    AS [dbo];

