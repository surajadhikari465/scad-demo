CREATE PROCEDURE amz.SendErrorNotificationEmail
(
       @CheckMessagesBeforeThisNumberOfMinutes INT,
	   @Recipients Varchar(200),
	   @Subject  Varchar(200),
	   @RunAsOfDate DateTime,
	   @Sender Varchar(200)
)

AS
BEGIN

	DECLARE @query VARCHAR(2048);
	DECLARE @queryForMail VARCHAR(2048);
	DECLARE @DBNAME VARCHAR(200)
	DECLARE @NumberOfFailedMessages INT
	DECLARE @Body VARCHAR(200) = 'test'

	IF EXISTS (SELECT * FROM Version WHERE Environment = 'TEST')
		   BEGIN
				 SET @DBNAME = 'ItemCatalog_TEST'
		   END
	ELSE
		   BEGIN
				 SET @DBNAME = 'ItemCatalog'
		   END

	SET @query = ('EXEC ' + @DBNAME+  '.AMZ.GetMessagesWithError @RunAsOfDate = '+ ''''+ Convert(varchar(20), @RunAsOfDate) + '''' +  ', @CheckMessagesBeforeThisNumberOfMinutes =  ' + Convert(varchar(5),@CheckMessagesBeforeThisNumberOfMinutes) )
	
	EXECUTE( @query)

	IF (EXISTS (Select 1 FROM ##tmpOrderInfoAmazon))
	BEGIN
		SET @queryForMail = 'SELECT * FROM ##tmpOrderInfoAmazon'
		SET @Body = 'Please see attached file for list of failed messages.'

		BEGIN TRY  
			EXEC msdb.dbo.sp_send_dbmail  
				 @recipients = @Recipients,  
				 @query = @queryForMail,
				 @attach_query_result_as_file = 1,
				 @query_result_separator =';',
				 @exclude_query_output =1,
				 @query_result_no_padding=1,
				 @query_attachment_filename = 'ErrorMessages.csv',
				 @body = @Body,  
				 @subject = @Subject,
				 @from_address = @Sender
		END TRY  

		BEGIN CATCH  
			Print 'Error Happened'
		END CATCH  

	END
	 
	IF OBJECT_ID('tempdb..##tmpOrderInfoAmazon') IS NOT NULL
		DROP TABLE ##tmpOrderInfoAmazon

END
GO

GRANT EXECUTE
    ON OBJECT::[amz].[SendErrorNotificationEmail] TO [IRMAPDXExtractRole]
    AS [dbo];
GO