CREATE PROCEDURE dbo.Administration_POSPush_InsertPOSWriter
@POSFileWriterCode varchar(20),
@POSFileWriterClass varchar(100),
@DelimChar varchar(10),
@OutputByIrmaBatches bit,
@FixedWidth bit,
@LeadingDelim bit,
@TrailingDelim bit, 
@FieldIdDelim bit,
@TaxFlagTrueChar char(1), 
@TaxFlagFalseChar char(1),
@EnforceDictionary bit,
@AppendToFile bit,
@FileWriterType varchar(10),
@ScaleWriterType int,
@BatchIdMin int,
@BatchIdMax int
AS
-- Insert a new configuration record into the POSWriter table for the
-- POS Push process.
BEGIN
   INSERT INTO POSWriter (POSFileWriterCode, POSFileWriterClass, DelimChar, OutputByIrmaBatches, FixedWidth, 
			LeadingDelim, TrailingDelim, FieldIdDelim, TaxFlagTrueChar, TaxFlagFalseChar, 
			EnforceDictionary, AppendToFile, FileWriterType, ScaleWriterType, BatchIdMin, BatchIdMax) 
   VALUES (@POSFileWriterCode, @POSFileWriterClass, @DelimChar, @OutputByIrmaBatches, @FixedWidth,  
			@LeadingDelim, @TrailingDelim, @FieldIdDelim, @TaxFlagTrueChar, @TaxFlagFalseChar, 
			@EnforceDictionary, @AppendToFile, @FileWriterType, @ScaleWriterType, @BatchIdMin, @BatchIdMax)
		
	--return primary key value of item that was just inserted for foreign key data to be added to POSEscapeChars table
	SELECT SCOPE_IDENTITY() AS POSFileWriterKey

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertPOSWriter] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertPOSWriter] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertPOSWriter] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertPOSWriter] TO [IRMAReportsRole]
    AS [dbo];

