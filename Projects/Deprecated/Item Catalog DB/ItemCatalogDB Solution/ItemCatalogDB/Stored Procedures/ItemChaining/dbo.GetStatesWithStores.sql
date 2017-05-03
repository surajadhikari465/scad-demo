if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStatesWithStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStatesWithStores]
GO

create procedure dbo.GetStatesWithStores as

	select distinct 
		State 
	from StoresVW
	order by 
		State
go