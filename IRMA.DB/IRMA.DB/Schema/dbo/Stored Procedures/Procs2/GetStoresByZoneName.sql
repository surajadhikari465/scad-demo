create procedure dbo.GetStoresByZoneName
	@Zone_Name varchar(20)
as
begin

select 
	* 
from 
	StoresVW
where 
	Zone_Name=@Zone_Name
ORDER BY 
	Mega_Store DESC, 
	WFM_Store DESC, 
	Store_Name
	
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoresByZoneName] TO [IRMAClientRole]
    AS [dbo];

