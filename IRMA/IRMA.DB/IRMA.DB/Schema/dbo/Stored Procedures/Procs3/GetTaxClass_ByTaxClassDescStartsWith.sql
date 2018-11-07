CREATE PROCEDURE [dbo].[GetTaxClass_ByTaxClassDescStartsWith] 
	@Start varchar(52)
AS

BEGIN
	SELECT @Start = @Start + '%'

	select 
		rtrim(TaxClassDesc) [Value],
		TaxClassID [ID] 
	from TaxClass
	where 
		TaxClassDesc like @Start
	order by 
		TaxClassDesc

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTaxClass_ByTaxClassDescStartsWith] TO [IRMASLIMRole]
    AS [dbo];

