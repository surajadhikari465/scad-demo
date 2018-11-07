-- This script was created using WinSQL Professional
-- Timestamp: 11/14/2008 4:31:43 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: InsertErrorLog;1 - Script Date: 11/14/2008 4:31:43 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertErrorLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertErrorLog]
GO
CREATE PROCEDURE dbo.InsertErrorLog 
	
	(
	@ErrorMessage varchar(2000)
	)
	
AS
	SET NOCOUNT ON 
	
	INSERT INTO ErrorLog
	                      ([Timestamp], [ErrorMessage])
	VALUES     (GETDATE(),@ErrorMessage)
	
	RETURN


GO
