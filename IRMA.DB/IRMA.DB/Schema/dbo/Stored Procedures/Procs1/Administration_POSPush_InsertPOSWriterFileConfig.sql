CREATE   PROCEDURE dbo.Administration_POSPush_InsertPOSWriterFileConfig
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
-- Insert a new configuration record into the POSWriterFileConfig table for the
-- POS Push process.
BEGIN
   INSERT INTO POSWriterFileConfig (POSFileWriterKey, POSChangeTypeKey, RowOrder, ColumnOrder, BitOrder, 
   				DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsBinaryInt, 
				IsTaxFlag, IsLiteral, IsPackedDecimal, IsDecimalValue, DecimalPrecision,
				IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars,
				IsBoolean, BooleanTrueChar, BooleanFalseChar, FixedWidthField, PackLength) 
   VALUES (@POSFileWriterKey, @POSChangeTypeKey, @RowOrder, @ColumnOrder, @BitOrder, 
   		@DataElement, @FieldID, @MaxFieldWidth, @TruncLeft, @DefaultValue, @IsBinaryInt,
		@IsTaxFlag, @IsLiteral, @IsPackedDecimal, @IsDecimalValue, @DecimalPrecision, @IncludeDecimal,
		@PadLeft, @FillChar, @LeadingChars, @TrailingChars,
		@IsBoolean, @BooleanTrueChar, @BooleanFalseChar, @FixedWidthField, @PackLength)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertPOSWriterFileConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertPOSWriterFileConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertPOSWriterFileConfig] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertPOSWriterFileConfig] TO [IRMAReportsRole]
    AS [dbo];

