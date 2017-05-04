-- =============================================
-- Author:		Alex Babichev
-- Create date: 10/23/2012
-- Description:	Weekly Process that Moves log data from Applog to ApplogArchive
-- Update History

--TFS #		Date		Who			Comments

-- =============================================
/*

*/

CREATE PROCEDURE [dbo].[MoveAppLogDatatoArchive]
    @Days int,
    @Level varchar(50) 
AS
  BEGIN
      SET nocount ON

      DECLARE @maxIDtoDelete INT,
      @CountRows INT
       
      
      SELECT @maxIDtoDelete = ISNULL(max(ID),0) from AppLog where Level = @Level and LogDate < DATEADD(DAY,0-@Days,GETDATE())
		
      BEGIN try
          BEGIN TRAN

          INSERT INTO AppLogArchive
                      (
					   [LogDate]
					  ,[ApplicationID]
					  ,[HostName]
					  ,[UserName]
					  ,[Thread]
					  ,[Level]
					  ,[Logger]
					  ,[Message]
					  ,[Exception]
					  ,[InsertDate]
					  )

          SELECT	   [LogDate]
					  ,[ApplicationID]
					  ,[HostName]
					  ,[UserName]
					  ,[Thread]
					  ,[Level]
					  ,[Logger]
					  ,[Message]
					  ,[Exception]
					  ,[InsertDate]
					   FROM [AppLog]
          WHERE  ID <= @maxIDtoDelete AND Level = @Level
		set @CountRows = @@ROWCOUNT
          DELETE AppLog
          WHERE  ID <= @maxIDtoDelete AND Level = @Level
          COMMIT         
         return @CountRows 
      END try

      BEGIN catch
          ROLLBACK
      END catch
  END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MoveAppLogDatatoArchive] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MoveAppLogDatatoArchive] TO [IRMASchedJobsRole]
    AS [dbo];

