-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:31:35 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetAvailablePONumberIDsByUserID;1 - Script Date: 11/3/2008 4:31:35 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvailablePONumberIDsByUserID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAvailablePONumberIDsByUserID]
GO

CREATE PROCEDURE dbo.GetAvailablePONumberIDsByUserID
	@UserID int

AS
BEGIN
	select PONumberID, PONumber
	from PONumber
	where AssignedByUserID = @UserID and PushedToIRMA = 0 and N.DeleteDate is null
END

GO
