SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnlockCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnlockCategory]
GO


CREATE PROCEDURE dbo.UnlockCategory 
@Category_ID int 
AS 

UPDATE ItemCategory 
SET User_ID = NULL 
WHERE Category_ID = @Category_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


