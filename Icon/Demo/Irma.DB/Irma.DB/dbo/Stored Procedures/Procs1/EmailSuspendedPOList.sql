-- =============================================
-- Author:		Faisal Ahmed
-- Create date: 01/31/2011
-- Description:	<This procedure will send email a list of currently suspended POs to PO accountants.>
-- =============================================
CREATE PROCEDURE EmailSuspendedPOList
AS
BEGIN
	SET NOCOUNT ON;

	Declare @Names VARCHAR(MAX) 
	Declare @EmailMessage as varchar(max)
	Declare @EmailSubject as varchar(max)
	Declare @EmailRecipients as varchar(max)
	
	Declare @Count int
	
	SELECT @Names = COALESCE(@Names, '') + 'PO# ' + LTRIM(CAST(OrderHeader_ID as varchar(12))) + char(13) 
	FROM dbo.OrderHeader OH
	WHERE OH.CloseDate IS NOT NULL AND OH.ApprovedDate IS NULL

	SELECT @Count = COUNT(*)
	FROM dbo.OrderHeader OH
	WHERE OH.CloseDate IS NOT NULL AND OH.ApprovedDate IS NULL

	--print @Names

	set @EmailSubject = 'Suspended PO Alert'
	
	SELECT @EmailRecipients = COALESCE(@EmailRecipients + '', '') + LTRIM(CAST(u.EMail as varchar(MAX))) + ';'
	FROM Users u
	WHERE (u.PO_Accountant = 1 or u.POApprovalAdmin = 1) and u.AccountEnabled = 1
	
	declare @Environment varchar(20)
	select @Environment = RIGHT(@@servername,1)	
	If @Environment = 'D' or @Environment = 'T' or @Environment ='Q'
	Begin
		set @EmailRecipients = 'faisal.ahmed@wholefoods.com;Winston.Denny@Wholefoods.com;'
	end

	set @EmailMessage = 'Hello:' + char(13) + char(13)
	set @EmailMessage = @EmailMessage + 'There are currently ' + CAST(@count as varchar(25)) + ' suspended POs for your region.' + char(13) + char(13)
	set @EmailMessage = @EmailMessage +  'The following PO numbers have recently been suspended.' + char(13) + char(13) + @Names + char(13) 
	set @EmailMessage = @EmailMessage + 'This is a system-generated email from the IRMA client.'
	
	EXEC msdb.dbo.sp_send_dbmail 
		@profile_name = 'IRMA', 
		@recipients = @EmailRecipients, 
		@body = @EmailMessage, 
		@subject = @EmailSubject,
		@Body_format = 'TEXT';
END