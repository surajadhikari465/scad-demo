SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetBeginQuarterDateRS') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetBeginQuarterDateRS
GO


CREATE PROCEDURE dbo.GetBeginQuarterDateRS
    @Date datetime = null
AS

BEGIN
    SET NOCOUNT ON

	DECLARE @Error_No int, @QuarterBeginDate datetime
    SELECT @Error_No = 0
	        

    SELECT @QuarterBeginDate = dbo.fn_QuarterBeginDate(ISNULL(@Date, GETDATE()))
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        SET NOCOUNT OFF
        RAISERROR ('GetBeginQuarterDateRS, GetBeginPeriodDate, failed with Error = %d', 16, 1, @Error_No)
        RETURN
    END
    ELSE
        SELECT @QuarterBeginDate As QuarterBeginDate

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
