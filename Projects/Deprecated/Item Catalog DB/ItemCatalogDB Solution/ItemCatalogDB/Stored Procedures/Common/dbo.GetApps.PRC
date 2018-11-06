SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetApps]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetApps]
GO


CREATE PROCEDURE dbo.GetApps
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT App.AppID, App.AppName
    FROM App
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


