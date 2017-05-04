SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetFQSOrgModifiedCreatedTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetFQSOrgModifiedCreatedTime]
GO

/****** Object:  Stored Procedure dbo.SetFQSOrgModifiedCreatedTime    Script Date: 4/25/99 10:35:03 PM ******/
CREATE PROCEDURE dbo.SetFQSOrgModifiedCreatedTime
@OrgNumber int
AS
UPDATE FQSOrganization
SET ModifiedCreatedTime = GetDate()
WHERE OrgNumber = @OrgNumber

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


