SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RemoveGLQueue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RemoveGLQueue]
GO


CREATE PROCEDURE dbo.RemoveGLQueue
@PushQueue_ID int
AS

DELETE
FROM GLPushQueue
WHERE PushQueue_ID = @PushQueue_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


