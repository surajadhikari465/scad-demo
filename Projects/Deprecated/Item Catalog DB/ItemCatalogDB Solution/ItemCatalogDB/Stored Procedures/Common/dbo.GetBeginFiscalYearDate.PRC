SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBeginFiscalYearDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBeginFiscalYearDate]
GO


CREATE PROCEDURE dbo.GetBeginFiscalYearDate
    @InDate datetime,
    @BP_Date datetime OUTPUT

AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @BP_Date = dbo.fn_FiscalYearBeginDate(@InDate)
        
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

