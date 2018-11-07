-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:41:31 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: InsertPushQueue;1 - Script Date: 11/3/2008 4:41:31 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertPushQueue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertPushQueue]
GO

CREATE PROCEDURE dbo.InsertPushQueue
	@POHeaderID int,
	@UserID int

AS
BEGIN

	update POHeader 
	set PushedByUserID = @UserID
	Where POHeaderID = @POHeaderID
	
	insert into PushToIRMAQueue (POHeaderID, ProcessingFlag) values(@POHeaderID, 0)

END

GO
