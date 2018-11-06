
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF Exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SendAutoPushFailureNotification]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SendAutoPushFailureNotification]
GO

CREATE PROCEDURE [dbo].[SendAutoPushFailureNotification]

AS 

DECLARE @NumOfRows			INT
DECLARE @PreviousRowIndex	INT
DECLARE @CurrentRowIndex	INT
DECLARE @PreviousEmail		VARCHAR(100)
DECLARE @PreviousUser		VARCHAR(50)
DECLARE @CurrentEmail		VARCHAR(100)
DECLARE @CurrentUser		VARCHAR(50)
DECLARE @PreviousVendor		VARCHAR(50)
DECLARE @CurrentVendor		VARCHAR(50)
DECLARE @MessageHeader		VARCHAR(MAX)
DECLARE @VendorHeader		VARCHAR(MAX)
DECLARE @POETPOList			VARCHAR(MAX) = '' 
DECLARE @CreateDate			DATETIME
DECLARE @ExpectedDate		DATETIME
DECLARE @AutoPushDate		DATETIME
DECLARE @MessageBody		VARCHAR(MAX) = ''
DECLARE @SubjectLine		VARCHAR(MAX) = 'POET - Auto Push to IRMA Failure Notification'
DECLARE @AddVendorHeader	BIT = 1


DECLARE @POList TABLE
(
RowID			INT NOT NULL IDENTITY(1,1),
UserName		VARCHAR(50),
Email			VARCHAR(100),
PONumberID		INT,
VendorName		VARCHAR(100),
CreatedDate		DATETIME,
ExpectedDate	DATETIME,
AutoPushDate	DATETIME
)

INSERT INTO @POList 
SELECT DISTINCT	
u.UserName, u.Email, pn.PONumber, ISNULL(po.VendorName,'N/A') AS VendorName, po.CreatedDate, po.ExpectedDate, po.AutoPushDate 
FROM PONumber pn INNER JOIN 
(SELECT DISTINCT ph1.PONumberID, ph1.POHeaderID, ph1.VendorName, ph1.CreatedDate, ph1.ExpectedDate, ph1.AutoPushDate, ph1.PushedToIRMADate 
 FROM POHeader ph1 (NOLOCK) INNER JOIN UploadSessionHistory ush (NOLOCK) 
 ON ph1.UploadSessionHistoryID = ush.UploadSessionHistoryID 
 WHERE ush.ValidationSuccessful = 1 AND
 ph1.AutoPushDate < GETDATE() AND 
 ph1.PushedToIRMADate IS NULL) po 
ON pn.PONumberID = po.PONumberID 
INNER JOIN Users u
ON pn.AssignedByUserID = u.UserID 
ORDER BY u.Email, VendorName 

SET @MessageHeader = 'Your POs that were scheduled for auto push were not successfully auto pushed to IRMA. Below is the information regarding the PO numbers that were impacted:' + CHAR(13)
SET @MessageHeader = @MessageHeader + 'Please contact the PO Tool Support alias (POToolSupport@wholefoods.com) for any assistance.' + CHAR(13) + CHAR(13)


SET @NumOfRows = (SELECT MAX(RowID) FROM @POList)

SET @CurrentRowIndex = 1
SET @PreviousRowIndex = 1
SET @PreviousEmail = (SELECT Email FROM @POList p WHERE p.RowID = @CurrentRowIndex)
SET @CurrentEmail  = @PreviousEmail
SET @PreviousUser = (SELECT UserName FROM @POList p WHERE p.RowID = @CurrentRowIndex)
SET @CurrentUser  = @PreviousUser
SET @PreviousVendor = (SELECT VendorName FROM @POList p WHERE p.RowID = @CurrentRowIndex)
SET @CurrentVendor = @PreviousVendor

select * from @POList  
select @NumOfRows

