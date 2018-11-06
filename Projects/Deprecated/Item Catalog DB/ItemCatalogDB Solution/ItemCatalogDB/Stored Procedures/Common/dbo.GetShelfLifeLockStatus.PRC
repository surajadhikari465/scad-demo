SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetShelfLifeLockStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetShelfLifeLockStatus]
GO


CREATE PROCEDURE dbo.GetShelfLifeLockStatus 
@ShelfLife_ID int 
AS 

SELECT User_ID 

FROM ItemShelfLife 

WHERE ShelfLife_ID = @ShelfLife_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


