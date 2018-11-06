SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteContacts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteContacts]
GO


CREATE PROCEDURE dbo.DeleteContacts 
@Vendor_Id int 
AS 

DELETE 
FROM Contact 
WHERE Vendor_ID = @Vendor_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

