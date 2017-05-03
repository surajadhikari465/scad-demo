if exists( select 'TRUE' from sys.objects o where o.object_id = OBJECT_ID(N'LabelType_Update') )
begin
    drop procedure dbo.LabelType_Update
end
go


create procedure dbo.LabelType_Update( @LabelTypeID int, @LabelTypeDesc varchar(4) )
as
begin
    update  LabelType
    set     LabelTypeDesc = @LabelTypeDesc
    where   LabelType_ID = @LabelTypeID
end
go

