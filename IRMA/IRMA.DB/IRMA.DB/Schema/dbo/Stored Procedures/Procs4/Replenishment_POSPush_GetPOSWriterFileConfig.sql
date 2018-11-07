CREATE PROCEDURE dbo.Replenishment_POSPush_GetPOSWriterFileConfig
    @FileWriterKey int
AS 
-- Queries the POSWriterFileConfig table to
-- retrieve all the change type details for the  
-- specified file writer.

BEGIN
SELECT 
POSChangeTypeKey, ColumnOrder, RowOrder, BitOrder, DataElement, 
FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, 
IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar,
LeadingChars, TrailingChars, IsBoolean, BooleanTrueChar, BooleanFalseChar,
FixedWidthField, IsPackedDecimal, IsBinaryInt, PackLength 
FROM POSWriterFileConfig 
WHERE (POSFileWriterKey = @FileWriterKey) 
ORDER BY POSChangeTypeKey, RowOrder, ColumnOrder, BitOrder
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPOSWriterFileConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPOSWriterFileConfig] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPOSWriterFileConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPOSWriterFileConfig] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPOSWriterFileConfig] TO [IRMAReportsRole]
    AS [dbo];

