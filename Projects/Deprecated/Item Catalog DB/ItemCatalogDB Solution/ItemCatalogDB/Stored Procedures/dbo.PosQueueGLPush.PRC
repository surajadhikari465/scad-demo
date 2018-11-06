SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PosQueueGLPush]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PosQueueGLPush]
GO


CREATE PROCEDURE dbo.PosQueueGLPush
@Store_No int,
@Sales_Date datetime,
@User_ID int
AS

UPDATE POSChanges
SET GL_InQueue = 1, Modified_By = @User_ID
WHERE Aggregated = 1 AND Store_No = @Store_No AND Sales_Date = @Sales_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

