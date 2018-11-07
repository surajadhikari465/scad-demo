SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSystemDateCmd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSystemDateCmd]
GO


CREATE PROCEDURE dbo.GetSystemDateCmd 
    @Date datetime OUTPUT
AS 
BEGIN
    SET @Date = GETDATE()
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

