SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LockUnit]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LockUnit]
GO


CREATE PROCEDURE dbo.LockUnit 
@Unit_ID int, 
@User_ID int 
AS 

UPDATE ItemUnit 
SET User_ID = @User_ID 
WHERE Unit_ID = @Unit_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


