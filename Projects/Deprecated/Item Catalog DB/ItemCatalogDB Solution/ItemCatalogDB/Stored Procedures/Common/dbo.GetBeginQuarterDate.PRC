SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBeginQuarterDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBeginQuarterDate]
GO


CREATE PROCEDURE dbo.GetBeginQuarterDate
    @InDate datetime,
    @BP_Date datetime OUTPUT

AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @BP_Date = dbo.fn_QuarterBeginDate(@InDate)
        
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

