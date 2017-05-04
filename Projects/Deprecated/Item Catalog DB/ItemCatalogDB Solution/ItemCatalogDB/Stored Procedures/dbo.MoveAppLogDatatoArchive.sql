
/****** Object:  StoredProcedure [dbo].[MoveAppLogDatatoArchive]    Script Date: 10/26/2012 09:23:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoveAppLogDatatoArchive]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MoveAppLogDatatoArchive]
GO

/****** Object:  StoredProcedure [dbo].[MoveAppLogDatatoArchive]    Script Date: 10/26/2012 09:23:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


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


