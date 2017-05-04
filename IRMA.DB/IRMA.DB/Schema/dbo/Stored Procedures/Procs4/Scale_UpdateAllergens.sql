
create procedure dbo.Scale_UpdateAllergens
	@Scale_Allergen_ID int,
	@AllergensDescription varchar(50),
	@Allergens varchar(4200),
	@LabelTypeId int
as
begin
	update Scale_Allergen
	set Description = @AllergensDescription,
		Allergens = @Allergens,
		Scale_LabelType_ID = @LabelTypeId
	where Scale_Allergen_ID = @Scale_Allergen_ID
end

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [dbo.Scale_UpdateAllergens.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_UpdateAllergens] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_UpdateAllergens] TO [IRSUser]
    AS [dbo];

