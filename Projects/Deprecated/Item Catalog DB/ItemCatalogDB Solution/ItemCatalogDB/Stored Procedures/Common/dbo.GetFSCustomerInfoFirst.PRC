SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetFSCustomerInfoFirst]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetFSCustomerInfoFirst]
GO


CREATE PROCEDURE dbo.GetFSCustomerInfoFirst
AS 

SELECT FSCustomer.Customer_ID, FSCustomer.Organization_ID, FSOrganization.OrganizationName, FSCustomer.Customer_Code, 
       FSCustomer.CustomerName, FSCustomer.Address_Line_1, FSCustomer.Address_Line_2, FSCustomer.City, FSCustomer.State, 
       FSCustomer.Zip_Code, FSCustomer.Phone, FSCustomer.Phone_Ext, FSCustomer.Fax, FSCustomer.Email_Address, 
       FSCustomer.Birthday, FSCustomer.Comment, FSCustomer.ActiveCustomer, FSCustomer.User_ID     
FROM  FSCustomer  INNER JOIN FSOrganization ON(FSCustomer.Organization_ID = FSOrganization.Organization_ID) 
WHERE FSCustomer.Customer_ID = (SELECT MIN(Customer_ID) FROM FSCustomer)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


