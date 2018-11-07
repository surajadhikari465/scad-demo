SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderHeaderLockStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderHeaderLockStatus]
GO


CREATE PROCEDURE dbo.GetOrderHeaderLockStatus 
@OrderHeader_ID int 
AS 

SELECT User_ID 

FROM OrderHeader 

WHERE OrderHeader_ID = @OrderHeader_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


