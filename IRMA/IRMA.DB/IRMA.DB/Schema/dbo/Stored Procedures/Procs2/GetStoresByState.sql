create procedure dbo.GetStoresByState 
	@State varchar(2)
as
begin

select 
	* 
from 
	StoresVW
where 
	State=@State
ORDER BY 
	Mega_Store DESC, 
	WFM_Store DESC, 
	Store_Name
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoresByState] TO [IRMAClientRole]
    AS [dbo];

