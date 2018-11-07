CREATE PROCEDURE dbo.Administration_POSPush_GetPOSWriterFileConfigurationForEdit
    @FileWriterKey int,
    @ChangeTypeKey int,
    @RowOrder int,
    @ColumnOrder int = NULL
AS
-- Selects the POSWriterFileConfig details for a 
-- writer, by change type, for the configuration
-- of the writer details.

BEGIN
SELECT w.POSFileWriterCode, ct.ChangeTypeDesc, wfc.RowOrder, wfc.ColumnOrder, 
	CASE wfc.IsBinaryInt WHEN 1 THEN wfc.BitOrder ELSE NULL END AS BitOrder,
	wfc.IsBinaryInt, wfc.IsLiteral, wfc.IsTaxFlag, wfc.DataElement, wfc.FieldID, wfc.MaxFieldWidth,
	wfc.TruncLeft, wfc.DefaultValue, 
	wfc.IsDecimalValue, wfc.IsPackedDecimal, wfc.DecimalPrecision, wfc.IncludeDecimal,
	wfc.PadLeft, wfc.FillChar, wfc.LeadingChars, wfc.TrailingChars,
	wfc.IsBoolean, wfc.BooleanTrueChar, wfc.BooleanFalseChar, wfc.FixedWidthField, wfc.PackLength  
FROM
	POSWriterFileConfig wfc, POSWriter w, POSChangeType ct 
WHERE
	wfc.POSFileWriterKey = @FileWriterKey AND
	wfc.POSFileWriterKey = w.POSFileWriterKey AND 
	wfc.POSChangeTypeKey = @ChangeTypeKey AND 
	wfc.POSChangeTypeKey = ct.POSChangeTypeKey AND
	wfc.RowOrder = @RowOrder AND 
	wfc.ColumnOrder = ISNULL(@ColumnOrder, wfc.ColumnOrder) AND 
	w.Disabled=0  
ORDER BY 
	wfc.POSFileWriterKey, wfc.POSChangeTypeKey, wfc.ColumnOrder, wfc.BitOrder

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigurationForEdit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigurationForEdit] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigurationForEdit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigurationForEdit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigurationForEdit] TO [IRMAReportsRole]
    AS [dbo];

