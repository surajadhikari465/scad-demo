SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetGLQueue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetGLQueue]
GO


CREATE PROCEDURE dbo.GetGLQueue
AS

SELECT TOP 1 PushQueue_ID, Start_Date, End_Date, Modified_By, Distributions, Transfers
FROM GLPushQueue
ORDER BY PushQueue_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

