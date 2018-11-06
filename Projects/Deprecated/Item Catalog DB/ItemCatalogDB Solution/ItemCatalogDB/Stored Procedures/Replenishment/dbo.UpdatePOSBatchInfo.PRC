SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdatePOSBatchInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdatePOSBatchInfo]
GO


CREATE PROCEDURE dbo.UpdatePOSBatchInfo
@Store_No int
AS 

UPDATE Store
SET BatchID = BatchID + 1
--    BatchRecords = 0
WHERE Store_No = @Store_No 
--	  AND BatchRecords > 0
-- Note: As of version V2.2.0, the replenishment code no longer maintains the BatchRecords count in the database
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

