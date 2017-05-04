SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteFSCustomers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteFSCustomers]
GO

CREATE PROCEDURE dbo.DeleteFSCustomers
@Organization_Id int 
AS 
DELETE 
FROM FSCustomer 
WHERE Organization_ID = @Organization_ID 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

