SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteFSCustomer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteFSCustomer]
GO


CREATE PROCEDURE dbo.DeleteFSCustomer 
@Customer_Id int 
AS 

DELETE 
FROM FSCustomer 
WHERE Customer_ID = @Customer_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

