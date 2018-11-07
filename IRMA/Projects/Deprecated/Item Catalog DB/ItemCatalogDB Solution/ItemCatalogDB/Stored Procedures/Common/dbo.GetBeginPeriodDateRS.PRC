SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetBeginPeriodDateRS') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetBeginPeriodDateRS
GO


CREATE PROCEDURE dbo.GetBeginPeriodDateRS
    @Date datetime = null

AS

BEGIN
    SET NOCOUNT ON

	DECLARE @Error_No int, @PeriodBeginDate datetime
    SELECT @Error_No = 0
	        

    SELECT @PeriodBeginDate = dbo.fn_PeriodBeginDate(ISNULL(@Date, GETDATE()))
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        SET NOCOUNT OFF
        RAISERROR ('GetBeginPeriodDateRS, GetBeginPeriodDate, failed with Error = %d', 16, 1, @Error_No)
        RETURN
    END
    ELSE
        SELECT @PeriodBeginDate As PeriodBeginDate

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
