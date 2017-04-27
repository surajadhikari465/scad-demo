SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetStoreMobilePrinter') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetStoreMobilePrinter
GO


CREATE PROCEDURE dbo.GetStoreMobilePrinter
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON

    SELECT MobilePrinter.MobilePrinterID, NetworkName
    FROM MobilePrinter
    INNER JOIN
        Store_MobilePrinter
        ON Store_MobilePrinter.MobilePrinterID = MobilePrinter.MobilePrinterID AND Store_No = @Store_No 

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


