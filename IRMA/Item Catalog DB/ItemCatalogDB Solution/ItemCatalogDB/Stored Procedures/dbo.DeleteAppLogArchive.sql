/****** Object:  StoredProcedure [dbo].[DeleteAppLogDataArchive]    Script Date: 10/26/2012 09:21:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAppLogDataArchive]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAppLogDataArchive]
GO


/****** Object:  StoredProcedure [dbo].[DeleteAppLogDataArchive]    Script Date: 10/26/2012 09:21:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


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


