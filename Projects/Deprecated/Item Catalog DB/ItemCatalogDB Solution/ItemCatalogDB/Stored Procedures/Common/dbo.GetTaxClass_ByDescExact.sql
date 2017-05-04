 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTaxClass_ByDescExact]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetTaxClass_ByDescExact]
GO

CREATE PROCEDURE dbo.GetTaxClass_ByDescExact
	@TaxClassDesc varchar(50)
AS 

SELECT 
	TaxClassID,
	TaxClassDesc
FROM 
	TaxClass
WHERE 
	TaxClassDesc = @TaxClassDesc

GO