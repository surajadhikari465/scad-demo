-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:34:52 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetSessionsWithExceptionsByUserID;1 - Script Date: 11/3/2008 4:34:53 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSessionsWithExceptionsByUserID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSessionsWithExceptionsByUserID]
GO

CREATE PROCEDURE dbo.GetSessionsWithExceptionsByUserID
	@UserID int

AS
BEGIN
select
	UploadSessionHistoryID
	, rtrim(cast(UploadSessionHistoryID as char(10))) + ': ' + isnull(FileName,'...') + ' - ' + convert(char(10),UploadedDate,101) as SessionDescription
	from
	UploadSessionHistory
	where 
	UploadUserID = @UserID
	order by UploadedDate desc

END




GO
