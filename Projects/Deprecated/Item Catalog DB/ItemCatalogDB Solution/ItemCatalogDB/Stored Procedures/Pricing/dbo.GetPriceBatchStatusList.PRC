SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetPriceBatchStatusList') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetPriceBatchStatusList
GO


CREATE PROCEDURE dbo.GetPriceBatchStatusList

AS

BEGIN
    SET NOCOUNT ON

    SELECT PriceBatchStatusID, PriceBatchStatusDesc
    FROM PriceBatchStatus (nolock)
    ORDER BY PriceBatchStatusID

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


