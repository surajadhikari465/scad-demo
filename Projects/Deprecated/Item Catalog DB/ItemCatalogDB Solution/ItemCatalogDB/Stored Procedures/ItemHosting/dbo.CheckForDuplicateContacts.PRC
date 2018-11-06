SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateContacts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateContacts]
GO


CREATE PROCEDURE dbo.CheckForDuplicateContacts
@Vendor_ID int,
@Contact_ID int,
@Contact_Name varchar(25) 
AS 

SELECT COUNT(*) AS ContactCount 
FROM Contact 
WHERE Contact_Name = @Contact_Name AND 
      Vendor_ID = @Vendor_ID AND 
      Contact_ID <> @Contact_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


