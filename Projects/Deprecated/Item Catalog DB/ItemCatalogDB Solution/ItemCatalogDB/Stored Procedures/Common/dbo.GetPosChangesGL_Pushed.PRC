SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPosChangesGL_Pushed]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPosChangesGL_Pushed]
GO


CREATE PROCEDURE dbo.GetPosChangesGL_Pushed
AS

SELECT DISTINCT Sales_Date, Store_No 
FROM POSChanges
WHERE GL_Pushed = 0
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



