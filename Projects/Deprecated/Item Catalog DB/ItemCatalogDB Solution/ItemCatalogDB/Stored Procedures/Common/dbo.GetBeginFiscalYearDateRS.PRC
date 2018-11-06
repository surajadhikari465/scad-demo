SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetBeginFiscalYearDateRS') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetBeginFiscalYearDateRS
GO


CREATE PROCEDURE dbo.GetBeginFiscalYearDateRS
    @Date datetime = null
AS

BEGIN
    SET NOCOUNT ON

	DECLARE @Error_No int, @FiscalYearBeginDate datetime
    SELECT @Error_No = 0
	        

    SELECT @FiscalYearBeginDate = dbo.fn_FiscalYearBeginDate(ISNULL(@Date, GETDATE()))
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        SET NOCOUNT OFF
        RAISERROR ('GetBeginFiscalYearDateRS, GetBeginPeriodDate, failed with Error = %d', 16, 1, @Error_No)
        RETURN
    END
    ELSE
        SELECT @FiscalYearBeginDate As FiscalYearBeginDate

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
