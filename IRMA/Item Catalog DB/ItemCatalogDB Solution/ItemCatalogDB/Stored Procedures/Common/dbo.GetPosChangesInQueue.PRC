SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPosChangesInQueue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPosChangesInQueue]
GO


CREATE PROCEDURE dbo.GetPosChangesInQueue
AS

SELECT TOP 1 Store_No, Sales_Date, Modified_By
FROM POSChanges
WHERE GL_InQueue = 1
ORDER BY Sales_Date, Store_No
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



