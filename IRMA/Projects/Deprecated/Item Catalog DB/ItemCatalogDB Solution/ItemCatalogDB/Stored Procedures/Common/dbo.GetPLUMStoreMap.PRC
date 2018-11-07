SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetPLUMStoreMap') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetPLUMStoreMap
GO


CREATE PROCEDURE dbo.GetPLUMStoreMap

AS

BEGIN
    SET NOCOUNT ON

    SELECT Store_No, PLUMStoreNo
    FROM Store (nolock)
    WHERE PLUMStoreNo IS NOT NULL

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



