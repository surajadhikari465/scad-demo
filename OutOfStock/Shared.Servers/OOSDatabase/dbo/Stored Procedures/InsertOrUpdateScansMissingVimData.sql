CREATE PROCEDURE dbo.InsertOrUpdateScansMissingVimData
	@ReportHeaderId int,
	@UPC varchar(13)
AS
BEGIN

	IF LEN(@UPC) < 13
		SET @upc = RIGHT('0000000000000' + @upc, 13);

	
	IF EXISTS (SELECT 1 FROM dbo.ScansMissingVimData WHERE Report_Header_Id = @ReportHeaderId AND upc= @upc)
	BEGIN
		UPDATE dbo.ScansMissingVimData
		SET ScanCount = ScanCount + 1
		WHERE Report_Header_Id = @reportHeaderId AND upc = @upc
	END  
	ELSE  
	BEGIN
		INSERT INTO dbo.ScansMissingVimData
		        ( Report_Header_Id, UPC, ScanCount )
		VALUES  ( @reportheaderid , -- Report_Header_Id - int
		          @upc, -- UPC - varchar(25)
		          1  -- ScanCount - int
		          )  

	END  
END;