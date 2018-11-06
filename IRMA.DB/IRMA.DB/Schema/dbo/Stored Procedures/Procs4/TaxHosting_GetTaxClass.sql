CREATE PROCEDURE dbo.TaxHosting_GetTaxClass
AS

/*********************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
----------------------------------------------------------------------------------------------------------------------------------------------
MYounes         Jan 20, 2011                1024                   Added ExternalTaxGroupCode field for display on ManageTaxClass.vb
**********************************************************************************************************************************************/

BEGIN
	SELECT 
		TaxClassID, 
		TaxClassDesc, 
		[ItemCount]		= (SELECT COUNT(1) FROM Item WHERE Item.TaxClassID = TaxClass.TaxClassID AND Item.Deleted_Item = 0),
		[TaxFlagCount]	= (SELECT COUNT(1) FROM TaxFlag WHERE TaxFlag.TaxClassID = TaxClass.TaxClassID),
		ExternalTaxGroupCode
	FROM 
		TaxClass
	ORDER BY 
		TaxClassDesc
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxClass] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxClass] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxClass] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxClass] TO [IRMAReportsRole]
    AS [dbo];

