SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetContactInfoFirst]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetContactInfoFirst]
GO


CREATE PROCEDURE dbo.GetContactInfoFirst
@Vendor_ID int
AS 

SELECT Contact_ID, Contact_Name, Phone, Phone_Ext, Fax , Email
FROM Contact 
WHERE Contact_ID = (SELECT MIN(Contact_ID) FROM Contact WHERE Vendor_ID = @Vendor_ID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


