SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOriginInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateOriginInfo]
GO


CREATE PROCEDURE dbo.UpdateOriginInfo
@Origin_ID int,
@Origin_Name varchar(50)
AS 

UPDATE ItemOrigin
SET User_ID = NULL,
    Origin_Name = @Origin_Name
WHERE Origin_ID = @Origin_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

