SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LockFSOrganization]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LockFSOrganization]
GO


CREATE PROCEDURE dbo.LockFSOrganization
@Organization_ID int, 
@User_ID int  
AS 

UPDATE FSOrganization 
SET User_ID = @User_ID 
WHERE Organization_ID = @Organization_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


