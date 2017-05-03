SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrganizationInfoLast]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrganizationInfoLast]
GO


CREATE PROCEDURE dbo.GetOrganizationInfoLast
AS 

SELECT Organization_ID, OrganizationName, Address_Line_1, Address_Line_2, City, State, Zip_Code, Phone, Phone_Ext, Fax, 
       Contact, Email_Address,Comment, ActiveOrganization, User_ID
FROM  FSOrganization
WHERE Organization_ID = (SELECT MAX(Organization_ID) FROM FSOrganization)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


