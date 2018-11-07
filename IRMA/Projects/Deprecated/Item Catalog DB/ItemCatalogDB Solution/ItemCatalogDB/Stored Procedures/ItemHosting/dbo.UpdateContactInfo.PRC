SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateContactInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateContactInfo]
GO


CREATE PROCEDURE dbo.UpdateContactInfo
@Contact_ID int,
@Contact_Name varchar(50),
@Phone varchar(14),
@Phone_Ext varchar(5),
@Fax varchar(14),
@Email varchar(50)
AS 

UPDATE Contact 
SET Contact_Name = @Contact_Name,
    Phone = @Phone,
    Phone_Ext = @Phone_Ext,
    Fax = @Fax,
	Email = @Email
WHERE Contact_ID = @Contact_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


