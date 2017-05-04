CREATE   PROCEDURE dbo.Administration_POSPush_UpdatePOSWriterFileConfig
@POSFileWriterKey int,
@POSChangeTypeKey int,
@RowOrder int,
@ColumnOrder int,
@BitOrder tinyint,
@DataElement varchar(100),
@FieldID varchar(20),
@MaxFieldWidth int,
@TruncLeft bit,
@DefaultValue varchar(10),
@IsTaxFlag bit, 
@IsLiteral bit,
@IsPackedDecimal bit,
@IsBinaryInt bit,
@IsDecimalValue bit,
@DecimalPrecision int,
@IncludeDecimal bit,
@PadLeft bit,
@FillChar varchar(1),
@LeadingChars varchar(10),
@TrailingChars varchar(10),
@IsBoolean bit,
@BooleanTrueChar varchar(10),
@BooleanFalseChar varchar(10),
@FixedWidthField bit,
@PackLength int

AS
-- Update an existing configuration record in the POSWriterFileConfig table for the
-- POS Push process.
BEGIN
   UPDATE POSWriterFileConfig SET 
   	DataElement=@DataElement,
   	FieldID=@FieldID, 
   	MaxFieldWidth=@MaxFieldWidth,
   	TruncLeft=@TruncLeft,
   	DefaultValue=@DefaultValue,
   	IsTaxFlag=@IsTaxFlag, 
   	IsLiteral=@IsLiteral,
   	IsPackedDecimal=@IsPackedDecimal, 
   	IsBinaryInt=@IsBinaryInt, 
   	IsDecimalValue=@IsDecimalValue, 
   	DecimalPrecision=@DecimalPrecision,
   	IncludeDecimal=@IncludeDecimal,
   	PadLeft=@PadLeft, 
	FillChar=@FillChar,
	LeadingChars=@LeadingChars,
	TrailingChars=@TrailingChars,
	IsBoolean=@IsBoolean,
	BooleanTrueChar=@BooleanTrueChar,
	BooleanFalseChar=@BooleanFalseChar,
	FixedWidthField=@FixedWidthField,
	PackLength=@PackLength
   WHERE POSFileWriterKey = @POSFileWriterKey 
	AND POSChangeTypeKey=@POSChangeTypeKey 
	AND RowOrder=@RowOrder
   	AND ColumnOrder=@ColumnOrder
   	AND BitOrder=@BitOrder
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriterFileConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriterFileConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriterFileConfig] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriterFileConfig] TO [IRMAReportsRole]
    AS [dbo];

