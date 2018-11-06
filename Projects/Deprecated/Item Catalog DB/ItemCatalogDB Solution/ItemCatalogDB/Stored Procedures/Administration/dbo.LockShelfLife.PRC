SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LockShelfLife]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LockShelfLife]
GO


CREATE PROCEDURE dbo.LockShelfLife 
@ShelfLife_ID int, 
@User_ID int  
AS 

UPDATE ItemShelfLife 
SET User_ID = @User_ID 
WHERE ShelfLife_ID = @ShelfLife_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