SELECT @CurrentVendor = p.VendorName, @CreateDate = p.CreatedDate, @ExpectedDate = p.ExpectedDate, @AutoPushDate = p.AutoPushDate FROM @POList p WHERE p.RowID = @CurrentRowIndex

SET @VendorHeader = 'Vendor: ' + @CurrentVendor + CHAR(13)
SET @VendorHeader = @POETPOList + 'Uploaded Date: ' + CONVERT(VARCHAR(25),@CreateDate) + CHAR(13)
SET @VendorHeader = @POETPOList + 'Expected Date: ' + CONVERT(VARCHAR(25),@ExpectedDate) + CHAR(13)
SET @VendorHeader = @POETPOList + 'Scheduled Auto Push Date: ' + CONVERT(VARCHAR(25),@AutoPushDate) + CHAR(13) + CHAR(13) + 'POET PO List: '

WHILE (@CurrentRowIndex <= @NumOfRows)
	BEGIN
		IF (@PreviousEmail = @CurrentEmail)

			BEGIN
				IF (@PreviousVendor = @CurrentVendor) 
					BEGIN
						SET @POETPOList = @POETPOList + CHAR(13) + CONVERT(VARCHAR(10),(SELECT p.PONumberID FROM @POList p WHERE p.RowID = @CurrentRowIndex))
					END
				ELSE
					BEGIN
						
						
						SELECT @PreviousVendor = p.VendorName, @CreateDate = p.CreatedDate, @ExpectedDate = p.ExpectedDate, @AutoPushDate = p.AutoPushDate FROM @POList p WHERE p.RowID = @PreviousRowIndex
						SET @VendorHeader = @VendorHeader + CHAR(13) + 'Vendor: ' + @PreviousVendor + CHAR(13)
						SET @VendorHeader = @VendorHeader + 'Uploaded Date: ' + CONVERT(VARCHAR(25),@CreateDate) + CHAR(13)
						SET @VendorHeader = @VendorHeader + 'Expected Date: ' + CONVERT(VARCHAR(25),@ExpectedDate) + CHAR(13)
						SET @VendorHeader = @VendorHeader + 'Scheduled Auto Push Date: ' + CONVERT(VARCHAR(25),@AutoPushDate) + CHAR(13)
						
						IF @AddVendorHeader = 1
							SET @POETPOList = @VendorHeader + @POETPOList + CHAR(13) + CHAR(13) 
						ELSE
							SET @POETPOList = @POETPOList + CHAR(13) + CHAR(13)
						
						SELECT @CurrentVendor = p.VendorName, @CreateDate = p.CreatedDate, @ExpectedDate = p.ExpectedDate, @AutoPushDate = p.AutoPushDate FROM @POList p WHERE p.RowID = @CurrentRowIndex
						SET @VendorHeader = 'Vendor: ' + @CurrentVendor + CHAR(13)
						SET @VendorHeader = @VendorHeader + 'Uploaded Date: ' + CONVERT(VARCHAR(25),@CreateDate) + CHAR(13)
						SET @VendorHeader = @VendorHeader + 'Expected Date: ' + CONVERT(VARCHAR(25),@ExpectedDate) + CHAR(13)
						SET @VendorHeader = @VendorHeader + 'Scheduled Auto Push Date: ' + CONVERT(VARCHAR(25),@AutoPushDate) + CHAR(13)
						
						
						SET @POETPOList = @POETPOList + @VendorHeader + CHAR(13) + CONVERT(VARCHAR(10),(SELECT p.PONumberID FROM @POList p WHERE p.RowID = @CurrentRowIndex))
												
						SET @AddVendorHeader = 0 

					END	
				
				SET @PreviousVendor = @CurrentVendor
			END
		ELSE
			BEGIN
				-- SEND EMAIL

				SELECT @PreviousVendor = p.VendorName, @CreateDate = p.CreatedDate, @ExpectedDate = p.ExpectedDate, @AutoPushDate = p.AutoPushDate FROM @POList p WHERE p.RowID = @PreviousRowIndex
				SET @VendorHeader = ''
				SET @VendorHeader = 'Vendor: ' + RTRIM(@PreviousVendor) + CHAR(13) + CHAR(13)
				SET @VendorHeader = @VendorHeader + 'Uploaded Date: ' + RTRIM(CONVERT(VARCHAR(25),@CreateDate)) + CHAR(13) 
				SET @VendorHeader = @VendorHeader + 'Expected Date: ' + RTRIM(CONVERT(VARCHAR(25),@ExpectedDate)) + CHAR(13) 
				SET @VendorHeader = @VendorHeader + 'Scheduled Auto Push Date: ' + RTRIM(CONVERT(VARCHAR(25),@AutoPushDate)) + CHAR(13)  
				
				IF @AddVendorHeader = 1
					SET @MessageBody = 'Dear ' + @PreviousUser + CHAR(13) + @MessageHeader + @VendorHeader + @POETPOList
				ELSE
					SET @MessageBody = 'Dear ' + @PreviousUser + CHAR(13) + @MessageHeader + @POETPOList
				
				
				EXEC msdb.dbo.sp_send_dbmail @recipients = @PreviousEmail, @from_address = 'POET@wholefoods.com', @body = @MessageBody, @subject = @SubjectLine 
				
				SET @MessageBody = ''
				SET @AddVendorHeader = 1
				SELECT @CurrentVendor = p.VendorName, @CreateDate = p.CreatedDate, @ExpectedDate = p.ExpectedDate, @AutoPushDate = p.AutoPushDate FROM @POList p WHERE p.RowID = @CurrentRowIndex
				
				IF (@PreviousVendor = @CurrentVendor) AND (@CurrentEmail = @PreviousEmail)
					BEGIN
						SET @VendorHeader  = CHAR(13) + 'Vendor: ' + @CurrentVendor + CHAR(13)
						SET @VendorHeader = @VendorHeader + 'Uploaded Date: ' + CONVERT(VARCHAR(25),@CreateDate) + CHAR(13)
						SET @VendorHeader = @VendorHeader + 'Expected Date: ' + CONVERT(VARCHAR(25),@ExpectedDate) + CHAR(13)
						SET @VendorHeader = @VendorHeader + 'Scheduled Auto Push Date: ' + CONVERT(VARCHAR(25),@AutoPushDate) + CHAR(13)  
						SET @POETPOList = CHAR(13) + @VendorHeader + CONVERT(VARCHAR(10),(SELECT p.PONumberID FROM @POList p WHERE p.RowID = @CurrentRowIndex))
					END
				ELSE
					SET @POETPOList = CHAR(13) + CONVERT(VARCHAR(10),(SELECT p.PONumberID FROM @POList p WHERE p.RowID = @CurrentRowIndex))
				
				SET @PreviousVendor = @CurrentVendor 
				
				SET @VendorHeader = ''
			END		
		
		SET @PreviousRowIndex =  @CurrentRowIndex
		SET @CurrentRowIndex = @CurrentRowIndex + 1
		
		IF @CurrentRowIndex > @NumOfRows 
			BEGIN
				-- SEND EMAIL
				SELECT @CurrentVendor = p.VendorName, @CreateDate = p.CreatedDate, @ExpectedDate = p.ExpectedDate, @AutoPushDate = p.AutoPushDate FROM @POList p WHERE p.RowID = @CurrentRowIndex - 1
				SET @VendorHeader  = CHAR(13) + 'Vendor: ' + @CurrentVendor + CHAR(13) + CHAR(13)
				SET @VendorHeader = @VendorHeader + 'Uploaded Date: ' + CONVERT(VARCHAR(25),@CreateDate) + CHAR(13)
				SET @VendorHeader = @VendorHeader + 'Expected Date: ' + CONVERT(VARCHAR(25),@ExpectedDate) + CHAR(13)
				SET @VendorHeader = @VendorHeader + 'Scheduled Auto Push Date: ' + CONVERT(VARCHAR(25),@AutoPushDate) + CHAR(13)  
				
				IF @AddVendorHeader = 1
					SET @MessageBody = 'Dear ' + @CurrentUser + CHAR(13) + @MessageHeader + +@VendorHeader + @POETPOList
				ELSE
					SET @MessageBody = 'Dear ' + @CurrentUser + CHAR(13) + @MessageHeader + @POETPOList
				
				EXEC msdb.dbo.sp_send_dbmail @recipients = @CurrentEmail, @from_address = 'POET@wholefoods.com', @body = @MessageBody, @subject = @SubjectLine 
				
				BREAK
			END 
		ELSE
			BEGIN
				SET @PreviousEmail = @CurrentEmail 
				SET @PreviousUser = @CurrentUser 
				SET @CurrentEmail  = (SELECT Email FROM @POList p WHERE p.RowID = @CurrentRowIndex)
				SET @CurrentUser = (SELECT UserName FROM @POList p WHERE p.RowID = @CurrentRowIndex)
				SET @PreviousVendor = @CurrentVendor 
				SET @CurrentVendor = (SELECT p.VendorName FROM @POList p WHERE p.RowID = @CurrentRowIndex)
				
			END 
	END
	
