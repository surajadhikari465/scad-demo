
create procedure dbo.Scale_AddAllergensToItem
	@Item_Key int,
	@AllergensDescription varchar(50),
	@Allergens varchar(4200),
	@LabelTypeId int
as
begin
	declare @allergensId int

	insert into Scale_Allergen(Description, Scale_LabelType_ID, Allergens)
	values(@AllergensDescription, @LabelTypeId, @Allergens)

	set @allergensId = SCOPE_IDENTITY()

	update ItemScale
	set Scale_Allergen_ID = @allergensId
	where Item_Key = @Item_Key
end

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [dbo.Scale_AddAllergensToItem.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_AddAllergensToItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_AddAllergensToItem] TO [IRSUser]
    AS [dbo];

