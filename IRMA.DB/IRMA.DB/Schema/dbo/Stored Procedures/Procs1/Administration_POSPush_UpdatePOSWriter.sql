CREATE PROCEDURE dbo.Administration_POSPush_UpdatePOSWriter
@POSFileWriterKey int, 
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
-- Update an existing configuration record in the POSWriter table for the
-- POS Push process.
BEGIN
   UPDATE POSWriter SET
   	POSFileWriterCode=@POSFileWriterCode, 
	POSFileWriterClass=@POSFileWriterClass, 
   	DelimChar=@DelimChar,
   	OutputByIrmaBatches=@OutputByIrmaBatches,
   	FixedWidth=@FixedWidth,
   	LeadingDelim=@LeadingDelim,
   	TrailingDelim=@TrailingDelim, 
   	FieldIdDelim=@FieldIdDelim,
   	TaxFlagTrueChar=@TaxFlagTrueChar,
   	TaxFlagFalseChar=@TaxFlagFalseChar, 
   	EnforceDictionary=@EnforceDictionary,
   	AppendToFile=@AppendToFile,
   	FileWriterType=@FileWriterType,
   	ScaleWriterType=@ScaleWriterType,
   	BatchIdMin=@BatchIdMin,
   	BatchIdMax=@BatchIdMax
   WHERE POSFileWriterKey = @POSFileWriterKey
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriter] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriter] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriter] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriter] TO [IRMAReportsRole]
    AS [dbo];

