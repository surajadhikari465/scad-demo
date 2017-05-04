if exists (select * from sysobjects where sysobjects.name = 'EXEInterfaces_CleanWorkspace' and sysobjects.xtype = 'p')
drop procedure dbo.EXEInterfaces_CleanWorkspace
go


create procedure dbo.EXEInterfaces_CleanWorkspace
(
@uniqueid varchar(255)
) as
begin
	delete from EXEInterfaces_ZeroShippedOrdersValidationWorkspace
	where uniqueid = @uniqueid
end
go


GRANT EXECUTE ON [dbo].[EXEInterfaces_CleanWorkspace] TO [IRMAAdminRole]
GRANT EXECUTE ON [dbo].[EXEInterfaces_CleanWorkspace] TO [IRMAClientRole]
GO