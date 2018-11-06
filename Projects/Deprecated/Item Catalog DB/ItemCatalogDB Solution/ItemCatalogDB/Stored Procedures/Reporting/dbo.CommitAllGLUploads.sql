SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CommitAllGLUploads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CommitAllGLUploads]
GO

CREATE PROCEDURE dbo.CommitAllGLUploads 
AS 

DECLARE @StartDate datetime
DECLARE @EndDate datetime

SELECT @StartDate = GETDATE()
SELECT @EndDate = GETDATE()

EXEC CommitGLUploadTransfers @StartDate, @EndDate, NULL
EXEC CommitGLUploadDistributions NULL,NULL,@StartDate,@EndDate
EXEC CommitGLUploadInventoryAdjustment NULL,NULL,NULL,NULL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO