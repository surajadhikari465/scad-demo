SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteFSOrganization]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteFSOrganization]
GO


CREATE PROCEDURE dbo.DeleteFSOrganization 
@Organization_Id int 
AS 

BEGIN

  DELETE 
  FROM FSCustomer 
  WHERE Organization_ID = @Organization_ID 

  DELETE 
  FROM FSOrganization 
  WHERE Organization_ID = @Organization_ID 

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


