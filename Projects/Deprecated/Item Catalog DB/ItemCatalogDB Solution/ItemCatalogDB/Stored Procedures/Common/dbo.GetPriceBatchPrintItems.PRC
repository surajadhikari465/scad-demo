SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetPriceBatchPrintItems') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetPriceBatchPrintItems
GO


CREATE PROCEDURE dbo.GetPriceBatchPrintItems
    @PriceBatchHeaderID int,
    @PrintSign bit
AS

BEGIN
    SET NOCOUNT ON

    SELECT Item_Key
    FROM PriceBatchDetail PBD
    WHERE PBD.PriceBatchHeaderID = @PriceBatchHeaderID
        AND PrintSign = @PrintSign
    GROUP BY Item_Key

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


