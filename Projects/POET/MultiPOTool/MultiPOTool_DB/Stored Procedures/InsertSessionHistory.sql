-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:35:53 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: InsertSessionHistory;1 - Script Date: 11/3/2008 4:35:53 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertSessionHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertSessionHistory]
GO
CREATE PROCEDURE dbo.InsertSessionHistory
	
	(
	@UserID int,
	@FileName varchar(100),
	@UploadedDate datetime,
	@SessionHIstoryID int OUTPUT
	)
	
AS
	SET NOCOUNT ON 
	
	INSERT INTO UploadSessionHistory
	                      (UploadUserID, FileName, UploadedDate)
	VALUES     (@UserID,@FileName,@UploadedDate)
	
	
	 set @SessionHistoryID = scope_identity()
	 
	 RETURN
	 
	


GO
