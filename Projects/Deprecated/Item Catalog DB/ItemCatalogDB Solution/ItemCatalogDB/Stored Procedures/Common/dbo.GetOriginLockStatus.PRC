SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOriginLockStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOriginLockStatus]
GO


CREATE PROCEDURE dbo.GetOriginLockStatus 
@Origin_ID int 
AS 

SELECT User_ID 

FROM ItemOrigin 

WHERE Origin_ID = @Origin_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



