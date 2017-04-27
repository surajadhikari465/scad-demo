SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LockCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LockCategory]
GO


CREATE PROCEDURE dbo.LockCategory 
@Category_ID int, 
@User_ID int 
AS 

UPDATE ItemCategory 
SET User_ID = @User_ID 
WHERE Category_ID = @Category_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


