if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTaxClass_ByTaxClassDescStartsWith]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetTaxClass_ByTaxClassDescStartsWith]
GO

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
go 