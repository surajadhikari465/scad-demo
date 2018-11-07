SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBeginPeriodDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBeginPeriodDate]
GO


CREATE PROCEDURE dbo.GetBeginPeriodDate
    @InDate datetime,
    @BP_Date datetime OUTPUT

AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @BP_Date = dbo.fn_PeriodBeginDate(@InDate)
        
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

