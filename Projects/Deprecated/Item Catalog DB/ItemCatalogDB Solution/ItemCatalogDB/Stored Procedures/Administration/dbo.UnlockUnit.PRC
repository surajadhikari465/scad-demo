SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnlockUnit]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnlockUnit]
GO


CREATE PROCEDURE dbo.UnlockUnit 
@Unit_ID int 
AS 

UPDATE ItemUnit 
SET User_ID = NULL 
WHERE Unit_ID = @Unit_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



