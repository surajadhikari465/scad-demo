SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateOrigins]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateOrigins]
GO


CREATE PROCEDURE dbo.CheckForDuplicateOrigins 
@Origin_ID int, 
@Origin_Name varchar(25) 
AS 

SELECT COUNT(*) AS OriginCount 
FROM ItemOrigin 
WHERE Origin_Name = @Origin_Name AND 
      Origin_ID <> @Origin_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

