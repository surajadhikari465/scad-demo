SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetFSOrganizationInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetFSOrganizationInfo]
GO

CREATE PROCEDURE dbo.GetFSOrganizationInfo
@Organization_ID int
AS 
SELECT Organization_ID, OrganizationName, Address_Line_1, Address_Line_2, City, State, Zip_Code, Phone, Phone_Ext, Fax, Contact, Email_Address,Comment, ActiveOrganization, User_ID
FROM  FSOrganization
WHERE Organization_ID = @Organization_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