-- Send email to PO Tool Support with a list of users whose POs failed during auto push	
DECLARE @POsPerRegion TABLE
(
Region			VARCHAR(2),
TotalPOs		INT
)

INSERT INTO @POsPerRegion
	SELECT 
		r.RegionCode,
		COUNT(po.PONumberID)
	FROM @POList AS po
	INNER JOIN Users u ON po.UserName = u.UserName
	INNER JOIN Regions r ON r.RegionID = u.RegionID
	GROUP BY r.RegionCode 

DECLARE @UsersPerRegion TABLE
(
Region			VARCHAR(2),
TotalUsers		INT
)

INSERT INTO @UsersPerRegion
	SELECT 
		r.RegionCode,
		COUNT(po.UserName)
	FROM (SELECT DISTINCT UserName FROM @POList) po
	INNER JOIN Users u ON po.UserName = u.UserName
	INNER JOIN Regions r ON r.RegionID = u.RegionID
	GROUP BY r.RegionCode 
	
DECLARE @FailurePerRegion TABLE
(
RowID			INT NOT NULL IDENTITY(1,1),
Region			VARCHAR(2),
TotalPOs		INT,
TotalUsers		INT
)

INSERT INTO @FailurePerRegion
	SELECT 
		pr.Region,
		pr.TotalPOs,
		ur.TotalUsers
	FROM @POsPerRegion pr 
	INNER JOIN @UsersPerRegion ur ON pr.Region = ur.Region
	
