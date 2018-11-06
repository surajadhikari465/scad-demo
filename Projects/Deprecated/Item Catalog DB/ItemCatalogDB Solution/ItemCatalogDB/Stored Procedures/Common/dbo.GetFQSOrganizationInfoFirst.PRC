SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetFQSOrganizationInfoFirst]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetFQSOrganizationInfoFirst]
GO

CREATE PROCEDURE dbo.GetFQSOrganizationInfoFirst
AS 
SELECT OrgNumber, DisabledFlag, Name, Address1, Address2, City, State, ZipCode, Phone, PhoneExtension, Fax, Email, Miscellaneous, Contact
FROM FQSOrganization
WHERE OrgNumber = (SELECT MIN(OrgNumber) FROM FQSOrganization)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



