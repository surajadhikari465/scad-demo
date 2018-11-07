SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnlockOrderHeader]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnlockOrderHeader]
GO


CREATE PROCEDURE dbo.UnlockOrderHeader 
@OrderHeader_ID int 
AS 

UPDATE OrderHeader 
SET User_ID = NULL 
WHERE OrderHeader_ID = @OrderHeader_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


