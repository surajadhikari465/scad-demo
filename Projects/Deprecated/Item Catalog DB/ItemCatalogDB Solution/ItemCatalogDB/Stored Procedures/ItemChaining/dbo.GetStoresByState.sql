if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoresByState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoresByState]
GO

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
go