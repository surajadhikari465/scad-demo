SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUnitLockStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUnitLockStatus]
GO


CREATE PROCEDURE dbo.GetUnitLockStatus 
@Unit_ID int 
AS 

SELECT User_ID 

FROM ItemUnit 

WHERE Unit_ID = @Unit_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


