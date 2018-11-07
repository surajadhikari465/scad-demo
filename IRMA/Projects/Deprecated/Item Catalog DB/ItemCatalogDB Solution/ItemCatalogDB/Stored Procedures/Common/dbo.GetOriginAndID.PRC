SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOriginAndID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOriginAndID]
GO


CREATE PROCEDURE dbo.GetOriginAndID 
AS 

SELECT Origin_ID, Origin_Name 
FROM ItemOrigin 
ORDER BY Origin_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

