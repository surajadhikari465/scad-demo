SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBrandLockStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBrandLockStatus]
GO


CREATE PROCEDURE dbo.GetBrandLockStatus 
@Brand_ID int 
AS 

SELECT User_ID 

FROM ItemBrand 

WHERE Brand_ID = @Brand_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


