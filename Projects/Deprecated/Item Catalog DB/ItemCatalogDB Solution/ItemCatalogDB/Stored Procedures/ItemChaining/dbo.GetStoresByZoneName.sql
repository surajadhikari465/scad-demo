if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoresByZoneName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoresByZoneName]
GO

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
go