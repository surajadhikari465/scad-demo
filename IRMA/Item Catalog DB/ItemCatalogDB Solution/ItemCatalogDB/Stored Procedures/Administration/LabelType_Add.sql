if exists( select 'TRUE' from sys.objects o where o.object_id = OBJECT_ID(N'LabelType_Add') )
begin
    drop procedure dbo.LabelType_Add
end
go


create procedure dbo.LabelType_Add( @LabelTypeDesc varchar(4), @LabelTypeID int output )
as
begin
    insert  into LabelType
    (       LabelTypeDesc
    )
    select  @LabelTypeDesc

    set @LabelTypeID = SCOPE_IDENTITY()
end
go

