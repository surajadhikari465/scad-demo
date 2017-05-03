SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertOrganization]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertOrganization]
GO


CREATE PROCEDURE dbo.InsertOrganization
@OrganizationName varchar(50)
AS 

INSERT INTO FSOrganization (OrganizationName)
VALUES (@OrganizationName)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


