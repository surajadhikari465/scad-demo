SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AppLogInsertEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AppLogInsertEntry]
GO


CREATE PROCEDURE [dbo].[AppLogInsertEntry]
	@logDate datetime
	,@appID [uniqueidentifier]
	,@thread varchar(255)
	,@logLevel varchar(50)
	,@logger varchar(255)
	,@message varchar(4000)
	,@exception varchar(2000)
AS 

BEGIN

	INSERT INTO AppLog ([LogDate],[ApplicationID],[HostName],[UserName],[Thread],[Level],[Logger],[Message],[Exception]) 
		VALUES (@logDate, @appID, HOST_NAME(), SUSER_NAME(), @thread, @logLevel, @logger, @message, @exception)
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