DECLARE @POListByRegion TABLE
(
RowID			INT NOT NULL IDENTITY(1,1),
Region			VARCHAR(2),
PONumberID		INT,
UserName		VARCHAR(50)
)	
	
INSERT INTO @POListByRegion
	SELECT
		r.RegionCode,
		po.PONumberID,
		u.UserName
	FROM @POList po
	INNER JOIN Users u ON po.UserName = u.UserName
	INNER JOIN Regions r ON r.RegionID = u.RegionID
	ORDER BY r.RegionCode, u.UserID 

select * from @POListByRegion

DECLARE @NumOfRegions	INT
DECLARE @Region			VARCHAR(2)
DECLARE @TotalPOs		INT
DECLARE @TotalUsers		INT
DECLARE @PONumber		INT
DECLARE	@UserID			VARCHAR(50)
DECLARE	@CurrentRegion	VARCHAR(2)
DECLARE @Env			VARCHAR(10)

SET @NumOfRegions = ISNULL((SELECT COUNT(*) FROM @FailurePerRegion), 0)
SET @CurrentRowIndex = 1

SELECT @Env = KeyValue FROM AppSettings WHERE KeyName = 'Environment'  

IF @NumOfRows > 0 
	BEGIN
		SET @SubjectLine = 'POET ' + @Env + ': Auto Push Failure - Date & Time of failure: ' + CONVERT(VARCHAR(25),GETDATE())

		SET @MessageBody = '' + CHAR(13)
		SET @MessageBody = @MessageBody + 'Impact of POET Auto Push Failure detailed below:' + CHAR(13) + CHAR(13) 

		SET @MessageBody = @MessageBody + 'Region        No Of POs       No Of Users' + CHAR(13)
		SET @MessageBody = @MessageBody + '-----------------------------------------------------' + CHAR(13)

		WHILE (@CurrentRowIndex <= @NumOfRegions)
			BEGIN
				SELECT 
					@Region		= r.Region, 
					@TotalPOs	= r.TotalPOs, 
					@TotalUsers = r.TotalUsers
				FROM @FailurePerRegion r WHERE r.RowID = @CurrentRowIndex
			
				SET @MessageBody = @MessageBody + @Region + REPLICATE(' ', 20)+ CONVERT(VARCHAR(5), @TotalPOs)  + REPLICATE(' ', 30) + CONVERT(VARCHAR(5), @TotalUsers) + CHAR(13)

				SET @CurrentRowIndex = @CurrentRowIndex + 1				
			END
			
		SET @MessageBody = @MessageBody + '-----------------------------------------------------' + CHAR(13) + CHAR(13) 
				
		SET @NumOfRows = ISNULL((SELECT COUNT(*) FROM @POListByRegion), 0)
		SET @CurrentRowIndex = 1
		
		IF @NumOfRows > 0 
			BEGIN
				SELECT 
					@CurrentRegion	= r.Region, 
					@PONumber		= r.PONumberID, 
					@UserID			= r.UserName
				FROM @POListByRegion r WHERE r.RowID = @CurrentRowIndex

				SET @MessageBody = @MessageBody + 'Failed PO List (' + @CurrentRegion + ')' +CHAR(13)
				SET @MessageBody = @MessageBody + CONVERT(VARCHAR(11), @PONumber)  + REPLICATE(' ', 15) + CONVERT(VARCHAR(30), @UserID) + CHAR(13)
				
				SET @CurrentRowIndex = @CurrentRowIndex + 1	
						
				WHILE (@CurrentRowIndex <= @NumOfRows)
					BEGIN
						SELECT 
							@Region		= r.Region, 
							@PONumber	= r.PONumberID, 
							@UserID		= r.UserName
						FROM @POListByRegion r WHERE r.RowID = @CurrentRowIndex
					
						IF @Region = @CurrentRegion 
							BEGIN
								SET @MessageBody = @MessageBody + CONVERT(VARCHAR(11), @PONumber)  + REPLICATE(' ', 15) + CONVERT(VARCHAR(30), @UserID) + CHAR(13)			
							END
						ELSE
							BEGIN		
								SET @CurrentRegion = @Region	
								SET @MessageBody = @MessageBody + CHAR(13) + 'Failed PO List (' + @CurrentRegion + ')' +CHAR(13)
								SET @MessageBody = @MessageBody + CONVERT(VARCHAR(11), @PONumber)  + REPLICATE(' ', 15) + CONVERT(VARCHAR(30), @UserID) + CHAR(13)
							END
							
						SET @CurrentRowIndex = @CurrentRowIndex + 1						
					END
			END
	
			PRINT @SubjectLine
			PRINT @MessageBody

			EXEC msdb.dbo.sp_send_dbmail @recipients = 'POToolSupport@wholefoods.com', @from_address = 'POET@wholefoods.com', @body = @MessageBody, @subject = @SubjectLine 				
	END
	GO
