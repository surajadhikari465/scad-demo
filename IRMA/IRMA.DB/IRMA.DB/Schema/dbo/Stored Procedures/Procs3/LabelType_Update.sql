create procedure dbo.LabelType_Update( @LabelTypeID int, @LabelTypeDesc varchar(4) )
as
begin
    update  LabelType
    set     LabelTypeDesc = @LabelTypeDesc
    where   LabelType_ID = @LabelTypeID
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LabelType_Update] TO [IRMAClientRole]
    AS [dbo];

