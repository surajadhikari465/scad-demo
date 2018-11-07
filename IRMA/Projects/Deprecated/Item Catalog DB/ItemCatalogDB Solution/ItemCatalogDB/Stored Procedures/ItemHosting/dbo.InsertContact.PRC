SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertContact]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertContact]
GO


CREATE PROCEDURE dbo.InsertContact
@Vendor_ID int,
@Contact_Name varchar(50)
AS 

INSERT INTO Contact (Vendor_ID, Contact_Name)
VALUES (@Vendor_ID, @Contact_Name)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


