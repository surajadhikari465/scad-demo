SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnlockFSCustomer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnlockFSCustomer]
GO


CREATE PROCEDURE dbo.UnlockFSCustomer 
@Customer_ID int 
AS 

UPDATE FSCustomer SET 
User_ID = NULL 
WHERE Customer_ID = @Customer_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


