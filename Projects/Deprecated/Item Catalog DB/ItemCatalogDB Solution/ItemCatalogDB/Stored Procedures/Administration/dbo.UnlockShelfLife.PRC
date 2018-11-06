SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnlockShelfLife]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnlockShelfLife]
GO


CREATE PROCEDURE dbo.UnlockShelfLife 
@ShelfLife_ID int 
AS 

UPDATE ItemShelfLife 
SET User_ID = NULL 
WHERE ShelfLife_ID = @ShelfLife_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


