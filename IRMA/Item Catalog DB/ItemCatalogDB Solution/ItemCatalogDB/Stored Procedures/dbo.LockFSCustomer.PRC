SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LockFSCustomer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LockFSCustomer]
GO


CREATE PROCEDURE dbo.LockFSCustomer
@Customer_ID int, 
@User_ID int  
AS 

UPDATE FSCustomer 
SET User_ID = @User_ID 
WHERE Customer_ID = @Customer_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


