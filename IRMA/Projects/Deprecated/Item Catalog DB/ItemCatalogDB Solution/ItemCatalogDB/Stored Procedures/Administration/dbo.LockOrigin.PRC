SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LockOrigin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LockOrigin]
GO


CREATE PROCEDURE dbo.LockOrigin 
@Origin_ID int, 
@User_ID int 
AS 

UPDATE ItemOrigin 
SET User_ID = @User_ID 
WHERE Origin_ID = @Origin_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



