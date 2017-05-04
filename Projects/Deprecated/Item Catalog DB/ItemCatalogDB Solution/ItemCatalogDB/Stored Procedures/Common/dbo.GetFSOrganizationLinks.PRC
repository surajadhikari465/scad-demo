SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetFSOrganizationLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetFSOrganizationLinks]
GO


CREATE PROCEDURE dbo.GetFSOrganizationLinks
@Organization_ID int 
AS 

SELECT Organization_ID AS Organization_ID 
FROM FSCustomer 
WHERE Organization_ID = @Organization_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


