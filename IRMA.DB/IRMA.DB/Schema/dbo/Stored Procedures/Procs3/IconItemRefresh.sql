
CREATE procedure [dbo].[IconItemRefresh] 
	@Identifiers varchar(max)
as
begin
	declare @newItemChangeType int = (select ItemChgTypeID from ItemChgType where ItemChgTypeDesc = 'New')

	insert into IconItemChangeQueue(Item_Key, Identifier, ItemChgTypeID, InsertDate)
	select ii.Item_Key,
		   ii.Identifier,		   
		   @newItemChangeType,
		   GETDATE()
	from fn_ParseStringList(@Identifiers, '|') list
	join ItemIdentifier ii on list.Key_Value = ii.Identifier
end

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [IconItemRefresh.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemRefresh] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemRefresh] TO [IRSUser]
    AS [dbo];

