create procedure dbo.LabelType_Add( @LabelTypeDesc varchar(4), @LabelTypeID int output )
as
begin
    insert  into LabelType
    (       LabelTypeDesc
    )
    select  @LabelTypeDesc

    set @LabelTypeID = SCOPE_IDENTITY()
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LabelType_Add] TO [IRMAClientRole]
    AS [dbo];

