SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LockOrderHeader]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LockOrderHeader]
GO


CREATE PROCEDURE dbo.LockOrderHeader 
@OrderHeader_ID int, 
@User_ID int 
AS 

UPDATE OrderHeader 
SET User_ID = @User_ID 
WHERE OrderHeader_ID = @OrderHeader_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


