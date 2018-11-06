-- =============================================
-- Author:		Alex Babichev
-- Create date: 10/23/2012
-- Description:	Weekly Process that deletes old log data from ApplogArchive
-- Update History

--TFS #		Date		Who			Comments

-- =============================================
/*

*/

CREATE PROCEDURE [dbo].[DeleteAppLogDataArchive]
    @Days int,
    @Level varchar(50) 
AS
  BEGIN
      SET nocount ON      
       DECLARE @CountRows INT
       
          DELETE AppLogArchive
          Where Level = @Level and LogDate < DATEADD(DAY,0-@Days,GETDATE())
          set @CountRows = @@ROWCOUNT         
         return @CountRows 
  END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteAppLogDataArchive] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteAppLogDataArchive] TO [IRMASchedJobsRole]
    AS [dbo];

