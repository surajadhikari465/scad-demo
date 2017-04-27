SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateOrganizations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateOrganizations]
GO


CREATE PROCEDURE dbo.CheckForDuplicateOrganizations 
@Organization_ID int,
@OrganizationName varchar(25) 
AS 

SELECT COUNT(*) AS OrganizationCount 
FROM FSOrganization
WHERE OrganizationName = @OrganizationName AND Organization_Id <> @Organization_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

