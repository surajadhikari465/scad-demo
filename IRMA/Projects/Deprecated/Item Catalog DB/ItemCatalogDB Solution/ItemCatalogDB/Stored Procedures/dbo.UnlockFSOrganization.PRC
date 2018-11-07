SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnlockFSOrganization]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnlockFSOrganization]
GO


CREATE PROCEDURE dbo.UnlockFSOrganization 
@Organization_ID int 
AS 

UPDATE FSOrganization SET 
User_ID = NULL 
WHERE Organization_ID = @Organization_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


