SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteOldVCAI_Exception]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteOldVCAI_Exception]
GO


CREATE PROCEDURE dbo.DeleteOldVCAI_Exception 

AS 
DECLARE @CutOffDate as datetime
SELECT @CutOffDate = dateadd(wk,-1, getdate())

DELETE 
FROM VendorCostHistoryExceptions
WHERE LastModified <= @cutoffdate and ExStatus = -1


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


