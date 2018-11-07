-- =============================================
-- Author:		Alex Babichev
-- Create date: 10/25/2012
-- Description:	Weekly Process that Moves log data from Applog to ApplogArchive and deletes old data from ApplogArchive
-- Update History

--TFS #		Date		Who			Comments

-- =============================================
/*

*/

CREATE PROCEDURE [dbo].[PurgeLog]
    @Debug int,
    @DebugArchive int,
    @Info int,
    @InfoArchive int, 
    @Warn int,
    @WarnArchive int, 
    @Error int,
    @ErrorArchive int, 
    @Fatal int,
    @FatalArchive int  
AS
  BEGIN
       SET nocount OFF
  DECLARE
		@ApplogCounter INT = 0,
		@ApplogArchiveCounter INT = 0, 
		@ApplogSummary INT = 0,
		@ApplogArchiveSummary INT = 0 
 
     EXEC @ApplogCounter = dbo.MoveAppLogDatatoArchive @Debug ,'Debug'    
     SET  @ApplogSummary = @ApplogSummary + @ApplogCounter
     EXEC @ApplogCounter = dbo.MoveAppLogDatatoArchive @Info ,'Info' 
     SET  @ApplogSummary = @ApplogSummary + @ApplogCounter    
     EXEC @ApplogCounter = dbo.MoveAppLogDatatoArchive @Warn ,'Warn' 
     SET  @ApplogSummary = @ApplogSummary + @ApplogCounter    
     EXEC @ApplogCounter = dbo.MoveAppLogDatatoArchive @Error ,'Error'  
     SET  @ApplogSummary = @ApplogSummary + @ApplogCounter   
     EXEC @ApplogCounter = dbo.MoveAppLogDatatoArchive @Fatal ,'Fatal'
     SET  @ApplogSummary = @ApplogSummary + @ApplogCounter

	 EXEC @ApplogArchiveCounter = dbo.DeleteAppLogDataArchive @DebugArchive ,'Debug' 
	 SET  @ApplogArchiveSummary = @ApplogArchiveSummary + @ApplogArchiveCounter   
     EXEC @ApplogArchiveCounter = dbo.DeleteAppLogDataArchive @InfoArchive ,'Info'     
     SET  @ApplogArchiveSummary = @ApplogArchiveSummary + @ApplogArchiveCounter  
     EXEC @ApplogArchiveCounter = dbo.DeleteAppLogDataArchive @WarnArchive ,'Warn'   
     SET  @ApplogArchiveSummary = @ApplogArchiveSummary + @ApplogArchiveCounter    
     EXEC @ApplogArchiveCounter = dbo.DeleteAppLogDataArchive @ErrorArchive ,'Error'  
     SET  @ApplogArchiveSummary = @ApplogArchiveSummary + @ApplogArchiveCounter     
     EXEC @ApplogArchiveCounter = dbo.DeleteAppLogDataArchive @FatalArchive ,'Fatal'
     SET  @ApplogArchiveSummary = @ApplogArchiveSummary + @ApplogArchiveCounter  
     
     select @ApplogSummary as applog,@ApplogArchiveSummary as applogarchive
      
  END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurgeLog] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurgeLog] TO [IRMASchedJobsRole]
    AS [dbo];

