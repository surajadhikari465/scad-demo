CREATE PROCEDURE dbo.DeleteScansMissingVimData (
	@ReportHeaderId int
	)
AS
BEGIN

 -- save
	DELETE FROM dbo.ScansMissingVimData WHERE Report_Header_Id = @ReportHeaderId


END