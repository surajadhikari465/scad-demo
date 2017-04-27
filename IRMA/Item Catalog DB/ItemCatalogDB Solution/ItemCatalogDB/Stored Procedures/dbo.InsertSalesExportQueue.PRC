SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.InsertSalesExportQueue') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.InsertSalesExportQueue
GO


CREATE PROCEDURE dbo.InsertSalesExportQueue
    @Store_No int,
    @Date datetime
AS

BEGIN
    SET NOCOUNT ON

    IF NOT EXISTS(SELECT * FROM SalesExportQueue WHERE Store_No = @Store_no and Date_Key = @Date)
        INSERT INTO SalesExportQueue VALUES(@Store_No, @Date)

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

