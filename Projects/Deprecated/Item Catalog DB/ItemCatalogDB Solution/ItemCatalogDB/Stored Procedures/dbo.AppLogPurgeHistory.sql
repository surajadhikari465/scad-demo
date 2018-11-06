SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AppLogPurgeHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AppLogPurgeHistory]
GO


CREATE PROCEDURE [dbo].[AppLogPurgeHistory]
    @applicationID [uniqueidentifier],
    @daysToKeep int
AS 

BEGIN

	if @daysToKeep > 0
	begin
		delete from applog
		where ApplicationID = @applicationID
		and logdate < getdate() - @daysToKeep
	end

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


